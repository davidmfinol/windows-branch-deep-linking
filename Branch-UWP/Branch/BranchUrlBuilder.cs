using BranchSdk.Net;
using BranchSdk.Net.Requests;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using BranchSdk.Utils;

namespace BranchSdk {
    public abstract class BranchUrlBuilder<T> where T : BranchUrlBuilder<T> {
        protected JsonObject parameters;
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
                this.parameters = new JsonObject();
            }
            if (value is string) this.parameters.Add(key, JsonValue.CreateStringValue((string)value));
            else if (value.IsNumber()) {
                if(value is float) {
                    this.parameters.Add(key, JsonValue.CreateNumberValue((float)value));
                } else {
                    this.parameters.Add(key, JsonValue.CreateNumberValue(Convert.ToInt32(value)));
                }
            } else if (value is bool) this.parameters.Add(key, JsonValue.CreateBooleanValue((bool)value));
            else if (value is null) this.parameters.Add(key, JsonValue.CreateNullValue());
            else if (value.IsArray() || value.IsDictionary() || value.IsList()) {
                this.parameters.Add(key, value.SerializeContainerAsJson());
            } else {
                this.parameters.Add(key, value.SerializeObjectAsJson());
            }
            return (T)this;
        }

        public virtual string GetUrl() {
            string shortUrl = null;
            BranchServerCreateUrl request = new BranchServerCreateUrl(alias, type, duration, tags, channel, feature, stage, campaign, BranchUtil.FormatLinkParam(parameters), null);
            request.RequestType = RequestTypes.POST;
            shortUrl = Branch.I.GenerateShortLinkInternal(request);
            return shortUrl;
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
