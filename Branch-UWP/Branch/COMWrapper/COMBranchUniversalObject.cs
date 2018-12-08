using BranchSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    public class COMBranchUniversalObject : ICOMBranchUniversalObject
    {
        public string CanonicalIdentifier { get; set; }
        public string CanonicalUrl { get; set; }
        public string Title { get; set; }
        public string ContentDescription { get; set; }
        public string ImageUrl { get; set; }
        public ICOMBranchContentMetadata Metadata { get; set; }
        public string ContentIndexMode { get; set; }
        public string LocalIndexMode { get; set; }
        public long CreationTimeStamp { get; set; }
        public List<string> Keywords { get; set; }

        public COMBranchUniversalObject()
        {
            Keywords = new List<string>();
            Metadata = new COMBranchContentMetadata();
        }

        public void AddKeyword(string keyword)
        {
            Keywords.Add(keyword);
        }

        public void ClearKeywords()
        {
            Keywords.Clear();
        }

        public bool KeywordExist(string keyword)
        {
            return Keywords.Contains(keyword);
        }

        public void RemoveKeyword(string keyword)
        {
            Keywords.Remove(keyword);
        }

        public string GetShortURL(ICOMBranchLinkProperties linkProperties)
        {
            BranchUniversalObject buo = ParseCOMBUO();
            BranchLinkProperties link = (linkProperties as COMBranchLinkProperties).ParseCOMLinkProperties();
            return buo.GetShortURL((linkProperties as COMBranchLinkProperties).ParseCOMLinkProperties());
        }

        public BranchUniversalObject ParseCOMBUO()
        {
            BranchUniversalObject buo = new BranchUniversalObject();
            buo.SetCanonicalIdentifier(CanonicalIdentifier);
            buo.SetCanonicalUrl(CanonicalUrl);
            buo.SetTitle(Title);
            buo.SetContentDescription(ContentDescription);
            buo.SetContentImageUrl(ImageUrl);
            buo.SetContentMetadata((Metadata as COMBranchContentMetadata).ParseCOMMetadata());

            if (Enum.TryParse(ContentIndexMode, out BranchUniversalObject.ContentIndexModes contentIndexMode)) {
                buo.SetContentIndexingMode(contentIndexMode);
            }

            if (Enum.TryParse(LocalIndexMode, out BranchUniversalObject.ContentIndexModes localIndexMode)) {
                buo.SetLocalIndexMode(localIndexMode);
            }

            buo.SetCreationTimeStamp(CreationTimeStamp);
            buo.SetKeywords(Keywords);

            return buo;
        }
    }
}
