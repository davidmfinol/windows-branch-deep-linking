using BranchSdk.Enum;
using System.Collections.Generic;
using COMWrapper;
using System;
using BranchSdk.Utils;

namespace BranchSdk
{
    public class BranchLinkProperties
    {
        public List<string> Tags { get; private set; }
        public string Feature { get; private set; }
        public string Alias { get; private set; }
        public string Stage { get; private set; }
        public int MatchDuration { get; private set; }
        public Dictionary<string, string> ControlParams { get; private set; }
        public string Channel { get; private set; }
        public string Campaign { get; private set; }

        public BranchLinkProperties()
        {
            Tags = new List<string>();
            Feature = "Share";
            ControlParams = new Dictionary<string, string>();
            Alias = "";
            Stage = "";
            MatchDuration = 0;
            Channel = "";
            Campaign = "";
        }

        public BranchLinkProperties(string json)
        {
            Tags = new List<string>();
            Feature = "Share";
            ControlParams = new Dictionary<string, string>();
            Alias = "";
            Stage = "";
            MatchDuration = 0;
            Channel = "";
            Campaign = "";

            LoadFromJson(json);
        }

        public BranchLinkProperties SetAlias(string alias)
        {
            this.Alias = alias;
            return this;
        }

        public BranchLinkProperties AddTag(string tag)
        {
            this.Tags.Add(tag);
            return this;
        }

        public BranchLinkProperties AddControlParameter(string key, string value)
        {
            this.ControlParams.Add(key, value);
            return this;
        }

        public BranchLinkProperties SetFeature(string feature)
        {
            this.Feature = feature;
            return this;
        }

        public BranchLinkProperties SetDuration(int duration)
        {
            this.MatchDuration = duration;
            return this;
        }

        public BranchLinkProperties SetStage(string stage)
        {
            this.Stage = stage;
            return this;
        }

        public BranchLinkProperties SetChannel(string channel)
        {
            this.Channel = channel;
            return this;
        }

        public BranchLinkProperties SetCampaign(string campaign)
        {
            this.Campaign = campaign;
            return this;
        }

        public void SetTags(List<string> tags)
        {
            Tags = tags;
        }

        public void SetControlParams(Dictionary<string, string> controlParams)
        {
            ControlParams = controlParams;
        }

        private void LoadFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                return;

            Dictionary<string, object> jsonObject = (Dictionary<string, object>)Json.Deserialize(json);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link))
                && (bool)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link)]) {

                if (jsonObject.ContainsKey("~channel") && !string.IsNullOrEmpty((string)jsonObject["~channel"])) {
                    SetChannel((string)jsonObject["~channel"]);
                }

                if (jsonObject.ContainsKey("~feature") && !string.IsNullOrEmpty((string)jsonObject["~feature"])) {
                    SetFeature((string)jsonObject["~feature"]);
                }

                if (jsonObject.ContainsKey("~stage") && !string.IsNullOrEmpty((string)jsonObject["~stage"])) {
                    SetStage((string)jsonObject["~stage"]);
                }

                if (jsonObject.ContainsKey("~campaign") && !string.IsNullOrEmpty((string)jsonObject["~campaign"])) {
                    SetCampaign((string)jsonObject["~campaign"]);
                }

                if (jsonObject.ContainsKey("~duration")) {
                    SetDuration(Convert.ToInt32(jsonObject["~duration"]));
                }

                if (jsonObject.ContainsKey("$match_duration")) {
                    SetDuration(Convert.ToInt32(jsonObject["$match_duration"]));
                }

                if (jsonObject.ContainsKey("~tags") && jsonObject["~tags"] != null) {
                    Tags = ObjectUtils.ListObjectToListString((List<object>)jsonObject["~tags"]);
                }

                foreach (string key in jsonObject.Keys) {
                    if (key.StartsWith("$")) {
                        string value = jsonObject[key].ToString();
                        AddControlParameter(key, value);
                    }
                }
            }
        }

        public ICOMBranchLinkProperties ParseNativeLinkProperties()
        {
            ICOMBranchLinkProperties comLink = new COMBranchLinkProperties();
            comLink.Tags = Tags;
            comLink.ControlParams = ControlParams;
            comLink.Feature = Feature;
            comLink.Alias = Alias;
            comLink.Stage = Stage;
            comLink.MatchDuration = MatchDuration;
            comLink.Channel = Channel;
            comLink.Campaign = Campaign;
            return comLink;
        }
    }
}
