using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace BranchSdk {
    public class BranchShareLinkManager {
        private BranchShareLinkBuilder branchShareLinkBuilder;
        private DataTransferManager dataTransferManager;

        private string url;

        public void ShareLink(DataTransferManager dataTransferManager, CoreDispatcher dispatcher, BranchShareLinkBuilder branchShareLinkBuilder) {
            this.branchShareLinkBuilder = branchShareLinkBuilder;
            this.dataTransferManager = dataTransferManager;
            this.dataTransferManager.DataRequested += OnDataRequested;

            branchShareLinkBuilder.GetBranchShortLinkBuilder().GetUrlWithCallback(async (url) => {
                this.url = url;
                await dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                    DataTransferManager.ShowShareUI();
                });
            });
        }

        public void CancelShareLinkDialog() {
            this.dataTransferManager.DataRequested -= OnDataRequested;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args) {
            if (string.IsNullOrEmpty(url)) url = branchShareLinkBuilder.GetDefaultUrl();

            DataRequest request = args.Request;

            if (!string.IsNullOrEmpty(branchShareLinkBuilder.GetTitle()))
                request.Data.Properties.Title = branchShareLinkBuilder.GetTitle();
            if (!string.IsNullOrEmpty(branchShareLinkBuilder.GetMessage())) {
                request.Data.Properties.Description = branchShareLinkBuilder.GetMessage();
                request.Data.SetText(branchShareLinkBuilder.GetMessage());
            }
            if (branchShareLinkBuilder.GetBitmap() != null) {
                RandomAccessStreamReference bitmap = RandomAccessStreamReference.CreateFromFile(branchShareLinkBuilder.GetBitmap());
                request.Data.Properties.Thumbnail = bitmap;
                request.Data.SetBitmap(bitmap);
            }
            if (!string.IsNullOrEmpty(url))
                request.Data.SetWebLink(new Uri(url));
        }
    }
}
