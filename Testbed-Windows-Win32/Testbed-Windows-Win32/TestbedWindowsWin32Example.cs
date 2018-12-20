using BranchSdk;
using BranchSdk.CrossPlatform;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Testbed_Windows_Win32
{
    public class TestbedWindowsWin32Example
    {
        public static void Main()
        {
            Console.WriteLine("Init app...");

            BranchConfigManager.LoadAll();
            LibraryAdapter.GetPrefHelper().LoadAll();
            Branch.GetTestInstance();

            Thread.Sleep(1000);

            Branch.I.InitSession(new BranchInitCallbackWrapper((buo, link, error) => {
                Console.WriteLine("\n\rSession was inited");
                Console.WriteLine("Title: {0}", buo.Title);
                Console.WriteLine("Link: {0}\n\r", buo.CanonicalUrl);
            }));

            Console.WriteLine("Init session...\n\r");
            Thread.Sleep(5000);

            ConsoleKeyInfo keyInfo = default(ConsoleKeyInfo);
            while (keyInfo == default(ConsoleKeyInfo) || keyInfo.Key != ConsoleKey.Q) {
                Console.Clear();
                Console.WriteLine("Press 1 to identity");
                Console.WriteLine("Press 2 to buy 5 credits");
                Console.WriteLine("Press 3 to load rewards");
                Console.WriteLine("Press 4 to generate short link");
                Console.WriteLine("Press 5 to logout");
                Console.WriteLine("Press 6 to redeem 5 credits");
                Console.WriteLine("Press 7 to call test event");
                Console.WriteLine("Press 8 to get credit history\n\r");
                keyInfo = Console.ReadKey();
                Console.WriteLine("\n\r");

                switch (keyInfo.Key) {
                    case ConsoleKey.D1:
                        Identity();
                        break;
                    case ConsoleKey.D2:
                        Buy5Credits();
                        break;
                    case ConsoleKey.D3:
                        LoadRewards();
                        break;
                    case ConsoleKey.D4:
                        GenerateShortLink();
                        break;
                    case ConsoleKey.D5:
                        Logout();
                        break;
                    case ConsoleKey.D6:
                        Redeem5Credits();
                        break;
                    case ConsoleKey.D7:
                        CallTestEvent();
                        break;
                    case ConsoleKey.D8:
                        GetCreditHistory();
                        break;
                }

                Thread.Sleep(2000);

                Console.WriteLine("\n\rPress Q to quit");
                Console.WriteLine("Press any other key to continue");
                keyInfo = Console.ReadKey();
            }

            LibraryAdapter.GetPrefHelper().SaveAll();
        }

        private static void Identity()
        {
            Branch.I.SetIdentity("TestUser", (parameters, error) => {
                Console.WriteLine(error == null ? "\n\rIdentity success" : "\n\rIdentity error");
            });
        }

        private static void Buy5Credits()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("name", "Alex");
            parameters.Add("boolean", true);
            parameters.Add("int", 1);
            parameters.Add("double", 0.13415512301);

            Branch.I.UserCompletedAction("buy", parameters);
        }

        private static void LoadRewards()
        {
            Branch.I.LoadRewards((change, error) => {
                Console.WriteLine("\n\rCount credits: {0}", Branch.I.GetCredits());
            });
        }

        private static void GenerateShortLink()
        {
            BranchUniversalObject branchUniversalObject = new BranchUniversalObject()
                   .SetCanonicalIdentifier("item/12345")
                   .SetCanonicalUrl("https://branch.io/deepviews")
                   .SetContentIndexingMode(BranchUniversalObject.ContentIndexModes.PRIVATE)
                   .SetLocalIndexMode(BranchUniversalObject.ContentIndexModes.PUBLIC)
                   .SetTitle("My Content Title")
                   .SetContentDescription("my_product_description1")
                   .SetContentImageUrl("https://example.com/mycontent-12345.png")
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

            branchUniversalObject.GetShortURL(linkProperties, (url) => {
                Console.WriteLine("\n\rShort link: {0}", url);
            });
        }

        private static void Logout()
        {
            Branch.I.Logout((logout, error) => {
                Console.WriteLine(error == null ? "\n\rLogout success" : "\n\rLogout error");
            });
        }

        private static void Redeem5Credits()
        {
            Branch.I.RedeemRewards(5, (changed, error) => {
                Console.WriteLine("\n\rCount credits: {0}", Branch.I.GetCredits());
            });
        }

        private static void CallTestEvent()
        {
            BranchEvent ev = new BranchEvent("test_custom_events")
                .SetDescription("Test description")
                .SetTransactionID("322")
                .AddCustomDataProperty("TestProperty1", "TestValue1")
                .AddCustomDataProperty("TestProperty2", "TestValue2");
            ev.LogEvent();
        }

        private static void GetCreditHistory()
        {
            Branch.I.GetCreditHistory((response, error) => {
                if (response != null) {
                    Console.WriteLine("\n\r");
                    foreach (Dictionary<string, object> prop in response) {
                        Dictionary<string, object> transaction = (Dictionary<string, object>)prop["transaction"];
                        StringBuilder sb = new StringBuilder();
                        sb.Append((string)transaction["date"] + " - ");
                        sb.Append((string)transaction["bucket"] + ", amount: ");
                        sb.Append(Convert.ToInt32(transaction["amount"]));
                        Console.WriteLine(sb.ToString());
                    }
                }
            });
        }
    }
}
