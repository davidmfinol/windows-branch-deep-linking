using Windows.Data.Json;
using System;

namespace BranchSdk {
    public class BranchShortLinkBuilder : BranchUrlBuilder<BranchShortLinkBuilder> {
        public BranchShortLinkBuilder SetAlias(string alias) {
            this.alias = alias;
            return this;
        }

        public BranchShortLinkBuilder SetChannel(string channel) {
            this.channel = channel;
            return this;
        }

        public BranchShortLinkBuilder SetDuration(int duration) {
            this.duration = duration;
            return this;
        }

        public BranchShortLinkBuilder SetFeature(string feature) {
            this.feature = feature;
            return this;
        }

        public BranchShortLinkBuilder SetParameters(JsonObject parameters) {
            this.parameters = parameters;
            return this;
        }

        public BranchShortLinkBuilder SetStage(string stage) {
            this.stage = stage;
            return this;
        }

        public BranchShortLinkBuilder SetCampaign(string campaign) {
            this.campaign = campaign;
            return this;
        }

        public BranchShortLinkBuilder SetType(int type) {
            this.type = type;
            return this;
        }
    }
}
