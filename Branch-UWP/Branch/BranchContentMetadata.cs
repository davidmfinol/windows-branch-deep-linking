using BranchSdk.Enum;
using COMWrapper;
using System.Collections.Generic;
using Windows.Data.Json;

namespace BranchSdk {
    public class BranchContentMetadata {
        public BranchContentSchema ContentSchema { get; set; }
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

        private List<string> imageCaptions;
        private Dictionary<string, string> customMetadata;

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

        public BranchContentMetadata(string json) {
            imageCaptions = new List<string>();
            customMetadata = new Dictionary<string, string>();

            LoadFromJson(json);
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

        public Dictionary<string, object> ConvertToDictionary()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            if (ContentSchema != BranchContentSchema.NONE) data.Add(BranchJsonKey.ContentSchema.GetKey(), (ContentSchema.ToString()));
            data.Add(BranchJsonKey.Quantity.GetKey(), (Quantity));
            data.Add(BranchJsonKey.Price.GetKey(), (Price));
            if (CurrencyType != BranchCurrencyType.NONE) data.Add(BranchJsonKey.PriceCurrency.GetKey(), (CurrencyType.ToString()));
            if (!string.IsNullOrEmpty(Sku)) data.Add(BranchJsonKey.SKU.GetKey(), (Sku));
            if (!string.IsNullOrEmpty(ProductName)) data.Add(BranchJsonKey.ProductName.GetKey(), (ProductName));
            if (!string.IsNullOrEmpty(ProductBrand)) data.Add(BranchJsonKey.ProductBrand.GetKey(), (ProductBrand));
            if (ProductCategory != BranchProductCategory.NONE) data.Add(BranchJsonKey.ProductCategory.GetKey(), (ProductCategory.ProductCategoryToString()));
            if (Condition != BranchCondition.NONE) data.Add(BranchJsonKey.Condition.GetKey(), (Condition.ToString()));
            if (!string.IsNullOrEmpty(ProductVariant)) data.Add(BranchJsonKey.ProductVariant.GetKey(), (ProductVariant));
            data.Add(BranchJsonKey.Rating.GetKey(), (Rating));
            data.Add(BranchJsonKey.RatingAverage.GetKey(), (RatingAverage));
            data.Add(BranchJsonKey.RatingCount.GetKey(), (RatingCount));
            data.Add(BranchJsonKey.RatingMax.GetKey(), (RatingMax));
            if (!string.IsNullOrEmpty(AddressStreet)) data.Add(BranchJsonKey.AddressStreet.GetKey(), (AddressStreet));
            if (!string.IsNullOrEmpty(AddressCity)) data.Add(BranchJsonKey.AddressCity.GetKey(), (AddressCity));
            if (!string.IsNullOrEmpty(AddressRegion)) data.Add(BranchJsonKey.AddressRegion.GetKey(), (AddressRegion));
            if (!string.IsNullOrEmpty(AddressCountry)) data.Add(BranchJsonKey.AddressCountry.GetKey(), (AddressCountry));
            if (!string.IsNullOrEmpty(AddressPostalCode)) data.Add(BranchJsonKey.AddressPostalCode.GetKey(), (AddressPostalCode));
            data.Add(BranchJsonKey.Latitude.GetKey(), (Latitude));
            data.Add(BranchJsonKey.Longitude.GetKey(), (Longitude));

            if (imageCaptions.Count > 0) {
                List<string> imageCaptionsList = new List<string>();
                foreach (string caption in imageCaptions) {
                    imageCaptionsList.Add(caption);
                }
                data.Add(BranchJsonKey.ImageCaptions.GetKey(), imageCaptionsList);
            }

            if (customMetadata.Count > 0) {
                foreach (string customDataKey in customMetadata.Keys) {
                    data.Add(string.Format("{0}{1}", "~cd_", customDataKey), (customMetadata[customDataKey]));
                }
            }

            return data;
        }

        public JsonObject ConvertToJson() {
            JsonObject metadataJson = new JsonObject();

            if (ContentSchema != BranchContentSchema.NONE) metadataJson.Add(BranchJsonKey.ContentSchema.GetKey(), JsonValue.CreateStringValue( ContentSchema.ToString()));
            metadataJson.Add(BranchJsonKey.Quantity.GetKey(), JsonValue.CreateNumberValue( Quantity));
            metadataJson.Add(BranchJsonKey.Price.GetKey(), JsonValue.CreateNumberValue( Price));
            if (CurrencyType != BranchCurrencyType.NONE) metadataJson.Add(BranchJsonKey.PriceCurrency.GetKey(), JsonValue.CreateStringValue( CurrencyType.ToString()));
            if (!string.IsNullOrEmpty(Sku)) metadataJson.Add(BranchJsonKey.SKU.GetKey(), JsonValue.CreateStringValue( Sku));
            if (!string.IsNullOrEmpty(ProductName)) metadataJson.Add(BranchJsonKey.ProductName.GetKey(), JsonValue.CreateStringValue(ProductName));
            if (!string.IsNullOrEmpty(ProductBrand)) metadataJson.Add(BranchJsonKey.ProductBrand.GetKey(), JsonValue.CreateStringValue(ProductBrand));
            if (ProductCategory != BranchProductCategory.NONE) metadataJson.Add(BranchJsonKey.ProductCategory.GetKey(), JsonValue.CreateStringValue(ProductCategory.ProductCategoryToString()));
            if (Condition != BranchCondition.NONE) metadataJson.Add(BranchJsonKey.Condition.GetKey(), JsonValue.CreateStringValue(Condition.ToString()));
            if (!string.IsNullOrEmpty(ProductVariant)) metadataJson.Add(BranchJsonKey.ProductVariant.GetKey(), JsonValue.CreateStringValue(ProductVariant));
            metadataJson.Add(BranchJsonKey.Rating.GetKey(), JsonValue.CreateNumberValue( Rating));
            metadataJson.Add(BranchJsonKey.RatingAverage.GetKey(), JsonValue.CreateNumberValue( RatingAverage));
            metadataJson.Add(BranchJsonKey.RatingCount.GetKey(), JsonValue.CreateNumberValue( RatingCount));
            metadataJson.Add(BranchJsonKey.RatingMax.GetKey(), JsonValue.CreateNumberValue( RatingMax));
            if (!string.IsNullOrEmpty(AddressStreet)) metadataJson.Add(BranchJsonKey.AddressStreet.GetKey(), JsonValue.CreateStringValue( AddressStreet));
            if (!string.IsNullOrEmpty(AddressCity)) metadataJson.Add(BranchJsonKey.AddressCity.GetKey(), JsonValue.CreateStringValue( AddressCity));
            if (!string.IsNullOrEmpty(AddressRegion)) metadataJson.Add(BranchJsonKey.AddressRegion.GetKey(), JsonValue.CreateStringValue( AddressRegion));
            if (!string.IsNullOrEmpty(AddressCountry)) metadataJson.Add(BranchJsonKey.AddressCountry.GetKey(), JsonValue.CreateStringValue( AddressCountry));
            if (!string.IsNullOrEmpty(AddressPostalCode)) metadataJson.Add(BranchJsonKey.AddressPostalCode.GetKey(), JsonValue.CreateStringValue( AddressPostalCode));
            metadataJson.Add(BranchJsonKey.Latitude.GetKey(), JsonValue.CreateNumberValue( Latitude));
            metadataJson.Add(BranchJsonKey.Longitude.GetKey(), JsonValue.CreateNumberValue( Longitude));

            if (imageCaptions.Count > 0) {
                List<string> imageCaptionsList = new List<string>();
                metadataJson.Add(BranchJsonKey.ImageCaptions.GetKey(), imageCaptionsList.SerializeListAsJson());
                foreach (string caption in imageCaptions) {
                    imageCaptionsList.Add(caption);
                }
            }

            if (customMetadata.Count > 0) {
                foreach (string customDataKey in customMetadata.Keys) {
                    metadataJson.Add(string.Format("{0}{1}", "~cd_", customDataKey), JsonValue.CreateStringValue(customMetadata[customDataKey]));
                }
            }

            return metadataJson;
        }

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            JsonObject jsonObject = JsonObject.Parse(json);

            if (jsonObject.ContainsKey(BranchJsonKey.ContentSchema.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ContentSchema.GetKey()].GetString())) {
                if(System.Enum.TryParse(jsonObject[BranchJsonKey.ContentSchema.GetKey()].GetString(), out BranchContentSchema contentSchema)) {
                    ContentSchema = contentSchema;
                }
            }
            if (jsonObject.ContainsKey(BranchJsonKey.PriceCurrency.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.PriceCurrency.GetKey()].GetString())) {
                if (System.Enum.TryParse(jsonObject[BranchJsonKey.PriceCurrency.GetKey()].GetString(), out BranchCurrencyType currencyType)) {
                    CurrencyType = currencyType;
                }
            }
            if (jsonObject.ContainsKey(BranchJsonKey.ProductCategory.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ProductCategory.GetKey()].GetString())) {
                if (System.Enum.TryParse(jsonObject[BranchJsonKey.ProductCategory.GetKey()].GetString(), out BranchProductCategory productCategory)) {
                    ProductCategory = productCategory;
                }
            }
            if (jsonObject.ContainsKey(BranchJsonKey.Condition.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.Condition.GetKey()].GetString())) {
                if (System.Enum.TryParse(jsonObject[BranchJsonKey.Condition.GetKey()].GetString(), out BranchCondition condition)) {
                    Condition = condition;
                }
            }

            if (jsonObject.ContainsKey(BranchJsonKey.Quantity.GetKey()))
                Quantity = jsonObject[BranchJsonKey.Quantity.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.Price.GetKey()))
                Price = jsonObject[BranchJsonKey.Price.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.SKU.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.SKU.GetKey()].GetString()))
                Sku = jsonObject[BranchJsonKey.SKU.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.ProductName.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ProductName.GetKey()].GetString()))
                ProductName = jsonObject[BranchJsonKey.ProductName.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.ProductBrand.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ProductBrand.GetKey()].GetString()))
                ProductBrand = jsonObject[BranchJsonKey.ProductBrand.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.ProductVariant.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.ProductVariant.GetKey()].GetString()))
                ProductVariant = jsonObject[BranchJsonKey.ProductVariant.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.Rating.GetKey()))
                Rating = jsonObject[BranchJsonKey.Rating.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.RatingAverage.GetKey()))
                RatingAverage = jsonObject[BranchJsonKey.RatingAverage.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.RatingCount.GetKey()))
                RatingCount = (int)jsonObject[BranchJsonKey.RatingCount.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.RatingMax.GetKey()))
                RatingMax = jsonObject[BranchJsonKey.RatingMax.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.AddressStreet.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.AddressStreet.GetKey()].GetString()))
                AddressStreet = jsonObject[BranchJsonKey.AddressStreet.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.AddressCity.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.AddressCity.GetKey()].GetString()))
                AddressCity = jsonObject[BranchJsonKey.AddressCity.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.AddressRegion.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.AddressRegion.GetKey()].GetString()))
                AddressRegion = jsonObject[BranchJsonKey.AddressRegion.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.AddressCountry.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.AddressCountry.GetKey()].GetString()))
                AddressCountry = jsonObject[BranchJsonKey.AddressCountry.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.AddressPostalCode.GetKey()) && !string.IsNullOrEmpty(jsonObject[BranchJsonKey.AddressPostalCode.GetKey()].GetString()))
                AddressPostalCode = jsonObject[BranchJsonKey.AddressPostalCode.GetKey()].GetString();
            if (jsonObject.ContainsKey(BranchJsonKey.Latitude.GetKey()))
                Latitude = jsonObject[BranchJsonKey.Latitude.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.Longitude.GetKey()))
                Longitude = jsonObject[BranchJsonKey.Longitude.GetKey()].GetNumber();
            if (jsonObject.ContainsKey(BranchJsonKey.Longitude.GetKey()))
                Longitude = jsonObject[BranchJsonKey.Longitude.GetKey()].GetNumber();

            if (jsonObject.ContainsKey(BranchJsonKey.ImageCaptions.GetKey()) && jsonObject[BranchJsonKey.ImageCaptions.GetKey()].GetObject().DeserializeObject<List<string>>() != null) {
                imageCaptions = jsonObject[BranchJsonKey.ImageCaptions.GetKey()].GetObject().DeserializeObject<List<string>>();
            }

            foreach (string key in jsonObject.Keys) {
                if (key.StartsWith("~cd_")) {
                    customMetadata.Add(key.Replace("~cd_", ""), jsonObject[key].GetString());
                }
            }
        }

        public ICOMBranchContentMetadata ParseNativeMetadata()
        {
            ICOMBranchContentMetadata comMetadata = new COMBranchContentMetadata();
            comMetadata.ImageCaptions = ImageCaptions;
            comMetadata.CustomMetadata = CustomMetadata;
            comMetadata.ContentSchema = ContentSchema.ToString();
            comMetadata.Quantity = Quantity;
            comMetadata.Price = Price;
            comMetadata.CurrencyType = CurrencyType.ToString();
            comMetadata.Sku = Sku;
            comMetadata.ProductName = ProductName;
            comMetadata.ProductBrand = ProductBrand;
            comMetadata.ProductCategory = ProductCategory.ToString();
            comMetadata.ProductVariant = ProductVariant;
            comMetadata.Rating = Rating;
            comMetadata.RatingAverage = RatingAverage;
            comMetadata.RatingCount = RatingCount;
            comMetadata.RatingMax = RatingMax;
            comMetadata.AddressStreet = AddressStreet;
            comMetadata.AddressCity = AddressCity;
            comMetadata.AddressRegion = AddressRegion;
            comMetadata.AddressCountry = AddressCountry;
            comMetadata.AddressPostalCode = AddressPostalCode;
            comMetadata.Latitude = Latitude;
            comMetadata.Longitude = Longitude;
            return comMetadata;
        }
    }
}
