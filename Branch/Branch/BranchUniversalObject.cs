using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

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
        public DateTime? ExpirationDate { get; private set; }

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
            ExpirationDate = null;
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

        public BranchUniversalObject SetContentExpiration(DateTime expirationDate) {
            ExpirationDate = expirationDate;
            return this;
        }

        public string GetShortURL(BranchLinkProperties linkProperties) {
            return GetLinkBuilder(linkProperties).GetUrl();
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

            //todo
            //shortLinkBuilder.AddParameters(BranchJsonKey.ContentExpiryTime.GetKey(), "" + ExpirationDate);
            shortLinkBuilder.AddParameters(BranchJsonKey.PublicallyIndexable.GetKey(), "" + IsPublicallyIndexable());

            JObject metadataJson = Metadata.ConvertToJson();
            foreach (JProperty prop in metadataJson.Properties()) {
                shortLinkBuilder.AddParameters(prop.Name, (string)prop.Value);
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

        public JObject ConvertToJson() {
            JObject buoJsonModel = new JObject();
            try {
                JObject metadataJsonObject = Metadata.ConvertToJson();
                foreach(JProperty key in metadataJsonObject.Properties()) {
                    buoJsonModel.Add(key.Name, key.Value);
                }
                if (!string.IsNullOrEmpty(Title)) {
                    buoJsonModel.Add(BranchJsonKey.ContentTitle.GetKey(), Title);
                }
                if (!string.IsNullOrEmpty(CanonicalIdentifier)) {
                    buoJsonModel.Add(BranchJsonKey.CanonicalIdentifier.GetKey(), CanonicalIdentifier);
                }
                if (!string.IsNullOrEmpty(CanonicalUrl)) {
                    buoJsonModel.Add(BranchJsonKey.CanonicalUrl.GetKey(), CanonicalUrl);
                }
                if (Keywords.Count > 0) {
                    JArray keyWordJsonArray = new JArray();
                    foreach (string keyword in Keywords) {
                        keyWordJsonArray.Add(keyword);
                    }
                    buoJsonModel.Add(BranchJsonKey.ContentKeyWords.GetKey(), keyWordJsonArray);
                }
                if (!string.IsNullOrEmpty(ContentDescription)) {
                    buoJsonModel.Add(BranchJsonKey.ContentDesc.GetKey(), ContentDescription);
                }
                if (!string.IsNullOrEmpty(ImageUrl)) {
                    buoJsonModel.Add(BranchJsonKey.ContentImgUrl.GetKey(), ImageUrl);
                }
                //todo
                //if (expirationInMilliSec_ > 0) {
                //    buoJsonModel.put(Defines.Jsonkey.ContentExpiryTime.getKey(), expirationInMilliSec_);
                //}
                buoJsonModel.Add(BranchJsonKey.PublicallyIndexable.GetKey(), IsPublicallyIndexable());
                buoJsonModel.Add(BranchJsonKey.LocallyIndexable.GetKey(), IsLocallyIndexable());
                //buoJsonModel.Add(BranchJsonKey.CreationTimestamp.GetKey(), creationTimeStamp_);

            } catch (Exception ignore) {
            }
            return buoJsonModel;
        }
    }
}
