namespace BranchSdk.Enum {
    public static class BranchEnumUtils {
        public static BranchCurrencyType ParseToCurrencyType(string iso4217Code) {
            if (!System.Enum.IsDefined(typeof(BranchCurrencyType), iso4217Code))
                return BranchCurrencyType.NONE;

            return (BranchCurrencyType)System.Enum.Parse(typeof(BranchCurrencyType), iso4217Code);
        }

        public static BranchContentSchema ParseToContentSchema(string name) {
            if (!System.Enum.IsDefined(typeof(BranchCurrencyType), name))
                return BranchContentSchema.NONE;

            return (BranchContentSchema)System.Enum.Parse(typeof(BranchContentSchema), name);
        }

        public static BranchCondition ParseToCondition(string name) {
            if (!System.Enum.IsDefined(typeof(BranchCondition), name))
                return BranchCondition.NONE;

            return (BranchCondition)System.Enum.Parse(typeof(BranchCondition), name);
        }

        public static string GetKey(LinkParam linkParam) {
            switch(linkParam) {
                case LinkParam.Tags: return "tags";
                case LinkParam.Alias: return "alias";
                case LinkParam.Type: return "type";
                case LinkParam.Duration: return "duration";
                case LinkParam.Channel: return "channel";
                case LinkParam.Feature: return "feature";
                case LinkParam.Stage: return "stage";
                case LinkParam.Campaign: return "campaing";
                case LinkParam.Data: return "data";
                case LinkParam.URL: return "url";
                default: return "undefinded";
            }
        }

        public static string GetEventName(BranchStandartEvent branchEvent) {
            switch(branchEvent) {
                case BranchStandartEvent.ADD_TO_CART: return "ADD_TO_CART";
                case BranchStandartEvent.ADD_TO_WISHLIST: return "ADD_TO_WISHLIST";
                case BranchStandartEvent.VIEW_CART: return "VIEW_CART";
                case BranchStandartEvent.INITIATE_PURCHASE: return "INITIATE_PURCHASE";
                case BranchStandartEvent.ADD_PAYMENT_INFO: return "ADD_PAYMENT_INFO";
                case BranchStandartEvent.PURCHASE: return "PURCHASE";
                case BranchStandartEvent.SPEND_CREDITS: return "SPEND_CREDITS";
                case BranchStandartEvent.SEARCH: return "SEARCH";
                case BranchStandartEvent.VIEW_ITEM: return "VIEW_ITEM";
                case BranchStandartEvent.VIEW_ITEMS: return "VIEW_ITEMS";
                case BranchStandartEvent.RATE: return "RATE";
                case BranchStandartEvent.SHARE: return "SHARE";
                case BranchStandartEvent.COMPLETE_REGISTRATION: return "COMPLETE_REGISTRATION";
                case BranchStandartEvent.COMPLETE_TUTORIAL: return "COMPLETE_TUTORIAL";
                case BranchStandartEvent.ACHIEVE_LEVEL: return "ACHIEVE_LEVEL";
                case BranchStandartEvent.UNLOCK_ACHIEVEMENT: return "UNLOCK_ACHIEVEMENT";
                default: return "undefinded";
            }
        }

        public static string GetPath(RequestPath requestPath) {
            switch(requestPath) {
                case RequestPath.RedeemRewards: return "v1/redeem";
                case RequestPath.GetURL: return "v1/url";
                case RequestPath.RegisterInstall: return "v1/install";
                case RequestPath.RegisterClose: return "v1/close";
                case RequestPath.RegisterOpen: return "v1/open";
                case RequestPath.RegisterView: return "v1/register-view";
                case RequestPath.GetCredits: return "v1/credits/";
                case RequestPath.GetCreditHistory: return "v1/credithistory";
                case RequestPath.CompletedAction: return "v1/event";
                case RequestPath.IdentifyUser: return "v1/profile";
                case RequestPath.Logout: return "v1/logout";
                case RequestPath.GetReferralCode: return "v1/referralcode";
                case RequestPath.ValidateReferralCode: return "v1/referralcode/";
                case RequestPath.ApplyReferralCode: return "v1/applycode/";
                case RequestPath.DebugConnect: return "v1/debug/connect";
                case RequestPath.ContentEvent: return "v1/content-events";
                case RequestPath.TrackStandardEvent: return "v2/event/standard";
                case RequestPath.TrackCustomEvent: return "v2/event/custom";
                default: return "undefinded";
            }
        }

        public static string GetKey(BranchJsonKey jsonKey) {
            switch(jsonKey) {
                case BranchJsonKey.IdentityID: return "identity_id";
                case BranchJsonKey.Identity: return "identity";
                case BranchJsonKey.DeviceFingerprintID: return "device_fingerprint_id";
                case BranchJsonKey.SessionID: return "session_id";
                case BranchJsonKey.LinkClickID: return "link_click_id";
                case BranchJsonKey.GoogleSearchInstallReferrer: return "google_search_install_referrer";
                case BranchJsonKey.GooglePlayInstallReferrer: return "install_referrer_extras";
                case BranchJsonKey.ClickedReferrerTimeStamp: return "clicked_referrer_ts";
                case BranchJsonKey.InstallBeginTimeStamp: return "install_begin_ts";
                case BranchJsonKey.FaceBookAppLinkChecked: return "facebook_app_link_checked";
                case BranchJsonKey.BranchLinkUsed: return "branch_used";
                case BranchJsonKey.ReferringBranchIdentity: return "referring_branch_identity";
                case BranchJsonKey.BranchIdentity: return "branch_identity";
                case BranchJsonKey.BranchKey: return "branch_key";
                case BranchJsonKey.BranchData: return "branch_data";

                case BranchJsonKey.Bucket: return "bucket";
                case BranchJsonKey.DefaultBucket: return "default";
                case BranchJsonKey.Amount: return "amount";
                case BranchJsonKey.CalculationType: return "calculation_type";
                case BranchJsonKey.Location: return "location";
                case BranchJsonKey.Type: return "type";
                case BranchJsonKey.CreationSource: return "creation_source";
                case BranchJsonKey.Prefix: return "prefix";
                case BranchJsonKey.Expiration: return "expiration";
                case BranchJsonKey.Event: return "event";
                case BranchJsonKey.Metadata: return "metadata";
                case BranchJsonKey.CommerceData: return "commerce_data";
                case BranchJsonKey.ReferralCode: return "referral_code";
                case BranchJsonKey.Total: return "total";
                case BranchJsonKey.Unique: return "unique";
                case BranchJsonKey.Length: return "length";
                case BranchJsonKey.Direction: return "direction";
                case BranchJsonKey.BeginAfterID: return "begin_after_id";
                case BranchJsonKey.Link: return "link";
                case BranchJsonKey.ReferringData: return "referring_data";
                case BranchJsonKey.ReferringLink: return "referring_link";
                case BranchJsonKey.IsFullAppConv: return "is_full_app_conversion";
                case BranchJsonKey.Data: return "data";
                case BranchJsonKey.OS: return "os";
                case BranchJsonKey.HardwareID: return "hardware_id";
                case BranchJsonKey.AndroidID: return "android_id";
                case BranchJsonKey.UnidentifiedDevice: return "unidentified_device";
                case BranchJsonKey.HardwareIDType: return "hardware_id_type";
                case BranchJsonKey.HardwareIDTypeVendor: return "vendor_id";
                case BranchJsonKey.HardwareIDTypeRandom: return "random";
                case BranchJsonKey.IsHardwareIDReal: return "is_hardware_id_real";
                case BranchJsonKey.AppVersion: return "app_version";
                case BranchJsonKey.OSVersion: return "os_version";
                case BranchJsonKey.Country: return "country";
                case BranchJsonKey.Language: return "language";
                case BranchJsonKey.IsReferrable: return "is_referrable";
                case BranchJsonKey.Update: return "update";
                case BranchJsonKey.OriginalInstallTime: return "first_install_time";
                case BranchJsonKey.FirstInstallTime: return "latest_install_time";
                case BranchJsonKey.LastUpdateTime: return "latest_update_time";
                case BranchJsonKey.PreviousUpdateTime: return "previous_update_time";
                case BranchJsonKey.URIScheme: return "uri_scheme";
                case BranchJsonKey.AppIdentifier: return "app_identifier";
                case BranchJsonKey.LinkIdentifier: return "link_identifier";
                case BranchJsonKey.GoogleAdvertisingID: return "google_advertising_id";
                case BranchJsonKey.AAID: return "aaid";
                case BranchJsonKey.LATVal: return "lat_val";
                case BranchJsonKey.LimitedAdTracking: return "limit_ad_tracking";
                case BranchJsonKey.limitFacebookTracking: return "limit_facebook_tracking";
                case BranchJsonKey.Debug: return "debug";
                case BranchJsonKey.Brand: return "brand";
                case BranchJsonKey.Model: return "model";
                case BranchJsonKey.ScreenDpi: return "screen_dpi";
                case BranchJsonKey.ScreenHeight: return "screen_height";
                case BranchJsonKey.ScreenWidth: return "screen_width";
                case BranchJsonKey.WiFi: return "wifi";
                case BranchJsonKey.LocalIP: return "local_ip";
                case BranchJsonKey.UserData: return "user_data";
                case BranchJsonKey.DeveloperIdentity: return "developer_identity";
                case BranchJsonKey.UserAgent: return "user_agent";
                case BranchJsonKey.SDK: return "sdk";
                case BranchJsonKey.SdkVersion: return "sdk_version";
                case BranchJsonKey.UIMode: return "ui_mode";

                case BranchJsonKey.Clicked_Branch_Link: return "+clicked_branch_link";
                case BranchJsonKey.IsFirstSession: return "+is_first_session";
                case BranchJsonKey.AndroidDeepLinkPath: return "$android_deeplink_path";
                case BranchJsonKey.DeepLinkPath: return "$deeplink_path";

                case BranchJsonKey.AndroidAppLinkURL: return "android_app_link_url";
                case BranchJsonKey.AndroidPushNotificationKey: return "branch";
                case BranchJsonKey.AndroidPushIdentifier: return "push_identifier";
                case BranchJsonKey.ForceNewBranchSession: return "branch_force_new_session";

                case BranchJsonKey.CanonicalIdentifier: return "$canonical_identifier";
                case BranchJsonKey.ContentTitle: return "$og_title";
                case BranchJsonKey.ContentDesc: return "$og_description";
                case BranchJsonKey.ContentImgUrl: return "$og_image_url";
                case BranchJsonKey.CanonicalUrl: return "$canonical_url";

                case BranchJsonKey.ContentType: return "$content_type";
                case BranchJsonKey.PublicallyIndexable: return "$publicly_indexable";
                case BranchJsonKey.LocallyIndexable: return "$locally_indexable";
                case BranchJsonKey.ContentKeyWords: return "$keywords";
                case BranchJsonKey.ContentExpiryTime: return "$exp_date";
                case BranchJsonKey.Params: return "params";
                case BranchJsonKey.SharedLink: return "$shared_link";
                case BranchJsonKey.ShareError: return "$share_error";

                case BranchJsonKey.External_Intent_URI: return "external_intent_uri";
                case BranchJsonKey.External_Intent_Extra: return "external_intent_extra";
                case BranchJsonKey.Last_Round_Trip_Time: return "lrtt";
                case BranchJsonKey.Branch_Round_Trip_Time: return "brtt";
                case BranchJsonKey.Branch_Instrumentation: return "instrumentation";
                case BranchJsonKey.Queue_Wait_Time: return "qwt";
                case BranchJsonKey.InstantDeepLinkSession: return "instant_dl_session";

                case BranchJsonKey.BranchViewData: return "branch_view_data";
                case BranchJsonKey.BranchViewID: return "id";
                case BranchJsonKey.BranchViewAction: return "action";
                case BranchJsonKey.BranchViewNumOfUse: return "number_of_use";
                case BranchJsonKey.BranchViewUrl: return "url";
                case BranchJsonKey.BranchViewHtml: return "html";

                case BranchJsonKey.Path: return "path";
                case BranchJsonKey.ViewList: return "view_list";
                case BranchJsonKey.ContentActionView: return "view";
                case BranchJsonKey.ContentPath: return "content_path";
                case BranchJsonKey.ContentNavPath: return "content_nav_path";
                case BranchJsonKey.ReferralLink: return "referral_link";
                case BranchJsonKey.ContentData: return "content_data";
                case BranchJsonKey.ContentEvents: return "events";
                case BranchJsonKey.ContentAnalyticsMode: return "content_analytics_mode";
                case BranchJsonKey.ContentDiscovery: return "cd";
                case BranchJsonKey.Environment: return "environment";
                case BranchJsonKey.InstantApp: return "INSTANT_APP";
                case BranchJsonKey.NativeApp: return "FULL_APP";

                case BranchJsonKey.TransactionID: return "transaction_id";
                case BranchJsonKey.Currency: return "currency";
                case BranchJsonKey.Revenue: return "revenue";
                case BranchJsonKey.Shipping: return "shipping";
                case BranchJsonKey.Tax: return "tax";
                case BranchJsonKey.Coupon: return "coupon";
                case BranchJsonKey.Affiliation: return "affiliation";
                case BranchJsonKey.Description: return "description";
                case BranchJsonKey.SearchQuery: return "search_query";

                case BranchJsonKey.Name: return "name";
                case BranchJsonKey.CustomData: return "custom_data";
                case BranchJsonKey.EventData: return "event_data";
                case BranchJsonKey.ContentItems: return "content_items";
                case BranchJsonKey.ContentSchema: return "$content_schema";
                case BranchJsonKey.Price: return "$price";
                case BranchJsonKey.PriceCurrency: return "$currency";
                case BranchJsonKey.Quantity: return "$quantity";
                case BranchJsonKey.SKU: return "$sku";
                case BranchJsonKey.ProductName: return "$product_name";
                case BranchJsonKey.ProductBrand: return "$product_brand";
                case BranchJsonKey.ProductCategory: return "$product_category";
                case BranchJsonKey.ProductVariant: return "$product_variant";
                case BranchJsonKey.Rating: return "$rating";
                case BranchJsonKey.RatingAverage: return "$rating_average";
                case BranchJsonKey.RatingCount: return "$rating_count";
                case BranchJsonKey.RatingMax: return "$rating_max";
                case BranchJsonKey.AddressStreet: return "$address_street";
                case BranchJsonKey.AddressCity: return "$address_city";
                case BranchJsonKey.AddressRegion: return "$address_region";
                case BranchJsonKey.AddressCountry: return "$address_country";
                case BranchJsonKey.AddressPostalCode: return "$address_postal_code";
                case BranchJsonKey.Latitude: return "$latitude";
                case BranchJsonKey.Longitude: return "$longitude";
                case BranchJsonKey.ImageCaptions: return "$image_captions";
                case BranchJsonKey.Condition: return "$condition";
                case BranchJsonKey.CreationTimestamp: return "$creation_timestamp";
                case BranchJsonKey.TrackingDisable: return "tracking_disabled";
                case BranchJsonKey.WindowsAppWebLinkUrl: return "windows_app_web_link_url";

                default: return "undefined";
            }
        }

        public static BranchProductCategory ParseToProductCategory(string category) {
            if (category.Equals("Animals & Pet Supplies")) {
                return BranchProductCategory.ANIMALS_AND_PET_SUPPLIES;
            }
            if (category.Equals("Apparel & Accessories")) {
                return BranchProductCategory.APPAREL_AND_ACCESSORIES;
            }
            if (category.Equals("Arts & Entertainment")) {
                return BranchProductCategory.ARTS_AND_ENTERTAINMENT;
            }
            if (category.Equals("Baby & Toddler")) {
                return BranchProductCategory.BABY_AND_TODDLER;
            }
            if (category.Equals("Business & Industrial")) {
                return BranchProductCategory.BUSINESS_AND_INDUSTRIAL;
            }
            if (category.Equals("Cameras & Optics")) {
                return BranchProductCategory.CAMERAS_AND_OPTICS;
            }
            if (category.Equals("Electronics")) {
                return BranchProductCategory.ELECTRONICS;
            }
            if (category.Equals("Food, Beverages & Tobacco")) {
                return BranchProductCategory.FOOD_BEVERAGES_AND_TOBACCO;
            }
            if (category.Equals("Furniture")) {
                return BranchProductCategory.FURNITURE;
            }
            if (category.Equals("Hardware")) {
                return BranchProductCategory.HARDWARE;
            }
            if (category.Equals("Health & Beauty")) {
                return BranchProductCategory.HEALTH_AND_BEAUTY;
            }
            if (category.Equals("Home & Garden")) {
                return BranchProductCategory.HOME_AND_GARDEN;
            }
            if (category.Equals("Luggage & Bags")) {
                return BranchProductCategory.LUGGAGE_AND_BAGS;
            }
            if (category.Equals("Mature")) {
                return BranchProductCategory.MATURE;
            }
            if (category.Equals("Media")) {
                return BranchProductCategory.MEDIA;
            }
            if (category.Equals("Office Supplies")) {
                return BranchProductCategory.OFFICE_SUPPLIES;
            }
            if (category.Equals("Religious & Ceremonial")) {
                return BranchProductCategory.RELIGIOUS_AND_CEREMONIAL;
            }
            if (category.Equals("Software")) {
                return BranchProductCategory.SOFTWARE;
            }
            if (category.Equals("Sporting Goods")) {
                return BranchProductCategory.SPORTING_GOODS;
            }
            if (category.Equals("Toys & Games")) {
                return BranchProductCategory.TOYS_AND_GAMES;
            }
            if (category.Equals("Vehicles & Parts")) {
                return BranchProductCategory.VEHICLES_AND_PARTS;
            }
            return BranchProductCategory.NONE;
        }

        public static string ProductCategoryToString(BranchProductCategory category) {
            switch (category) {
                case BranchProductCategory.ANIMALS_AND_PET_SUPPLIES:
                    return "Animals & Pet Supplies";
                case BranchProductCategory.APPAREL_AND_ACCESSORIES:
                    return "Apparel & Accessories";
                case BranchProductCategory.ARTS_AND_ENTERTAINMENT:
                    return "Arts & Entertainment";
                case BranchProductCategory.BABY_AND_TODDLER:
                    return "Baby & Toddler";
                case BranchProductCategory.BUSINESS_AND_INDUSTRIAL:
                    return "Business & Industrial";
                case BranchProductCategory.CAMERAS_AND_OPTICS:
                    return "Cameras & Optics";
                case BranchProductCategory.ELECTRONICS:
                    return "Electronics";
                case BranchProductCategory.FOOD_BEVERAGES_AND_TOBACCO:
                    return "Food, Beverages & Tobacco";
                case BranchProductCategory.FURNITURE:
                    return "Furniture";
                case BranchProductCategory.HARDWARE:
                    return "Hardware";
                case BranchProductCategory.HEALTH_AND_BEAUTY:
                    return "Health & Beauty";
                case BranchProductCategory.HOME_AND_GARDEN:
                    return "Home & Garden";
                case BranchProductCategory.LUGGAGE_AND_BAGS:
                    return "Luggage & Bags";
                case BranchProductCategory.MATURE:
                    return "Mature";
                case BranchProductCategory.MEDIA:
                    return "Media";
                case BranchProductCategory.OFFICE_SUPPLIES:
                    return "Office Supplies";
                case BranchProductCategory.RELIGIOUS_AND_CEREMONIAL:
                    return "Religious & Ceremonial";
                case BranchProductCategory.SOFTWARE:
                    return "Software";
                case BranchProductCategory.SPORTING_GOODS:
                    return "Sporting Goods";
                case BranchProductCategory.TOYS_AND_GAMES:
                    return "Toys & Games";
                case BranchProductCategory.VEHICLES_AND_PARTS:
                    return "Vehicles & Parts";
            }
            return "";
        }
    }
}
