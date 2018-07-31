namespace BranchSdk {
    public interface IBranchPrefHelper {
        string GetAPIBaseUrl();
        string GetBranchKey();
        string GetDeviceFingerPrintId();
        string GetIdentity();
        string GetIdentityId();
        string GetLinkClickId();
        string GetSessionId();
        string GetSessionParams();
        string GetUserUrl();
        string GetInstallParams();
        int GetIsReferrable();
        void SetBranchKey(string branchKey);
        void SetDeviceFingerPrintId(string deviceFingerPrintId);
        void SetIdentity(string identity);
        void SetIdentityId(string identityId);
        void SetLinkClickId(string linkClickId);
        void SetSessionId(string sessionId);
        void SetSessionParams(string sessionParams);
        void SetUserUrl(string userUrl);
        void SetInstallParams(string parameters);
        void ClearUserValues();
        void SetCreditCount(string bucket, int count);
        void SetActionTotalCount(string action, int count);
        void SetActionUniqueCount(string action, int count);
        void ClearIsReferrable();
        void SetIsReferrable();
    }
}