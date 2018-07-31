using BranchSdk.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BranchSdk {
    public class BranchContentMetadata {
        BranchContentSchema ContentSchema { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public BranchCurrencyType CurrencyType { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string ProductBrand { get; set; }
        public BranchProductCategory ProductCategory { get; set; }
        public BranchCondition Condition { get; set; }
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

        private readonly List<string> imageCaptions;
        private readonly Dictionary<string, string> customMetadata;

        public List<string> ImageCaptions {
            get {
                return imageCaptions;
            }
        }

        public Dictionary<string, string> CustomMetadata {
            get {
                return customMetadata;
            }
        }

        public BranchContentMetadata() {
            imageCaptions = new List<string>();
            customMetadata = new Dictionary<string, string>();
        }

        public BranchContentMetadata(List<string> imageCaptions, Dictionary<string, string> customMetadata) {
            this.imageCaptions = imageCaptions;
            this.customMetadata = customMetadata;
        }

        public BranchContentMetadata AddImageCaptions(params string[] captions) {
            this.imageCaptions.AddRange(captions);
            return this;
        }

        public BranchContentMetadata AddCustomMetadata(string key, string value) {
            customMetadata.Add(key, value);
            return this;
        }

        public BranchContentMetadata SetAddress(string street, string city, string region, string country, string postalCode) {
            this.AddressStreet = street;
            this.AddressCity = city;
            this.AddressRegion = region;
            this.AddressCountry = country;
            this.AddressPostalCode = postalCode;
            return this;
        }

        public BranchContentMetadata SetLocation(double latitude, double longitude) {
            this.Latitude = latitude;
            this.Longitude = longitude;
            return this;
        }

        public BranchContentMetadata SetRating(double rating, double averageRating, double maximumRating, int ratingCount) {
            this.Rating = rating;
            this.RatingAverage = averageRating;
            this.RatingMax = maximumRating;
            this.RatingCount = ratingCount;
            return this;
        }

        public BranchContentMetadata SetRating(double averageRating, double maximumRating, int ratingCount) {
            this.RatingAverage = averageRating;
            this.RatingMax = maximumRating;
            this.RatingCount = ratingCount;
            return this;
        }

        public BranchContentMetadata SetPrice(double price, BranchCurrencyType currency) {
            this.Price = price;
            this.CurrencyType = currency;
            return this;
        }

        public JObject ConvertToJson() {
            JObject metadataJson = new JObject();

            if (ContentSchema != BranchContentSchema.NONE) metadataJson.Add(BranchJsonKey.ContentSchema.GetKey(), ContentSchema.ToString());
            metadataJson.Add(BranchJsonKey.Quantity.GetKey(), Quantity);
            metadataJson.Add(BranchJsonKey.Price.GetKey(), Price);
            if (CurrencyType != BranchCurrencyType.NONE) metadataJson.Add(BranchJsonKey.PriceCurrency.GetKey(), CurrencyType.ToString());
            if (!string.IsNullOrEmpty(Sku)) metadataJson.Add(BranchJsonKey.SKU.GetKey(), Sku);
            if (!string.IsNullOrEmpty(ProductName)) metadataJson.Add(BranchJsonKey.ProductName.GetKey(), ProductName);
            if (!string.IsNullOrEmpty(ProductBrand)) metadataJson.Add(BranchJsonKey.ProductBrand.GetKey(), ProductBrand);
            if (ProductCategory != BranchProductCategory.NONE) metadataJson.Add(BranchJsonKey.ProductCategory.GetKey(), ProductCategory.ProductCategoryToString());
            if (Condition != BranchCondition.NONE) metadataJson.Add(BranchJsonKey.Condition.GetKey(), Condition.ToString());
            if (!string.IsNullOrEmpty(ProductVariant)) metadataJson.Add(BranchJsonKey.ProductVariant.GetKey(), ProductVariant);
            metadataJson.Add(BranchJsonKey.Rating.GetKey(), Rating);
            metadataJson.Add(BranchJsonKey.RatingAverage.GetKey(), RatingAverage);
            metadataJson.Add(BranchJsonKey.RatingCount.GetKey(), RatingCount);
            metadataJson.Add(BranchJsonKey.RatingMax.GetKey(), RatingMax);
            if (!string.IsNullOrEmpty(AddressStreet)) metadataJson.Add(BranchJsonKey.AddressStreet.GetKey(), AddressStreet);
            if (!string.IsNullOrEmpty(AddressCity)) metadataJson.Add(BranchJsonKey.AddressCity.GetKey(), AddressCity);
            if (!string.IsNullOrEmpty(AddressRegion)) metadataJson.Add(BranchJsonKey.AddressRegion.GetKey(), AddressRegion);
            if (!string.IsNullOrEmpty(AddressCountry)) metadataJson.Add(BranchJsonKey.AddressCountry.GetKey(), AddressCountry);
            if (!string.IsNullOrEmpty(AddressPostalCode)) metadataJson.Add(BranchJsonKey.AddressPostalCode.GetKey(), AddressPostalCode);
            metadataJson.Add(BranchJsonKey.Latitude.GetKey(), Latitude);
            metadataJson.Add(BranchJsonKey.Longitude.GetKey(), Longitude);

            if (imageCaptions.Count > 0) {
                List<string> imageCaptionsList = new List<string>();
                metadataJson.Add(new JProperty(BranchJsonKey.ImageCaptions.GetKey(), imageCaptionsList));
                foreach (string caption in imageCaptions) {
                    imageCaptionsList.Add(caption);
                }
            }

            if (customMetadata.Count > 0) {
                foreach (string customDataKey in customMetadata.Keys) {
                    metadataJson.Add(customDataKey, customMetadata[customDataKey]);
                }
            }

            return metadataJson;
        }

        public static BranchContentMetadata CreateFromJson(string json) {
            return JsonConvert.DeserializeObject<BranchContentMetadata>(json);
        }
    }
}
