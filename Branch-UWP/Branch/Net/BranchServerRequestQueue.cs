using BranchSdk.CrossPlatform;
using BranchSdk.Net.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

            Task.Run(() => { TaskWorking(); });
            return true;
        }

        public static void ClearPendingRequests() {
            queue.Clear();
        }

        private static void TaskWorking() {
            while (runningQueue.Count > 0) {
                BranchServerRequest request = runningQueue.Dequeue();
                HandleRequest(request);
            }
        }

        private static async Task<BranchRequestResponse> HandleFailRequestGet(BranchServerRequest request, Uri requestUri, HttpClient client, HttpResponseMessage httpResponse) {
            string rawError = await httpResponse.Content.ReadAsStringAsync();

            if (LibraryAdapter.GetPrefHelper().GetMaxRetries() < 1) {
                request.OnFailed(rawError, (int)httpResponse.StatusCode);
                Debug.WriteLine("Request Error: " + rawError);
                return new BranchRequestResponse(string.Empty, rawError);
            }

            for (int i = 0; i < LibraryAdapter.GetPrefHelper().GetMaxRetries(); i++) {
                httpResponse = await client.GetAsync(requestUri);

                if ((int)httpResponse.StatusCode >= 500) {
                    Debug.WriteLine("Failed request, retry: " + i);

                    await Task.Delay(LibraryAdapter.GetPrefHelper().GetRetryInterval());
                    continue;
                } else {
                    if (httpResponse.IsSuccessStatusCode) {
                        Debug.WriteLine("Success request, retry: " + i);

                        string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                        request.OnSuccess(responseAsText);

                        return new BranchRequestResponse(responseAsText, string.Empty);
                    } else {
                        rawError = await httpResponse.Content.ReadAsStringAsync();
                        request.OnFailed(rawError, (int)httpResponse.StatusCode);
                        Debug.WriteLine("Request Error: " + rawError);

                        return new BranchRequestResponse(string.Empty, rawError);
                    }
                }
            }

            return new BranchRequestResponse(string.Empty, rawError);
        }

        private static async Task<BranchRequestResponse> HandleFailRequestPost(BranchServerRequest request, Uri requestUri, HttpClient client, HttpResponseMessage httpResponse, HttpContent content) {
            string rawError = await httpResponse.Content.ReadAsStringAsync();

            if (LibraryAdapter.GetPrefHelper().GetMaxRetries() < 1) {
                request.OnFailed(rawError, (int)httpResponse.StatusCode);
                Debug.WriteLine("Request Error: " + rawError);
                return new BranchRequestResponse(string.Empty, rawError);
            }

            for (int i = 0; i < LibraryAdapter.GetPrefHelper().GetMaxRetries(); i++) {
                httpResponse = await client.PostAsync(requestUri, content);

                if ((int)httpResponse.StatusCode >= 500) {
                    Debug.WriteLine("Failed request, retry: " + i);

                    await Task.Delay(LibraryAdapter.GetPrefHelper().GetRetryInterval());
                    continue;
                } else {
                    if (httpResponse.IsSuccessStatusCode) {
                        Debug.WriteLine("Success request, retry: " + i);

                        string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                        request.OnSuccess(responseAsText);

                        return new BranchRequestResponse(responseAsText, string.Empty);
                    } else {
                        rawError = await httpResponse.Content.ReadAsStringAsync();
                        request.OnFailed(rawError, (int)httpResponse.StatusCode);
                        Debug.WriteLine("Request Error: " + rawError);

                        return new BranchRequestResponse(string.Empty, rawError);
                    }
                }
            }

            return new BranchRequestResponse(string.Empty, rawError);
        }

        public static async Task<BranchRequestResponse> HandleRequest(BranchServerRequest request) {
            //if tracking disabled and request doesnt support execute without tracking properties
            if (BranchTrackingController.TrackingDisabled && !request.PrepareExecuteWithoutTracking()) {
                BranchError error = new BranchError(string.Empty, BranchError.ERR_BRANCH_TRACKING_DISABLED);
                request.OnFailed(string.Empty, error.GetErrorCode());
                return new BranchRequestResponse(string.Empty, error.GetMessage());
            }

            try {
                HttpClient httpClient = new HttpClient();
                var headers = httpClient.DefaultRequestHeaders;
                httpClient.Timeout = TimeSpan.FromSeconds(LibraryAdapter.GetPrefHelper().GetNetworkTimeout());

                if (request.RequestType == RequestTypes.GET) {
                    Uri requestUri = new Uri(GetUriWithParameters(request.RequestUrl(), request.Parameters));
                    HttpResponseMessage httpResponse = new HttpResponseMessage();

                    httpResponse = await httpClient.GetAsync(requestUri);

                    Debug.WriteLine("Get request: " + GetUriWithParameters(request.RequestUrl(), request.Parameters));

                    if ((int)httpResponse.StatusCode >= 500) {
                        Debug.WriteLine("Failed request, retrying...");

                        return await HandleFailRequestGet(request, requestUri, httpClient, httpResponse);
                    } else {
                        if (httpResponse.IsSuccessStatusCode) {
                            string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                            request.OnSuccess(responseAsText);
                            return new BranchRequestResponse(responseAsText, string.Empty);
                        } else {
                            string rawError = await httpResponse.Content.ReadAsStringAsync();
                            request.OnFailed(rawError, (int)httpResponse.StatusCode);
                            Debug.WriteLine("Request Error: " + rawError);
                            return new BranchRequestResponse(string.Empty, rawError);
                        }
                    }
                } else {
                    Uri requestUri = new Uri(request.RequestUrl());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

                    HttpContent content = new StringContent(request.PostData.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponse = await httpClient.PostAsync(requestUri, content);

                    Debug.WriteLine("Post request: " + request.RequestUrl());
                    Debug.WriteLine("Post data: " + request.PostData);

                    if ((int)httpResponse.StatusCode >= 500) {
                        Debug.WriteLine("Failed request, retrying...");

                        return await HandleFailRequestPost(request, requestUri, httpClient, httpResponse, content);
                    } else {
                        if (httpResponse.IsSuccessStatusCode) {
                            string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                            request.OnSuccess(responseAsText);
                            return new BranchRequestResponse(responseAsText, string.Empty);
                        } else {
                            string rawError = await httpResponse.Content.ReadAsStringAsync();
                            request.OnFailed(rawError, (int)httpResponse.StatusCode);
                            Debug.WriteLine("Request Error: " + rawError);
                            return new BranchRequestResponse(string.Empty, rawError);
                        }
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("sdk request error: " + e.Message + " - " + e.StackTrace);
                return new BranchRequestResponse(string.Empty, "sdk request error: " + e.Message + " - " + e.StackTrace);
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
