using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    [ComVisible(true)]
    public interface ICOMBranchUniversalObject
    {
        string CanonicalIdentifier { get; set; }
        string CanonicalUrl { get; set; }
        string Title { get; set; }
        string ContentDescription { get; set; }
        string ImageUrl { get; set; }
        ICOMBranchContentMetadata Metadata { get; set; }
        string ContentIndexMode { get; set; }
        string LocalIndexMode { get; set; } 
        long CreationTimeStamp { get; set; }
        List<string> Keywords { get; set; }

        void AddKeyword(string keyword);
        void RemoveKeyword(string keyword);
        bool KeywordExist(string keyword);
        void ClearKeywords();

        string GetShortURL(ICOMBranchLinkProperties linkProperties);
    }
}
