using System.Collections.Generic;

namespace BranchSdk {
    public class BranchShareLinkBuilder {
        private string messageTitle;
        private string messageBody;
        private string defaultUrl;
        private byte[] bitmap;
        private BranchShortLinkBuilder shortLinkBuilder;

        public BranchShareLinkBuilder(Dictionary<string, object> parameters) {
            this.shortLinkBuilder = new BranchShortLinkBuilder();

            foreach (string key in parameters.Keys) {
                shortLinkBuilder.AddParameters(key, parameters[key]);
            }

            messageTitle = string.Empty;
            messageBody = string.Empty;
        }

        public BranchShareLinkBuilder(BranchShortLinkBuilder shortLinkBuilder) {
            this.shortLinkBuilder = shortLinkBuilder;

            messageTitle = string.Empty;
            messageBody = string.Empty;
        }

        public BranchShareLinkBuilder SetTitle(string messageTitle) {
            this.messageTitle = messageTitle;
            return this;
        }

        public BranchShareLinkBuilder SetMessage(string messageBody) {
            this.messageBody = messageBody;
            return this;
        }

        public BranchShareLinkBuilder SetDefaultURL(string url) {
            defaultUrl = url;
            return this;
        }

        public BranchShareLinkBuilder SetBitmap(byte[] bitmap) {
            this.bitmap = bitmap;
            return this;
        }

        public BranchShareLinkBuilder AddTag(string tag) {
            this.shortLinkBuilder.AddTag(tag);
            return this;
        }

        public BranchShareLinkBuilder AddTags(List<string> tags) {
            this.shortLinkBuilder.AddTags(tags);
            return this;
        }

        public BranchShareLinkBuilder SetFeature(string feature) {
            this.shortLinkBuilder.SetFeature(feature);
            return this;
        }

        public BranchShareLinkBuilder SetStage(string stage) {
            this.shortLinkBuilder.SetStage(stage);
            return this;
        }

        public BranchShareLinkBuilder SetAlias(string alias) {
            this.shortLinkBuilder.SetAlias(alias);
            return this;
        }

        public BranchShareLinkBuilder SetMatchDuration(int matchDuration) {
            this.shortLinkBuilder.SetDuration(matchDuration);
            return this;
        }

        public string GetTitle() { return messageTitle; }
        public string GetMessage() { return messageBody; }
        public string GetDefaultUrl() { return defaultUrl; }
        public byte[] GetBitmap() { return bitmap; }
        public BranchShortLinkBuilder GetBranchShortLinkBuilder() { return shortLinkBuilder; }

        public void ShareLink() {
            Branch.I.ShareLink(this);
        }
    }
}
