namespace BranchSdk {
    public interface IBranchSystemObserver {
        string GetLocalIp();
        string GetOS();
        string GetOSVersion();
        string GetRawOSVersion();
    }
}