namespace BranchSdk {
    public class BranchShareSheetStyle {
        private string messageTitle;
        private string messageBody;
        private string defaultURL;
        private byte[] bitmap;

        public BranchShareSheetStyle(string messageTitle, string messageBody) {
            this.messageTitle = messageTitle;
            this.messageBody = messageBody;
        }

        public BranchShareSheetStyle SetDefaultUrl(string defaultUrl) {
            this.defaultURL = defaultUrl;
            return this;
        }

        public BranchShareSheetStyle SetBitmap(byte[] bitmap) {
            this.bitmap = bitmap;
            return this;
        }

        public string GetMessageTitle() {
            return messageTitle;
        }

        public string GetMessageBody() {
            return messageBody;
        }

        public string GetDefaultUrl() {
            return defaultURL;
        }

        public byte[] GetBitmap() {
            return bitmap;
        }
    }
}
