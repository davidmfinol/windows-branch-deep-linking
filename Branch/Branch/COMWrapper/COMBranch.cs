using BranchSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace COMWrapper
{
    [ComVisible(true)]
    public class COMBranch : ICOMBranch
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMInitSessionCallback(ICOMBranchUniversalObject buo, ICOMBranchLinkProperties link, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMGetRewardHistoryCallback(string json, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMGetRewardsCallback(bool changed, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMLogoutCallback(bool logoutSuccess, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMRedeemRewardsCallback(bool changed, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMIdentityUserCallback(string referrinParams, string error);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMUserCompletedActionCallback();

        public void CancelShareLinkDialog()
        {
            Branch.I.CancelShareLinkDialog();
        }

        public void DisableSimulateInstalls()
        {
            Branch.I.DisableSimulateInstalls();
        }

        public void DisableTestMode()
        {
            Branch.I.DisableTestMode();
        }

        public void DisableTracking(bool disableTracking)
        {
            Branch.I.DisableTracking(disableTracking);
        }

        public void EnableSimulateInstalls()
        {
            Branch.I.EnableSimulateInstalls();
        }

        public void EnableTestMode()
        {
            Branch.I.EnableTestMode();
        }

        public unsafe void GetCreditHistory(void* callback)
        {
            Branch.I.GetCreditHistory((json, error) => {
                string comJson = "";
                string comError = "";

                if (json != null) comJson = json.ToString();
                if (error != null) comError = error.GetMessage();

                COMGetRewardHistoryCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMGetRewardHistoryCallback>((IntPtr)callback);
                comCallback.Invoke(comJson, comError);
            });
        }

        public unsafe void GetCreditHistory(string bucket, void* callback)
        {
            Branch.I.GetCreditHistory(bucket, (json, error) => {
                string comJson = "";
                string comError = "";

                if (json != null) comJson = json.ToString();
                if (error != null) comError = error.GetMessage();

                COMGetRewardHistoryCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMGetRewardHistoryCallback>((IntPtr)callback);
                comCallback.Invoke(comJson, comError);
            });
        }

        public unsafe void GetCreditHistory(string afterID, int length, string order, void* callback)
        {
            Branch.CreditHistoryOrder orderEnum;
            if (!Enum.TryParse(order, out orderEnum)) {
                orderEnum = Branch.CreditHistoryOrder.KMostRecentFirst;
            }

            Branch.I.GetCreditHistory(afterID, length, orderEnum, (json, error) => {
                string comJson = "";
                string comError = "";

                if (json != null) comJson = json.ToString();
                if (error != null) comError = error.GetMessage();

                COMGetRewardHistoryCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMGetRewardHistoryCallback>((IntPtr)callback);
                comCallback.Invoke(comJson, comError);
            });
        }

        public unsafe void GetCreditHistory(string bucket, string afterId, int length, string order, void* callback)
        {
            Branch.CreditHistoryOrder orderEnum;
            if (!Enum.TryParse(order, out orderEnum)) {
                orderEnum = Branch.CreditHistoryOrder.KMostRecentFirst;
            }

            Branch.I.GetCreditHistory(bucket, afterId, length, orderEnum, (json, error) => {
                string comJson = "";
                string comError = "";

                if (json != null) comJson = json.ToString();
                if (error != null) comError = error.GetMessage();

                COMGetRewardHistoryCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMGetRewardHistoryCallback>((IntPtr)callback);
                comCallback.Invoke(comJson, comError);
            });
        }

        public int GetCredits()
        {
            return Branch.I.GetCredits();
        }

        public string GetFirstParams()
        {
            return Branch.I.GetFirstParams().ToString();
        }

        public void GetBranchInstance(bool isLive, string key)
        {
            Branch.GetBranchInstance(isLive, key);
        }

        public string GetSessionParams()
        {
            return Branch.I.GetSessionParams().ToString();
        }

        public void InitSession(string linkUrl = "", bool autoInitSession = false)
        {
            Branch.I.InitSession(linkUrl, autoInitSession);
        }

        public void InitSession(bool isReferrable, string linkUrl = "")
        {
            Branch.I.InitSession(isReferrable, linkUrl);
        }

        public unsafe void InitSession(void* callback, string linkUrl = "")
        {
            BranchInitCallbackWrapper initCallbackWrapper = new BranchInitCallbackWrapper((buo, link, error) => {
                ICOMBranchUniversalObject comBUO = null;
                ICOMBranchLinkProperties comLink = null;
                string comError = "";

                if (buo != null) comBUO = buo.ParseNativeBUO();
                if (link != null) comLink = link.ParseNativeLinkProperties();
                if (error != null) comError = error.GetMessage();

                COMInitSessionCallback comCallback 
                    = Marshal.GetDelegateForFunctionPointer<COMInitSessionCallback>((IntPtr)callback);
                comCallback.Invoke(comBUO, comLink, comError);
            });
            Branch.I.InitSession(initCallbackWrapper, linkUrl);
        }

        public unsafe void InitSession(bool isReferrable, void* callback, string linkUrl = "")
        {
            BranchInitCallbackWrapper initCallbackWrapper = new BranchInitCallbackWrapper((buo, link, error) => {
                ICOMBranchUniversalObject comBUO = null;
                ICOMBranchLinkProperties comLink = null;
                string comError = "";

                if (buo != null) comBUO = buo.ParseNativeBUO();
                if (link != null) comLink = link.ParseNativeLinkProperties();
                if (error != null) comError = error.GetMessage();

                COMInitSessionCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMInitSessionCallback>((IntPtr)callback);
                comCallback.Invoke(comBUO, comLink, comError);
            });
            Branch.I.InitSession(isReferrable, initCallbackWrapper, linkUrl);
        }

        public bool IsTrackingDisabled()
        {
            return Branch.I.IsTrackingDisabled();
        }

        public unsafe void LoadRewards(void* callback)
        {
            Branch.I.LoadRewards((changed, error) => {
                string comError = "";
                if (error != null) comError = error.GetMessage();

                COMGetRewardsCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMGetRewardsCallback>((IntPtr)callback);
                comCallback.Invoke(changed, comError);
            });
        }

        public unsafe void Logout(void* callback)
        {
            Branch.I.Logout((logoutSuccess, error) => {
                string comError = "";
                if (error != null) comError = error.GetMessage();

                COMLogoutCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMLogoutCallback>((IntPtr)callback);
                comCallback.Invoke(logoutSuccess, comError);
            });
        }

        public void RedeemRewards(int count)
        {
            Branch.I.RedeemRewards(count);
        }

        public unsafe void RedeemRewards(int count, void* callback)
        {
            Branch.I.RedeemRewards(count, (changed, error) => {
                string comError = "";
                if (error != null) comError = error.GetMessage();

                COMRedeemRewardsCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMRedeemRewardsCallback>((IntPtr)callback);
                comCallback.Invoke(changed, comError);
            });
        }

        public unsafe void RedeemRewards(string bucket, int count)
        {
            Branch.I.RedeemRewards(bucket, count);
        }

        public unsafe void RedeemRewards(string bucket, int count, void* callback)
        {
            Branch.I.RedeemRewards(bucket, count, (changed, error) => {
                string comError = "";
                if (error != null) comError = error.GetMessage();

                COMRedeemRewardsCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMRedeemRewardsCallback>((IntPtr)callback);
                comCallback.Invoke(changed, comError);
            });
        }

        public void SetDebug(bool isDebug)
        {
            Branch.I.SetDebug(isDebug);
        }

        public unsafe void SetIdentity(string userID, void* callback)
        {
            Branch.I.SetIdentity(userID, (refParams, error) => {
                string comError = "";
                string comRefParams = "";

                if (error != null) comError = error.GetMessage();
                if (refParams != null) comRefParams = refParams.ToString();

                COMIdentityUserCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMIdentityUserCallback>((IntPtr)callback);
                comCallback.Invoke(comRefParams, comError);
            });
        }

        public void SetMaxRetries(int maxRetries)
        {
            Branch.I.SetMaxRetries(maxRetries);
        }

        public void SetNetworkTimeout(int timeout)
        {
            Branch.I.SetNetworkTimeout(timeout);
        }

        public void SetRequestMetadata(string key, string value)
        {
            Branch.I.SetRequestMetadata(key, value);
        }

        public void SetRetryInterval(int retryInterval)
        {
            Branch.I.SetRetryInterval(retryInterval);
        }

        public void UserCompletedAction(string action, string metadata)
        {
            Branch.I.UserCompletedAction(action, JsonObject.Parse(metadata));
        }

        public void UserCompletedAction(string action)
        {
            Branch.I.UserCompletedAction(action);
        }

        public unsafe void UserCompletedAction(string action, void* callback)
        {
            Branch.I.UserCompletedAction(action, () => {
                COMUserCompletedActionCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMUserCompletedActionCallback>((IntPtr)callback);
                comCallback.Invoke();
            });
        }

        public unsafe void UserCompletedAction(string action, string metadata, void* callback)
        {
            Branch.I.UserCompletedAction(action, JsonObject.Parse(metadata), () => {
                COMUserCompletedActionCallback comCallback
                    = Marshal.GetDelegateForFunctionPointer<COMUserCompletedActionCallback>((IntPtr)callback);
                comCallback.Invoke();
            });
        }
    }
}
