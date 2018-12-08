using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace COMWrapper
{
    [ComVisible(true)]
    public interface ICOMBranchContentMetadata
    {
        string ContentSchema { get; set; }
        double Quantity { get; set; }
        double Price { get; set; }
        string CurrencyType { get; set; }
        string Sku { get; set; }
        string ProductName { get; set; }
        string ProductBrand { get; set; }
        string ProductCategory { get; set; }
        string Condition { get; set; }
        string ProductVariant { get; set; }
        double Rating { get; set; }
        double RatingAverage { get; set; }
        int RatingCount { get; set; }
        double RatingMax { get; set; }
        string AddressStreet { get; set; }
        string AddressCity { get; set; }
        string AddressRegion { get; set; }
        string AddressCountry { get; set; }
        string AddressPostalCode { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }

        List<string> ImageCaptions { get; set; }
        Dictionary<string, string> CustomMetadata { get; set; }

        void AddImageCaption(string imageCaption);
        void RemoveImageCaption(string imageCaption);
        bool ImageCaptionExist(string imageCaption);
        void ClearImageCaptions();

        void AddCustomMetadata(string key, string customMetadata);
        void RemoveCustomMetadata(string key);
        bool CustomMetadataExist(string key);
        void ClearCustomMetadatas();
    }
}
