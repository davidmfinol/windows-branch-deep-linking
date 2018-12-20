using BranchSdk.Net;
using System;
using System.Collections.Generic;
using BranchSdk.Utils;
using BranchSdk.Net.Requests;

namespace BranchSdk {
    public abstract class BranchUrlBuilder<T> where T : BranchUrlBuilder<T> {
        protected Dictionary<string, object> parameters;
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
                this.parameters = new Dictionary<string, object>();
            }
            if (value is string) this.parameters.Add(key, (string)value);
            else if (ObjectUtils.IsNumber(value)) {
                if (value is float) {
                    this.parameters.Add(key, (float)value);
                } else {
                    this.parameters.Add(key, Convert.ToInt32(value));
                }
            } else if (value is bool) this.parameters.Add(key, (bool)value);
            else if (value is null) this.parameters.Add(key, null);
            else if (ObjectUtils.IsArray(value) || ObjectUtils.IsDictionary(value) || ObjectUtils.IsList(value)) {
                this.parameters.Add(key, Json.Serialize(value));
            } else {
                this.parameters.Add(key, Json.Serialize(value));
            }
            return (T)this;
        }

        public virtual void GetUrl(Action<string> onFinish) {
            BranchServerCreateUrl request = new BranchServerCreateUrl(alias, type, duration, tags, channel, feature, stage, campaign, BranchUtil.FormatLinkParam(parameters), null);
            request.RequestType = RequestTypes.POST;
            Branch.I.GenerateShortLinkInternal(request, onFinish);
        }

        public virtual void GetUrlWithCallback(Action<string> callback) {
            BranchServerCreateUrl request = new BranchServerCreateUrl(alias, type, duration, tags, channel, feature, stage, campaign, BranchUtil.FormatLinkParam(parameters), (url, error) => {
                callback.Invoke(url);
            });
            request.RequestType = RequestTypes.POST;
            Branch.I.GenerateShortLinkInternalWithCallback(request);
        }
    }
}
