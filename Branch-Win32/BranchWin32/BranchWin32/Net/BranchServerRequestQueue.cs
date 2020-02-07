using BranchSdk.CrossPlatform;
using BranchSdk.Net.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace BranchSdk.Net {
    public static class BranchServerRequestQueue {
        public static int MaxRequestsAtOnce = 25;

        private static List<BranchServerRequest> queue = new List<BranchServerRequest>();
        private static Queue<BranchServerRequest> runningQueue = new Queue<BranchServerRequest>();

        public static void AddRequest(BranchServerRequest request) {
            queue.Add(request);
        }

        public static bool RunQueue() {
            if (runningQueue.Count > 0) return false;

            for (int i = 0; i < queue.Count && i < MaxRequestsAtOnce; i++) {
                runningQueue.Enqueue(queue[i]);
            }

            queue.RemoveRange(0, runningQueue.Count);

            Thread thread = new Thread(TaskWorking);
            thread.Start();
            return true;
        }

        public static void ClearPendingRequests() {
            queue.Clear();
        }

        private static void TaskWorking() {
            while (runningQueue.Count > 0) {
                BranchServerRequest request = runningQueue.Dequeue();
                HandleRequest(request, null);
            }
        }

        private static void HandleFailRequestGet(BranchServerRequest request, string requestUri, BranchWebClient client, HttpWebResponse httpResponse, Action<BranchRequestResponse> callback) {
            string rawError = string.Empty;

            using (StreamReader r = new StreamReader(httpResponse.GetResponseStream())) {
                rawError = r.ReadToEnd();
            }

            if (LibraryAdapter.GetPrefHelper().GetMaxRetries() < 1) {
                request.OnFailed(rawError, (int)httpResponse.StatusCode);
                Console.WriteLine("Request Error: " + rawError);

                if (callback != null)
                    callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
                return;
            }

            for (int i = 0; i < LibraryAdapter.GetPrefHelper().GetMaxRetries(); i++) {
                try {
                    string responseContent = client.DownloadString(GetUriWithParameters(request.RequestUrl(), request.Parameters));
                    request.OnSuccess(responseContent);

                    if (callback != null)
                        callback.Invoke(new BranchRequestResponse(responseContent, string.Empty));
                } catch (WebException e) {
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if ((int)response.StatusCode >= 500) {
                            Console.WriteLine("Failed request, retry: " + i);
                            Thread.Sleep(LibraryAdapter.GetPrefHelper().GetRetryInterval());
                            continue;
                        } else {
                            using (StreamReader r = new StreamReader(e.Response.GetResponseStream())) {
                                string error = r.ReadToEnd();
                                request.OnFailed(error, (int)response.StatusCode);
                                Console.WriteLine("Request Error: " + error);

                                if (callback != null)
                                    callback.Invoke(new BranchRequestResponse(string.Empty, error));
                            }
                        }
                    } else {
                        if (callback != null)
                            callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                    }
                }
            }

            if (callback != null)
                callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
        }

        private static void HandleFailRequestPost(BranchServerRequest request, string requestUri, BranchWebClient client, HttpWebResponse httpResponse, Action<BranchRequestResponse> callback) {
            string rawError = string.Empty;

            using (StreamReader r = new StreamReader(httpResponse.GetResponseStream())) {
                rawError = r.ReadToEnd();
            }

            if (LibraryAdapter.GetPrefHelper().GetMaxRetries() < 1) {
                request.OnFailed(rawError, (int)httpResponse.StatusCode);
                Console.WriteLine("Request Error: " + rawError);

                if (callback != null)
                    callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
                return;
            }

            for (int i = 0; i < LibraryAdapter.GetPrefHelper().GetMaxRetries(); i++) {
                try {
                    string responseContent = client.UploadString(request.RequestUrl(), "POST", Json.Serialize(request.PostData));
                    request.OnSuccess(responseContent);

                    if (callback != null)
                        callback.Invoke(new BranchRequestResponse(responseContent, string.Empty));
                } catch(WebException e) {
                    var response = e.Response as HttpWebResponse;
                    if (response != null) {
                        if ((int)response.StatusCode >= 500) {
                            Console.WriteLine("Failed request, retry: " + i);
                            Thread.Sleep(LibraryAdapter.GetPrefHelper().GetRetryInterval());
                            continue;
                        } else {
                            using (StreamReader r = new StreamReader(e.Response.GetResponseStream())) {
                                string error = r.ReadToEnd();
                                request.OnFailed(error, (int)response.StatusCode);
                                Console.WriteLine("Request Error: " + error);

                                if (callback != null)
                                    callback.Invoke(new BranchRequestResponse(string.Empty, error));
                            }
                        }
                    } else {
                        if (callback != null)
                            callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                    }
                }
            }

            if (callback != null)
                callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
        }

        public static void HandleRequest(BranchServerRequest request, Action<BranchRequestResponse> callback) {
            //if tracking disabled and request doesnt support execute without tracking properties
            if (BranchTrackingController.TrackingDisabled && !request.PrepareExecuteWithoutTracking()) {
                BranchError error = new BranchError(string.Empty, BranchError.ERR_BRANCH_TRACKING_DISABLED);
                request.OnFailed(string.Empty, error.GetErrorCode());

                if (callback != null)
                    callback.Invoke(new BranchRequestResponse(string.Empty, error.GetMessage()));
                return;
            }

            using (BranchWebClient client = new BranchWebClient()) {
                client.Timeout = LibraryAdapter.GetPrefHelper().GetNetworkTimeout();

                if (request.RequestType == RequestTypes.GET) {
                    try {
                        string responseContent = client.DownloadString(GetUriWithParameters(request.RequestUrl(), request.Parameters));
                        request.OnSuccess(responseContent);

                        if (callback != null)
                            callback.Invoke(new BranchRequestResponse(responseContent, string.Empty));
                    } catch (WebException e) {
                        if (e.Status == WebExceptionStatus.ProtocolError) {
                            var response = e.Response as HttpWebResponse;
                            if (response != null) {
                                Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
                                if ((int)response.StatusCode >= 500) {
                                    HandleFailRequestGet(request, request.RequestUrl(), client, response, callback);
                                } else {
                                    using (StreamReader r = new StreamReader(e.Response.GetResponseStream())) {
                                        string rawError = r.ReadToEnd();
                                        request.OnFailed(rawError, (int)response.StatusCode);
                                        Console.WriteLine("Request Error: " + rawError);

                                        if (callback != null)
                                            callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
                                    }
                                }
                            } else {
                                if (callback != null)
                                    callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                            }
                        } else {
                            if (callback != null)
                                callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                        }
                    }
                } else {
                    Console.WriteLine("Post data: " + Json.Serialize(request.PostData));

                    try {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        string responseContent = client.UploadString(request.RequestUrl(), "POST", Json.Serialize(request.PostData));
                        request.OnSuccess(responseContent);

                        if (callback != null)
                            callback.Invoke(new BranchRequestResponse(responseContent, string.Empty));
                    } catch (WebException e) {
                        if (e.Status == WebExceptionStatus.ProtocolError) {
                            var response = e.Response as HttpWebResponse;
                            if (response != null) {
                                Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
                                if ((int)response.StatusCode >= 500) {
                                    HandleFailRequestPost(request, request.RequestUrl(), client, response, callback);
                                } else {
                                    using (StreamReader r = new StreamReader(e.Response.GetResponseStream())) {
                                        string rawError = r.ReadToEnd();
                                        request.OnFailed(rawError, (int)response.StatusCode);
                                        Console.WriteLine("Request Error: " + rawError);

                                        if (callback != null)
                                            callback.Invoke(new BranchRequestResponse(string.Empty, rawError));
                                    }
                                }
                            } else {
                                if (callback != null)
                                    callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                            }
                        } else {
                            if (callback != null)
                                callback.Invoke(new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace));
                        }
                    }
                }
            }
        }

        public static string GetUriWithParameters(string baseUri, Dictionary<string, string> parameters) {
            StringBuilder uriString = new StringBuilder();
            uriString.Append(baseUri);
            uriString.Append("?");
            foreach (string key in parameters.Keys) {
                uriString.Append(string.Format("{0}={1}&", key, parameters[key]));
            }
            uriString.Remove(uriString.Length - 1, 1);
            return uriString.ToString();
        }
    }
}
