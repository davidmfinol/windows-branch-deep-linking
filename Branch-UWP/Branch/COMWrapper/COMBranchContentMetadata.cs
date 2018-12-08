using BranchSdk;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    public class COMBranchContentMetadata : ICOMBranchContentMetadata
    {
        public string ContentSchema { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string CurrencyType { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductCategory { get; set; }
        public string Condition { get; set; }
        public string ProductVariant { get; set; }
        public double Rating { get; set; }
        public double RatingAverage { get; set; }
        public int RatingCount { get; set; }
        public double RatingMax { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public string AddressRegion { get; set; }
        public string AddressCountry { get; set; }
        public string AddressPostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<string> ImageCaptions { get; set; }
        public Dictionary<string, string> CustomMetadata { get; set; }

        public COMBranchContentMetadata()
        {
            ImageCaptions = new List<string>();
            CustomMetadata = new Dictionary<string, string>();
        }

        public void AddCustomMetadata(string key, string customMetadata)
        {
            if (CustomMetadata.ContainsKey(key)) {
                CustomMetadata[key] = customMetadata;
            } else {
                CustomMetadata.Add(key, customMetadata);
            }
        }

        public void AddImageCaption(string imageCaption)
        {
            ImageCaptions.Add(imageCaption);
        }

        public void ClearCustomMetadatas()
        {
            CustomMetadata.Clear();
        }

        public void ClearImageCaptions()
        {
            ImageCaptions.Clear();
        }

        public bool CustomMetadataExist(string key)
        {
            return CustomMetadata.ContainsKey(key);
        }

        public bool ImageCaptionExist(string imageCaption)
        {
            return ImageCaptions.Contains(imageCaption);
        }

        public void RemoveCustomMetadata(string key)
        {
            if (CustomMetadata.ContainsKey(key)) {
                CustomMetadata.Remove(key);
            }
        }

        public void RemoveImageCaption(string imageCaption)
        {
            ImageCaptions.Remove(imageCaption);
        }

        public BranchContentMetadata ParseCOMMetadata()
        {
            BranchContentMetadata metadata = new BranchContentMetadata(ImageCaptions, CustomMetadata);

            if (Enum.TryParse(ContentSchema, out BranchContentSchema contentScheme)) {
                metadata.ContentSchema = contentScheme;
            }

            metadata.Quantity = Quantity;
            metadata.Price = Price;

            if (Enum.TryParse(CurrencyType, out BranchCurrencyType currencyType)) {
                metadata.CurrencyType = currencyType;
            }

            metadata.Sku = Sku;
            metadata.ProductName = ProductName;
            metadata.ProductBrand = ProductBrand;

            if (Enum.TryParse(ProductCategory, out BranchProductCategory productCategory)) {
                metadata.ProductCategory = productCategory;
            }

            metadata.ProductVariant = ProductVariant;
            metadata.Rating = Rating;
            metadata.RatingAverage = RatingAverage;
            metadata.RatingCount = RatingCount;
            metadata.RatingMax = RatingMax;
            metadata.AddressStreet = AddressStreet;
            metadata.AddressCity = AddressCity;
            metadata.AddressRegion = AddressRegion;
            metadata.AddressCountry = AddressCountry;
            metadata.AddressPostalCode = AddressPostalCode;
            metadata.Latitude = Latitude;
            metadata.Longitude = Longitude;

            return metadata;
        }
    }
}
