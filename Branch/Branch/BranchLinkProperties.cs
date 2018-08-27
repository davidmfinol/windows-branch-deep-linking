using BranchSdk.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            JObject jsonObject = JObject.Parse(json);
            if(jsonObject.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey())
                && jsonObject[BranchJsonKey.Clicked_Branch_Link.GetKey()].Value<bool>()) {

                if (jsonObject.ContainsKey("~channel") && !string.IsNullOrEmpty(jsonObject["~channel"].Value<string>())) {
                    SetChannel(jsonObject["~channel"].Value<string>());
                }

                if (jsonObject.ContainsKey("~feature") && !string.IsNullOrEmpty(jsonObject["~feature"].Value<string>())) {
                    SetFeature(jsonObject["~feature"].Value<string>());
                }

                if (jsonObject.ContainsKey("~stage") && !string.IsNullOrEmpty(jsonObject["~stage"].Value<string>())) {
                    SetStage(jsonObject["~stage"].Value<string>());
                }

                if (jsonObject.ContainsKey("~campaign") && !string.IsNullOrEmpty(jsonObject["~campaign"].Value<string>())) {
                    SetCampaign(jsonObject["~campaign"].Value<string>());
                }

                if (jsonObject.ContainsKey("~duration") && !string.IsNullOrEmpty(jsonObject["~duration"].Value<string>())) {
                    SetDuration(jsonObject["~duration"].Value<int>());
                }

                if (jsonObject.ContainsKey("$match_duration") && !string.IsNullOrEmpty(jsonObject["$match_duration"].Value<string>())) {
                    SetDuration(jsonObject["$match_duration"].Value<int>());
                }

                if (jsonObject.ContainsKey("~tags") && jsonObject["~tags"].ToObject<List<string>>() != null) {
                    Tags = jsonObject["~tags"].ToObject<List<string>>();
                }

                foreach (JProperty prop in jsonObject.Properties()) {
                    if (prop.Name.StartsWith("$")) {
                        string value = string.Empty;

                        if (jsonObject[prop.Name].Type == JTokenType.Array) {
                            value = jsonObject[prop.Name].Value<JArray>().ToString();
                        } else if (jsonObject[prop.Name].Type == JTokenType.Object) {
                            value = jsonObject[prop.Name].Value<JObject>().ToString();
                        } else {
                            value = jsonObject[prop.Name].Value<string>();
                        }

                        AddControlParameter(prop.Name, value);
                    }
                }
            }
        }
    }
}
