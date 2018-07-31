using BranchSdk.Net;
using BranchSdk.Net.Requests;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk {
    public abstract class BranchUrlBuilder<T> where T : BranchUrlBuilder<T> {
        protected JObject parameters;
        protected string channel;
        protected string feature;
        protected string stage;
        protected string campaign;
        protected string alias;
        protected int type = Branch.LINK_TYPE_UNLIMITED_USE;
        protected int duration;
        protected List<string> tags;

        public T AddTag(string tag) {
            if (this.tags == null) {
                this.tags = new List<string>();
            }
            this.tags.Add(tag);
            return (T)this;
        }

        public T AddTags(List<string> tags) {
            if (this.tags == null) {
                this.tags = new List<string>();
            }
            this.tags.AddRange(tags);
            return (T)this;
        }

        public T AddParameters(string key, object value) {
            if (this.parameters == null) {
                this.parameters = new JObject();
            }
            this.parameters.Add(new JProperty(key, value));
            return (T)this;
        }

        public virtual string GetUrl() {
            string shortUrl = null;
            BranchServerCreateUrl request = new BranchServerCreateUrl(alias, type, duration, tags, channel, feature, stage, campaign, BranchUtil.FormatLinkParam(parameters), null);
            request.RequestType = RequestTypes.POST;
            shortUrl = Branch.I.GenerateShortLinkInternal(request);
            return shortUrl;
        }
    }
}
