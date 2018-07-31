using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public BranchLinkProperties(string dataJson) {
            Tags = new List<String>();
            Feature = "";
            Alias = "";
            Channel = "";
            Stage = "";
            MatchDuration = 0;
            ControlParams = new Dictionary<String, String>();

            LoadFromDictionary(JsonConvert.DeserializeObject<Dictionary<string, object>>(dataJson));
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

        private void LoadFromDictionary(Dictionary<string, object> data) {
            if (data == null)
                return;

            if (data.ContainsKey("~tags") && data["~tags"] != null) {
                List<object> tempList = data["~tags"] as List<object>;

                if (tempList != null) {
                    foreach (object obj in tempList) {
                        if (obj != null) {
                            Tags.Add(obj.ToString());
                        }
                    }
                }
            }
            if (data.ContainsKey("~feature") && data["~feature"] != null) {
                Feature = data["~feature"].ToString();
            }
            if (data.ContainsKey("~alias") && data["~alias"] != null) {
                Alias = data["~alias"].ToString();
            }
            if (data.ContainsKey("~channel") && data["~channel"] != null) {
                Channel = data["~channel"].ToString();
            }
            if (data.ContainsKey("~stage") && data["~stage"] != null) {
                Stage = data["~stage"].ToString();
            }
            if (data.ContainsKey("~campaign") && data["~campaign"] != null) {
                Campaign = data["~campaign"].ToString();
            }
            if (data.ContainsKey("~duration")) {
                if (!string.IsNullOrEmpty(data["~duration"].ToString())) {
                    MatchDuration = Convert.ToInt32(data["~duration"].ToString());
                }
            }
            if (data.ContainsKey("control_params")) {
                if (data["control_params"] != null) {
                    Dictionary<string, object> paramsTemp = data["control_params"] as Dictionary<string, object>;

                    if (paramsTemp != null) {
                        foreach (string key in paramsTemp.Keys) {
                            if (paramsTemp[key] != null) {
                                ControlParams.Add(key, paramsTemp[key].ToString());
                            }
                        }
                    }
                }
            }
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(this);
        }

        public static BranchLinkProperties FromJson(string json) {
            return JsonConvert.DeserializeObject<BranchLinkProperties>(json);
        }
    }
}
