using BranchSdk.Enum;
using BranchSdk.Utils;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;
using COMWrapper;

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
            CreationTimeStamp = DateTime.UtcNow.CurrentTimeMillis();
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
            CreationTimeStamp = DateTime.UtcNow.CurrentTimeMillis();

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

        public string GetShortURL(BranchLinkProperties linkProperties) {
            return GetLinkBuilder(linkProperties).GetUrl();
        }

        public void ShowShareSheet(DataTransferManager dataTransferManager, CoreDispatcher dispatcher, BranchLinkProperties linkProperties, BranchShareSheetStyle style) {
            BranchShareLinkBuilder shareLinkBuilder = new BranchShareLinkBuilder(dataTransferManager, dispatcher, GetLinkBuilder(linkProperties));
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
                shortLinkBuilder.AddParameters(BranchJsonKey.ContentTitle.GetKey(), Title);
            }
            if (!string.IsNullOrEmpty(CanonicalIdentifier)) {
                shortLinkBuilder.AddParameters(BranchJsonKey.CanonicalIdentifier.GetKey(), CanonicalIdentifier);
            }
            if (!string.IsNullOrEmpty(CanonicalUrl)) {
                shortLinkBuilder.AddParameters(BranchJsonKey.CanonicalUrl.GetKey(), CanonicalUrl);
            }
            if (Keywords.Count > 0) {
                shortLinkBuilder.AddParameters(BranchJsonKey.ContentKeyWords.GetKey(), Keywords);
            }
            if (!string.IsNullOrEmpty(ContentDescription)) {
                shortLinkBuilder.AddParameters(BranchJsonKey.ContentDesc.GetKey(), ContentDescription);
            }
            if (!string.IsNullOrEmpty(ImageUrl)) {
                shortLinkBuilder.AddParameters(BranchJsonKey.ContentImgUrl.GetKey(), ImageUrl);
            }

            shortLinkBuilder.AddParameters(BranchJsonKey.PublicallyIndexable.GetKey(), IsPublicallyIndexable());

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

        public JsonObject ConvertToJson() {
            JsonObject buoJsonModel = new JsonObject();
            try {
                JsonObject metadataJsonObject = Metadata.ConvertToJson();
                foreach(string key in metadataJsonObject.Keys) {
                    buoJsonModel.Add(key, metadataJsonObject[key]);
                }

                if (!string.IsNullOrEmpty(Title)) {
                    buoJsonModel.Add(BranchJsonKey.ContentTitle.GetKey(), JsonValue.CreateStringValue(Title));
                }
                if (!string.IsNullOrEmpty(CanonicalIdentifier)) {
                    buoJsonModel.Add(BranchJsonKey.CanonicalIdentifier.GetKey(), JsonValue.CreateStringValue(CanonicalIdentifier));
                }
                if (!string.IsNullOrEmpty(CanonicalUrl)) {
                    buoJsonModel.Add(BranchJsonKey.CanonicalUrl.GetKey(), JsonValue.CreateStringValue(CanonicalUrl));
                }
                if (!string.IsNullOrEmpty(ContentDescription)) {
                    buoJsonModel.Add(BranchJsonKey.ContentDesc.GetKey(), JsonValue.CreateStringValue(ContentDescription));
                }
                if (!string.IsNullOrEmpty(ImageUrl)) {
                    buoJsonModel.Add(BranchJsonKey.ContentImgUrl.GetKey(), JsonValue.CreateStringValue(ImageUrl));
                }

                if (Keywords.Count > 0) {
                    JsonArray keyWordJsonArray = new JsonArray();
                    foreach (string keyword in Keywords) {
                        keyWordJsonArray.Add(JsonValue.CreateStringValue(keyword));
                    }
                    buoJsonModel.Add(BranchJsonKey.ContentKeyWords.GetKey(), keyWordJsonArray);
                }

                buoJsonModel.Add(BranchJsonKey.PublicallyIndexable.GetKey(), JsonValue.CreateBooleanValue(IsPublicallyIndexable()));
                buoJsonModel.Add(BranchJsonKey.LocallyIndexable.GetKey(), JsonValue.CreateBooleanValue(IsLocallyIndexable()));
                buoJsonModel.Add(BranchJsonKey.CreationTimestamp.GetKey(), JsonValue.CreateNumberValue(CreationTimeStamp));
            } catch (Exception ignore) {
            }
            return buoJsonModel;
        }

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            JsonObject jsonObject = JsonObject.Parse(json);
            if (jsonObject.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey())
                && jsonObject[BranchJsonKey.Clicked_Branch_Link.GetKey()].GetBoolean()) {

                Metadata = new BranchContentMetadata(json);

                if (jsonObject.ContainsKey(BranchJsonKey.ContentTitle.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ContentTitle.GetKey()].GetString())) {
                    SetTitle(jsonObject[BranchJsonKey.ContentTitle.GetKey()].GetString());
                }
                if (jsonObject.ContainsKey(BranchJsonKey.CanonicalIdentifier.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.CanonicalIdentifier.GetKey()].GetString())) {
                    SetCanonicalIdentifier(jsonObject[BranchJsonKey.CanonicalIdentifier.GetKey()].GetString());
                }
                if (jsonObject.ContainsKey(BranchJsonKey.CanonicalUrl.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.CanonicalUrl.GetKey()].GetString())) {
                    SetCanonicalUrl(jsonObject[BranchJsonKey.CanonicalUrl.GetKey()].GetString());
                }
                if (jsonObject.ContainsKey(BranchJsonKey.ContentDesc.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ContentDesc.GetKey()].GetString())) {
                    SetContentDescription(jsonObject[BranchJsonKey.ContentDesc.GetKey()].GetString());
                }
                if (jsonObject.ContainsKey(BranchJsonKey.ContentImgUrl.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ContentImgUrl.GetKey()].GetString())) {
                    SetContentImageUrl(jsonObject[BranchJsonKey.ContentImgUrl.GetKey()].GetString());
                }

                if (jsonObject.ContainsKey(BranchJsonKey.ContentKeyWords.GetKey()) && jsonObject[BranchJsonKey.ContentKeyWords.GetKey()].GetArray().DeserializeObject<List<string>>() != null) {
                    Keywords = jsonObject[BranchJsonKey.ContentKeyWords.GetKey()].GetArray().DeserializeObject<List<string>>();
                }

                if (jsonObject.ContainsKey(BranchJsonKey.PublicallyIndexable.GetKey())) {
                    if (jsonObject[BranchJsonKey.PublicallyIndexable.GetKey()].GetBoolean()) {
                        ContentIndexMode = ContentIndexModes.PRIVATE;
                    }
                }
                if (jsonObject.ContainsKey(BranchJsonKey.LocallyIndexable.GetKey())) {
                    if (jsonObject[BranchJsonKey.LocallyIndexable.GetKey()].GetBoolean()) {
                        LocalIndexMode = ContentIndexModes.PUBLIC;
                    }
                }
                if (jsonObject.ContainsKey(BranchJsonKey.CreationTimestamp.GetKey())) {
                    CreationTimeStamp = (long)jsonObject[BranchJsonKey.LocallyIndexable.GetKey()].GetNumber();
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
