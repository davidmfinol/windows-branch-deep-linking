using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BranchSdk;
using System.Threading.Tasks;
using BranchSdk.CrossPlatform;
using Windows.UI.Text;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestbedWindows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Main();
        }

        public void Main() {
            Task.Run(async () => {
                await LibraryAdapter.GetPrefHelper().LoadAll();
                Debug.WriteLine("Setup test branch key");
                LibraryAdapter.GetPrefHelper().SetBranchKey("key_test_gcy1q6txmcqHyqPqacgBZpbiush0RSDs");

                Branch.I.InitSession(new BranchInitCallbackWrapper(async (parameters, error) => {
                    List<string> lines = new List<string>();
                    lines.Add("Init session, parameters: ");
                    foreach (string key in parameters.Keys) {
                        lines.Add(key + " - " + parameters[key]);
                    }
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                        AddLog(lines);
                    });
                }));
            });
        }

        private void OnCalleventClicked(object sender, RoutedEventArgs e) {
            BranchEvent ev = new BranchEvent("test_custom_events")
                    .SetDescription("Test description")
                    .SetTransactionID("322")
                    .AddCustomDataProperty("TestProperty", "TestValue");
            ev.LogEvent();
        }

        private void OnIdentityClicked(object sender, RoutedEventArgs e) {
            Branch.I.SetIdentity("User1488", async (referringParams, error) => {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    List<string> lines = new List<string>();
                    lines.Add("Set identity, response: " + referringParams.ToString());
                    lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                    AddLog(lines);
                });
            });
        }

        private void OnGetShortLinkClicked(object sender, RoutedEventArgs e) {
            BranchUniversalObject branchUniversalObject = new BranchUniversalObject()
                   .SetCanonicalIdentifier("item/12345")
                   .SetCanonicalUrl("https://branch.io/deepviews")
                   .SetContentIndexingMode(BranchUniversalObject.ContentIndexModes.PRIVATE)
                   .SetLocalIndexMode(BranchUniversalObject.ContentIndexModes.PUBLIC)
                   .SetTitle("My Content Title")
                   .SetContentDescription("my_product_description1")
                   .SetContentImageUrl("https://example.com/mycontent-12345.png")
                   .SetContentExpiration(DateTime.UtcNow)
                   .SetContentImageUrl("https://test_img_url")
                   .AddKeyWord("My_Keyword1")
                   .AddKeyWord("My_Keyword2")
                   .SetContentMetadata(
                        new BranchContentMetadata().AddCustomMetadata("testkey", "testvalue")
                   );

            BranchLinkProperties linkProperties = new BranchLinkProperties()
                     .AddTag("Tag1")
                     .SetChannel("Sharing_Channel_name")
                     .SetFeature("my_feature_name")
                     .AddControlParameter("$android_deeplink_path", "custom/path/*")
                     .AddControlParameter("$ios_url", "http://example.com/ios")
                     .SetDuration(100);

            Task.Run(async () => {
                string url = branchUniversalObject.GetShortURL(linkProperties);
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    List<string> lines = new List<string>();
                    lines.Add("Short url: " + url);
                    AddLog(lines);
                });
            });
        }

        private void OnLogoutClicked(object sender, RoutedEventArgs e) {
            Branch.I.Logout((logout, error) => {
                Task.Run(async () => {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                        List<string> lines = new List<string>();
                        lines.Add("Logout status: " + logout);
                        lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                        AddLog(lines);
                    });
                });
            });
        }

        private void OnGetCreditsClicked(object sender, RoutedEventArgs e) {
            Branch.I.LoadRewards(async (changed, error) => {
                int credits = LibraryAdapter.GetPrefHelper().GetCreditCount();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    List<string> lines = new List<string>();
                    lines.Add("Credits count: " + credits);
                    lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                    AddLog(lines);
                });
            });
        }

        private void OnRedeemFiveClicked(object sender, RoutedEventArgs e) {
            Branch.I.RedeemRewards(5, async (changed, error) => {
                int credits = LibraryAdapter.GetPrefHelper().GetCreditCount();
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    List<string> lines = new List<string>();
                    lines.Add("Credits count: " + credits);
                    lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                    AddLog(lines);
                });
            });
        }

        private void OnBuyWithMetadataClicked(object sender, RoutedEventArgs e) {
            JObject parameters = new JObject();
            parameters.Add("name", "Alex");
            parameters.Add("boolean", true);
            parameters.Add("int", 1);
            parameters.Add("double", 0.13415512301);

            Branch.I.UserCompletedAction("buy", parameters);
        }

        private void OnGetCreditHistoryClicked(object sender, RoutedEventArgs e) {
            Branch.I.GetCreditHistory(async (response, error) => {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                    List<string> lines = new List<string>();
                    if(response != null) {
                        foreach(JObject prop in response) {
                            JObject transaction = prop["transaction"].Value<JObject>();
                            StringBuilder sb = new StringBuilder();
                            sb.Append(transaction["date"].Value<string>() + " - ");
                            sb.Append(transaction["bucket"].Value<string>() + ", amount: ");
                            sb.Append(transaction["amount"].Value<int>());
                            lines.Add(sb.ToString());
                        }
                    }
                    lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                    AddLog(lines);
                });
            });
        }

        private void AddLog(string text) {
            TextBox logText = new TextBox();
            logText.FontSize = 20;
            logText.FontWeight = FontWeights.Normal;
            logText.TextWrapping = TextWrapping.Wrap;
            logText.IsReadOnly = true;
            logText.AcceptsReturn = true;
            logText.BorderThickness = new Thickness(0);
            logText.Margin = new Thickness(0, 3, 0, 3);
            logText.Text = text;

            (this.FindName("LogStack") as StackPanel).Children.Add(logText);
        }

        private void AddLog(List<string> lines) {
            TextBox logText = new TextBox();
            logText.FontSize = 20;
            logText.FontWeight = FontWeights.Normal;
            logText.TextWrapping = TextWrapping.Wrap;
            logText.IsReadOnly = true;
            logText.AcceptsReturn = true;
            logText.BorderThickness = new Thickness(0);
            logText.Margin = new Thickness(0, 3, 0, 3);

            int i = 0;
            foreach (string line in lines) {
                logText.Text += line + (i < lines.Count - 1 ? Environment.NewLine : string.Empty);
                i++;
            }

            (this.FindName("LogStack") as StackPanel).Children.Add(logText);
        }
    }
}
