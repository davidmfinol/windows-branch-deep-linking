using System.Threading.Tasks;
using Windows.Data.Json;

namespace BranchSdk
{
    public interface IBranchPrefHelper
    {
        bool IsLoaded { get; }
        void ClearBranchAnalyticsData();
        void ClearIsReferrable();
        void ClearUserValues();
        string GetAPIBaseUrl();
        string GetBranchAnalyticsData();
        string GetBranchKey();
        int GetCreditCount();
        int GetCreditCount(string bucket);
        string GetDeviceFingerPrintId();
        string GetIdentity();
        string GetDeveloperIdentity();
        string GetIdentityId();
        string GetInstallParams();
        int GetIsReferrable();
        string GetLinkClickId();
        int GetMaxRetries();
        int GetNetworkTimeout();
        string GetRequestMetadata();
        int GetRetryInterval();
        string GetSessionId();
        string GetSessionParams();
        bool GetTrackingDisable();
        string GetUserUrl();
        Task LoadAll();
        Task SaveAll();
        void SaveBranchAnalyticsData(string analyticsDataRaw);
        void SetActionTotalCount(string action, int count);
        void SetActionUniqueCount(string action, int count);
        void SetBranchKey(string branchKey);
        void SetCreditCount(string bucket, int count);
        void SetDeviceFingerPrintId(string deviceFingerPrintId);
        void SetIdentity(string identity);
        void SetDeveloperIdentity(string developerIdentity);
        void SetIdentityId(string identityId);
        void SetInstallParams(string installParams);
        void SetIsReferrable();
        void SetLinkClickId(string linkClickId);
        void SetMaxRetries(int maxRetries);
        void SetNetworkTimeout(int networkTimeout);
        void SetRequestMetadata(string key, string value);
        void SetRetryInterval(int retryInterval);
        void SetSessionId(string sessionId);
        void SetSessionParams(string sessionParams);
        void SetTrackingDisable(bool value);
        void SetUserUrl(string userUrl);
    }
}