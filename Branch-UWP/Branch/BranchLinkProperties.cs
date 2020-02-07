using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using COMWrapper;

namespace BranchSdk {
    public class BranchLinkProperties {
        public List<string> Tags { get; private set; }
        public string Feature { get; private set; }
        public string Alias { get; private set; }
        public string Stage { get; private set; }
        public int MatchDuration { get; private set; }
        public Dictionary<string, string> ControlParams { get; private set; }
        public string Channel { get; private set; }
        public string Campaign { get; private set; }

        public BranchLinkProperties() {
            Tags = new List<string>();
            Feature = "Share";
            ControlParams = new Dictionary<string, string>();
            Alias = "";
            Stage = "";
            MatchDuration = 0;
            Channel = "";
            Campaign = "";
        }

        public BranchLinkProperties(string json) {
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

        public BranchLinkProperties SetAlias(string alias) {
            this.Alias = alias;
            return this;
        }

        public BranchLinkProperties AddTag(string tag) {
            this.Tags.Add(tag);
            return this;
        }

        public BranchLinkProperties AddControlParameter(string key, string value) {
            this.ControlParams.Add(key, value);
            return this;
        }

        public BranchLinkProperties SetFeature(string feature) {
            this.Feature = feature;
            return this;
        }

        public BranchLinkProperties SetDuration(int duration) {
            this.MatchDuration = duration;
            return this;
        }

        public BranchLinkProperties SetStage(string stage) {
            this.Stage = stage;
            return this;
        }

        public BranchLinkProperties SetChannel(string channel) {
            this.Channel = channel;
            return this;
        }

        public BranchLinkProperties SetCampaign(string campaign) {
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

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            JsonObject jsonObject = JsonObject.Parse(json);
            if(jsonObject.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey())
                && jsonObject[BranchJsonKey.Clicked_Branch_Link.GetKey()].GetBoolean()) {

                if (jsonObject.ContainsKey("~channel") && !string.IsNullOrEmpty(jsonObject["~channel"].GetString())) {
                    SetChannel(jsonObject["~channel"].GetString());
                }

                if (jsonObject.ContainsKey("~feature") && !string.IsNullOrEmpty(jsonObject["~feature"].GetString())) {
                    SetFeature(jsonObject["~feature"].GetString());
                }

                if (jsonObject.ContainsKey("~stage") && !string.IsNullOrEmpty(jsonObject["~stage"].GetString())) {
                    SetStage(jsonObject["~stage"].GetString());
                }

                if (jsonObject.ContainsKey("~campaign") && !string.IsNullOrEmpty(jsonObject["~campaign"].GetString())) {
                    SetCampaign(jsonObject["~campaign"].GetString());
                }

                if (jsonObject.ContainsKey("~duration")) {
                    SetDuration((int)jsonObject["~duration"].GetNumber());
                }

                if (jsonObject.ContainsKey("$match_duration")) {
                    SetDuration((int)jsonObject["$match_duration"].GetNumber());
                }

                if (jsonObject.ContainsKey("~tags") && jsonObject["~tags"].GetArray().DeserializeObject<List<string>>() != null) {
                    Tags = jsonObject["~tags"].GetArray().DeserializeObject<List<string>>();
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
