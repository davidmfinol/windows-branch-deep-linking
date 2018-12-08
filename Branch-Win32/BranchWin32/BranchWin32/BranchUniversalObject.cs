using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using COMWrapper;
using BranchSdk.Utils;

namespace BranchSdk {
    public class BranchUniversalObject {
        public string CanonicalIdentifier { get; private set; }
        public string CanonicalUrl { get; private set; }
        public string Title { get; private set; }
        public string ContentDescription { get; private set; }
        public string ImageUrl { get; private set; }
        public BranchContentMetadata Metadata { get; private set; }
        public ContentIndexModes ContentIndexMode { get; private set; }
        public ContentIndexModes LocalIndexMode { get; private set; }
        public List<string> Keywords { get; private set; }
        public long CreationTimeStamp { get; private set; }

        public enum ContentIndexModes {
            PUBLIC,
            PRIVATE
        }

        public BranchUniversalObject() {
            CanonicalIdentifier = "";
            CanonicalUrl = "";
            Title = "";
            ContentDescription = "";
            ImageUrl = "";
            Metadata = new BranchContentMetadata();
            ContentIndexMode = 0;
            LocalIndexMode = 0;
            Keywords = new List<string>();
            CreationTimeStamp = DateTimeUtils.CurrentTimeMillis(DateTime.UtcNow);
        }

        public BranchUniversalObject(string json) {
            CanonicalIdentifier = "";
            CanonicalUrl = "";
            Title = "";
            ContentDescription = "";
            ImageUrl = "";
            Metadata = new BranchContentMetadata();
            ContentIndexMode = 0;
            LocalIndexMode = 0;
            Keywords = new List<string>();
            CreationTimeStamp = DateTimeUtils.CurrentTimeMillis(DateTime.UtcNow);

            LoadFromJson(json);
        }

        public BranchUniversalObject SetCanonicalIdentifier(string canonicalIdentifier) {
            CanonicalIdentifier = canonicalIdentifier;
            return this;
        }

        public BranchUniversalObject SetCanonicalUrl(string canonicalUrl) {
            CanonicalUrl = canonicalUrl;
            return this;
        }

        public BranchUniversalObject SetTitle(string title) {
            Title = title;
            return this;
        }

        public BranchUniversalObject SetContentDescription(string description) {
            ContentDescription = description;
            return this;
        }

        public BranchUniversalObject SetContentImageUrl(string imageUrl) {
            ImageUrl = imageUrl;
            return this;
        }

        public BranchUniversalObject AddContentMetadata(Dictionary<string, string> metadata) {
            if (metadata != null) {
                foreach (string key in metadata.Keys) {
                    Metadata.AddCustomMetadata(key, metadata[key]);
                }
            }
            return this;
        }

        public BranchUniversalObject AddContentMetadata(string key, string value) {
            if (Metadata != null) {
                Metadata.AddCustomMetadata(key, value);
            }
            return this;
        }

        public BranchUniversalObject SetContentMetadata(BranchContentMetadata metadata) {
            Metadata = metadata;
            return this;
        }

        public BranchUniversalObject SetContentIndexingMode(ContentIndexModes indexMode) {
            ContentIndexMode = indexMode;
            return this;
        }

        public BranchUniversalObject SetLocalIndexMode(ContentIndexModes localIndexMode) {
            LocalIndexMode = localIndexMode;
            return this;
        }

        public BranchUniversalObject AddKeyWord(string keyword) {
            Keywords.Add(keyword);
            return this;
        }

        public void SetCreationTimeStamp(long timestamp)
        {
            CreationTimeStamp = timestamp;
        }

        public void SetKeywords(List<string> keywords)
        {
            Keywords = keywords;
        }

        public void GetShortURL(BranchLinkProperties linkProperties, Action<string> onFinish) {
            GetLinkBuilder(linkProperties).GetUrl(onFinish);
        }

        public void ShowShareSheet(BranchLinkProperties linkProperties, BranchShareSheetStyle style) {
            BranchShareLinkBuilder shareLinkBuilder = new BranchShareLinkBuilder(GetLinkBuilder(linkProperties));
            shareLinkBuilder.SetTitle(style.GetMessageTitle());
            shareLinkBuilder.SetMessage(style.GetMessageBody());
            shareLinkBuilder.SetDefaultURL(style.GetDefaultUrl());
            shareLinkBuilder.SetBitmap(style.GetBitmap());
            shareLinkBuilder.ShareLink();
        }

        private BranchShortLinkBuilder GetLinkBuilder(BranchLinkProperties linkProperties) {
            BranchShortLinkBuilder branchShortLink = new BranchShortLinkBuilder();
            return GetLinkBuilder(branchShortLink, linkProperties);
        }

        private BranchShortLinkBuilder GetLinkBuilder(BranchShortLinkBuilder shortLinkBuilder, BranchLinkProperties linkProperties) {
            if (linkProperties.Tags != null) {
                shortLinkBuilder.AddTags(linkProperties.Tags);
            }
            if (!string.IsNullOrEmpty(linkProperties.Feature)) {
                shortLinkBuilder.SetFeature(linkProperties.Feature);
            }
            if (!string.IsNullOrEmpty(linkProperties.Alias)) {
                shortLinkBuilder.SetAlias(linkProperties.Alias);
            }
            if (!string.IsNullOrEmpty(linkProperties.Channel)) {
                shortLinkBuilder.SetChannel(linkProperties.Channel);
            }
            if (!string.IsNullOrEmpty(linkProperties.Stage)) {
                shortLinkBuilder.SetStage(linkProperties.Stage);
            }
            if (!string.IsNullOrEmpty(linkProperties.Campaign)) {
                shortLinkBuilder.SetCampaign(linkProperties.Campaign);
            }
            if (linkProperties.MatchDuration > 0) {
                shortLinkBuilder.SetDuration(linkProperties.MatchDuration);
            }

            if (!string.IsNullOrEmpty(Title)) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.ContentTitle), Title);
            }
            if (!string.IsNullOrEmpty(CanonicalIdentifier)) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalIdentifier), CanonicalIdentifier);
            }
            if (!string.IsNullOrEmpty(CanonicalUrl)) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalUrl), CanonicalUrl);
            }
            if (Keywords.Count > 0) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.ContentKeyWords), Keywords);
            }
            if (!string.IsNullOrEmpty(ContentDescription)) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.ContentDesc), ContentDescription);
            }
            if (!string.IsNullOrEmpty(ImageUrl)) {
                shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.ContentImgUrl), ImageUrl);
            }

            shortLinkBuilder.AddParameters(BranchEnumUtils.GetKey(BranchJsonKey.PublicallyIndexable), IsPublicallyIndexable());

            Dictionary<string, object> metadata = Metadata.ConvertToDictionary();
            foreach (string key in metadata.Keys) {
                shortLinkBuilder.AddParameters(key, metadata[key]);
            }

            Dictionary<string, string> controlParam = linkProperties.ControlParams;
            foreach (string key in controlParam.Keys) {
                shortLinkBuilder.AddParameters(key, controlParam[key]);
            }
            return shortLinkBuilder;
        }

        public bool IsPublicallyIndexable() {
            return ContentIndexMode == ContentIndexModes.PRIVATE;
        }

        public bool IsLocallyIndexable() {
            return LocalIndexMode == ContentIndexModes.PUBLIC;
        }

        public Dictionary<string, object> ConvertToJson() {
            Dictionary<string, object> buoJsonModel = new Dictionary<string, object>();
            try {
                Dictionary<string, object> metadataJsonObject = Metadata.ConvertToJson();
                foreach(string key in metadataJsonObject.Keys) {
                    buoJsonModel.Add(key, metadataJsonObject[key]);
                }

                if (!string.IsNullOrEmpty(Title)) {
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentTitle), Title);
                }
                if (!string.IsNullOrEmpty(CanonicalIdentifier)) {
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalIdentifier), CanonicalIdentifier);
                }
                if (!string.IsNullOrEmpty(CanonicalUrl)) {
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalUrl), CanonicalUrl);
                }
                if (!string.IsNullOrEmpty(ContentDescription)) {
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentDesc), ContentDescription);
                }
                if (!string.IsNullOrEmpty(ImageUrl)) {
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentImgUrl), ImageUrl);
                }

                if (Keywords.Count > 0) {
                    List<string> keyWordJsonArray = new List<string>();
                    foreach (string keyword in Keywords) {
                        keyWordJsonArray.Add(keyword);
                    }
                    buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentKeyWords), keyWordJsonArray);
                }

                buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.PublicallyIndexable), IsPublicallyIndexable());
                buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.LocallyIndexable), IsLocallyIndexable());
                buoJsonModel.Add(BranchEnumUtils.GetKey(BranchJsonKey.CreationTimestamp), CreationTimeStamp);
            } catch (Exception ignore) {
            }
            return buoJsonModel;
        }

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            Dictionary<string, object> jsonObject = (Dictionary<string, object>)Json.Deserialize(json);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link))
                && (bool)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link)]) {

                Metadata = new BranchContentMetadata(json);

                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ContentTitle)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentTitle)])) {
                    SetTitle((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentTitle)]);
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalIdentifier)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.CanonicalIdentifier)])) {
                    SetCanonicalIdentifier((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.CanonicalIdentifier)]);
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.CanonicalUrl)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.CanonicalUrl)])) {
                    SetCanonicalUrl((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.CanonicalUrl)]);
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ContentDesc)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentDesc)])) {
                    SetContentDescription((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentDesc)]);
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ContentImgUrl)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentImgUrl)])) {
                    SetContentImageUrl((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentImgUrl)]);
                }

                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ContentKeyWords)) && jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentKeyWords)] != null) {
                    Keywords = ObjectUtils.ListObjectToListString((List<object>)Json.Deserialize((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentKeyWords)]));
                }

                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.PublicallyIndexable))) {
                    if ((bool)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.PublicallyIndexable)]) {
                        ContentIndexMode = ContentIndexModes.PRIVATE;
                    }
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.LocallyIndexable))) {
                    if ((bool)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.LocallyIndexable)]) {
                        LocalIndexMode = ContentIndexModes.PUBLIC;
                    }
                }
                if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.CreationTimestamp))) {
                    CreationTimeStamp = (long)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.LocallyIndexable)];
                }
            }
        }

        public ICOMBranchUniversalObject ParseNativeBUO()
        {
            ICOMBranchUniversalObject comBUO = new COMBranchUniversalObject();
            comBUO.CanonicalIdentifier = CanonicalIdentifier;
            comBUO.CanonicalUrl = CanonicalUrl;
            comBUO.Title = Title;
            comBUO.ContentDescription = ContentDescription;
            comBUO.ImageUrl = ImageUrl;
            comBUO.Metadata = Metadata.ParseNativeMetadata();
            comBUO.ContentIndexMode = ContentIndexMode.ToString();
            comBUO.LocalIndexMode = LocalIndexMode.ToString();
            comBUO.CreationTimeStamp = CreationTimeStamp;
            comBUO.Keywords = Keywords;
            return comBUO;
        }
    }
}
