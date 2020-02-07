using BranchSdk;
using System.Collections.Generic;

namespace COMWrapper
{
    public class COMBranchLinkProperties : ICOMBranchLinkProperties
    {
        public List<string> Tags { get; set; }
        public Dictionary<string, string> ControlParams { get; set; }
        public string Feature { get; set; }
        public string Alias { get; set; }
        public string Stage { get; set; }
        public int MatchDuration { get; set; }
        public string Channel { get; set; }
        public string Campaign { get; set; }

        public COMBranchLinkProperties()
        {
            Tags = new List<string>();
            ControlParams = new Dictionary<string, string>();
        }

        public void AddControlParam(string key, string controlParam)
        {
            if (ControlParams.ContainsKey(key)) {
                ControlParams[key] = controlParam;
            } else {
                ControlParams.Add(key, controlParam);
            }
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public void ClearControlParams()
        {
            ControlParams.Clear();
        }

        public void ClearTags()
        {
            Tags.Clear();
        }

        public bool ControlParamExist(string key)
        {
            return ControlParams.ContainsKey(key);
        }

        public bool TagExist(string tag)
        {
            return Tags.Contains(tag);
        }

        public void RemoveControlParam(string key)
        {
            if (ControlParams.ContainsKey(key)) {
                ControlParams.Remove(key);
            }
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public BranchLinkProperties ParseCOMLinkProperties()
        {
            BranchLinkProperties linkProperties = new BranchLinkProperties();
            linkProperties.SetTags(Tags);
            linkProperties.SetControlParams(ControlParams);
            linkProperties.SetFeature (Feature);
            linkProperties.SetAlias( Alias);
            linkProperties.SetStage(Stage);
            linkProperties.SetDuration(MatchDuration);
            linkProperties.SetChannel(Channel);
            linkProperties.SetCampaign(Campaign);
            return linkProperties;
        }
    }
}
