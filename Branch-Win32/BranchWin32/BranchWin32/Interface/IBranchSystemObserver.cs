namespace BranchSdk {
    public interface IBranchSystemObserver {
        bool IsRealHardwareId { get; }
        string GetUniqueID(bool debug);
        string GetLocalIp();
        string GetOS();
        string GetOSVersion();
        string GetRawOSVersion();
    }
}