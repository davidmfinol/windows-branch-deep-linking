using BranchSdk;
using BranchSdk.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace COMWrapper
{
    public class COMBranchUniversalObject : ICOMBranchUniversalObject
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void COMGenerateShortLinkCallback([MarshalAs(UnmanagedType.LPTStr)] StringBuilder link);

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

        public unsafe void GetShortURL(ICOMBranchLinkProperties linkProperties, void* callback)
        {
            BranchUniversalObject buo = ParseCOMBUO();
            BranchLinkProperties link = (linkProperties as COMBranchLinkProperties).ParseCOMLinkProperties();
            buo.GetShortURL((linkProperties as COMBranchLinkProperties).ParseCOMLinkProperties(), (comLink) => {
                Console.WriteLine(comLink);

                COMGenerateShortLinkCallback comCallback
                    = (COMGenerateShortLinkCallback)Marshal.GetDelegateForFunctionPointer((IntPtr)callback, typeof(COMGenerateShortLinkCallback));
                comCallback.Invoke(new StringBuilder(comLink));
            });
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

            buo.SetContentIndexingMode(EnumUtils.TryParse<BranchUniversalObject.ContentIndexModes>(ContentIndexMode));
            buo.SetLocalIndexMode(EnumUtils.TryParse<BranchUniversalObject.ContentIndexModes>(LocalIndexMode));

            buo.SetCreationTimeStamp(CreationTimeStamp);
            buo.SetKeywords(Keywords);

            return buo;
        }
    }
}
