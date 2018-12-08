using System.Collections.Generic;

namespace COMWrapper
{
    public interface ICOMBranchLinkProperties
    {
        List<string> Tags { get; set; }
        Dictionary<string, string> ControlParams { get; set; }

        string Feature { get; set; }
        string Alias { get; set; }
        string Stage { get; set; }
        int MatchDuration { get; set; }
        string Channel { get; set; }
        string Campaign { get; set; }

        void AddTag(string tag);
        void RemoveTag(string tag);
        bool TagExist(string tag);
        void ClearTags();

        void AddControlParam(string key, string controlParam);
        void RemoveControlParam(string key);
        bool ControlParamExist(string key);
        void ClearControlParams();
    }
}
