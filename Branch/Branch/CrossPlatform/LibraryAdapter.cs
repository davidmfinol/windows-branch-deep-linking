namespace BranchSdk.CrossPlatform {
    public static class LibraryAdapter {
        private static IBranchSystemObserver systemObserver;
        private static IBranchPrefHelper prefHelper;

        public static IBranchSystemObserver GetSystemObserver() {
            if(systemObserver == null) {
                systemObserver = DefaultSystemObserver();
            }
            return systemObserver;
        }

        public static IBranchPrefHelper GetPrefHelper() {
            if(prefHelper==null) {
                prefHelper = DefaultPrefHelper();
            }
            return prefHelper;
        }

        public static void SetSystemObserver(IBranchSystemObserver systemObserver) {
            LibraryAdapter.systemObserver = systemObserver;
        }

        public static void SetPrefHelper(IBranchPrefHelper prefHelper) {
            LibraryAdapter.prefHelper = prefHelper;
        }

        private static IBranchSystemObserver DefaultSystemObserver() {
            return new BranchSystemObserver();
        }

        private static IBranchPrefHelper DefaultPrefHelper() {
            return new BranchPrefHelper();
        }
    }
}
