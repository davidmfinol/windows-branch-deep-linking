using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BranchSdk;
using System.Threading.Tasks;
using BranchSdk.CrossPlatform;
using System.Diagnostics;
using Windows.UI.Text;
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

        public void Main(bool setBranchKey = true) {
            Task.Run(async () => {
                Task t = Task.Run(() => {
                    if (setBranchKey)
                        LibraryAdapter.GetPrefHelper().SetBranchKey("key_test_gcy1q6txmcqHyqPqacgBZpbiush0RSDs"); //temp
                });
                await t;
                t.Wait(100);

                Branch.I.InitSession(new BranchInitCallbackWrapper((parameters, error) => {
                    List<string> lines = new List<string>();
                    lines.Add("Init session, parameters: ");
                    foreach (string key in parameters.Keys) {
                        lines.Add(key + " - " + parameters[key]);
                    }
                    Task.Run(async () => {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                            AddLog(lines);
                        });
                    });
                }));
            });
        }

        private void OnCalleventClicked(object sender, RoutedEventArgs e) {
            Task.Run(() => {
                BranchEvent ev = new BranchEvent("test_custom_events")
                    .SetDescription("Test description")
                    .SetTransactionID("322")
                    .AddCustomDataProperty("TestProperty", "TestValue");
                ev.LogEvent();
            });
        }

        private void OnIdentityClicked(object sender, RoutedEventArgs e) {
            Task.Run(() => {
                Branch.I.SetIdentity("User1488", (referringParams, error) => {
                    Task.Run(async () => {
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () => {
                            List<string> lines = new List<string>();
                            lines.Add("Set identity, response: " + referringParams.ToString());
                            lines.Add("Error: " + (error != null ? error.GetMessage() : "no errors"));
                            AddLog(lines);
                        });
                    });
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
            foreach(string line in lines) {
                logText.Text += line + (i < lines.Count - 1 ? Environment.NewLine : string.Empty);
                i++;
            }

            (this.FindName("LogStack") as StackPanel).Children.Add(logText);
        }

        private void OnGetShortLinkClicked(object sender, RoutedEventArgs e) {
            Task.Run(() => {
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
            });
        }

        private void OnLogoutClicked(object sender, RoutedEventArgs e) {
            Task.Run(() => {
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
            });
        }
    }
}
