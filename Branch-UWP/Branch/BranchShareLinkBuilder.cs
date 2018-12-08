using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Core;

namespace BranchSdk {
    public class BranchShareLinkBuilder {
        private string messageTitle;
        private string messageBody;
        private string defaultUrl;
        private StorageFile bitmap;
        private BranchShortLinkBuilder shortLinkBuilder;
        private CoreDispatcher dispatcher;
        private DataTransferManager dataTransferManager;

        public BranchShareLinkBuilder(DataTransferManager dataTransferManager, CoreDispatcher dispatcher, JsonObject parameters) {
            this.shortLinkBuilder = new BranchShortLinkBuilder();
            this.dispatcher = dispatcher;
            this.dataTransferManager = dataTransferManager;

            foreach (string key in parameters.Keys) {
                shortLinkBuilder.AddParameters(key, parameters[key]);
            }

            messageTitle = string.Empty;
            messageBody = string.Empty;
        }

        public BranchShareLinkBuilder(DataTransferManager dataTransferManager, CoreDispatcher dispatcher, BranchShortLinkBuilder shortLinkBuilder) {
            this.shortLinkBuilder = shortLinkBuilder;
            this.dispatcher = dispatcher;
            this.dataTransferManager = dataTransferManager;

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

        public BranchShareLinkBuilder SetBitmap(StorageFile bitmap) {
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
        public CoreDispatcher GetDispatcher() { return dispatcher; }
        public DataTransferManager GetDataTransferManager() { return dataTransferManager; }
        public StorageFile GetBitmap() { return bitmap; }
        public BranchShortLinkBuilder GetBranchShortLinkBuilder() { return shortLinkBuilder; }

        public void ShareLink() {
            Branch.I.ShareLink(dataTransferManager, dispatcher, this);
        }
    }
}
