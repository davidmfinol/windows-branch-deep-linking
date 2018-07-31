using Newtonsoft.Json.Linq;
using System;

namespace BranchSdk {
    public class BranchShortLinkBuilder : BranchUrlBuilder<BranchShortLinkBuilder> {
        public BranchShortLinkBuilder SetAlias(String alias) {
            this.alias = alias;
            return this;
        }

        public BranchShortLinkBuilder SetChannel(String channel) {
            this.channel = channel;
            return this;
        }

        public BranchShortLinkBuilder SetDuration(int duration) {
            this.duration = duration;
            return this;
        }

        public BranchShortLinkBuilder SetFeature(String feature) {
            this.feature = feature;
            return this;
        }

        public BranchShortLinkBuilder SetParameters(JObject parameters) {
            this.parameters = parameters;
            return this;
        }

        public BranchShortLinkBuilder SetStage(String stage) {
            this.stage = stage;
            return this;
        }

        public BranchShortLinkBuilder SetCampaign(String campaign) {
            this.campaign = campaign;
            return this;
        }

        public BranchShortLinkBuilder SetType(int type) {
            this.type = type;
            return this;
        }
    }
}
