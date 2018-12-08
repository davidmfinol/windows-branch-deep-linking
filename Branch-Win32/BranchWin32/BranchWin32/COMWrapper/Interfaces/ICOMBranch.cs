using System.Runtime.InteropServices;

namespace COMWrapper
{
    [ComVisible(true)]
    public interface ICOMBranch
    {
        #region Base branch methods
        void GetBranchInstance(bool isLive, string key);
        void EnableTestMode();
        void DisableTestMode();
        void SetDebug(bool isDebug);
        void DisableTracking(bool disableTracking);
        bool IsTrackingDisabled();
        void SetNetworkTimeout(int timeout);
        void SetMaxRetries(int maxRetries);
        void SetRetryInterval(int retryInterval);
        void SetRequestMetadata(string key, string value);
        void EnableSimulateInstalls();
        void DisableSimulateInstalls();
        void InitSession(string linkUrl = "", bool autoInitSession = false);
        void InitSession(bool isReferrable, string linkUrl = "");
        unsafe void InitSession(void* callback, string linkUrl = "");
        unsafe void InitSession(bool isReferrable, void* callback, string linkUrl = "");
        string GetFirstParams();
        string GetSessionParams();
        unsafe void SetIdentity(string userID, void* callback);
        unsafe void Logout(void* callback);
        unsafe void LoadRewards(void* callback);
        void RedeemRewards(int count);
        unsafe void RedeemRewards(int count, void* callback);
        void RedeemRewards(string bucket, int count);
        unsafe void RedeemRewards(string bucket, int count, void* callback);
        void UserCompletedAction(string action, string metadata);
        void UserCompletedAction(string action);
        unsafe void UserCompletedAction(string action, void* callback);
        unsafe void UserCompletedAction(string action, string metadata, void* callback);
        void CancelShareLinkDialog();
        unsafe void GetCreditHistory(void* callback);
        unsafe void GetCreditHistory(string bucket, void* callback);
        unsafe void GetCreditHistory(string afterID, int length, string order, void* callback);
        unsafe void GetCreditHistory(string bucket, string afterId, int length, string order, void* callback);
        int GetCredits();
        #endregion
    }
}
