using BranchSdk.Enum;
using BranchSdk.Utils;
using COMWrapper;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

            if (ContentSchema != BranchContentSchema.NONE) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentSchema), (ContentSchema.ToString()));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Quantity), (Quantity));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Price), (Price));
            if (CurrencyType != BranchCurrencyType.NONE) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.PriceCurrency), (CurrencyType.ToString()));
            if (!string.IsNullOrEmpty(Sku)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.SKU), (Sku));
            if (!string.IsNullOrEmpty(ProductName)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductName), (ProductName));
            if (!string.IsNullOrEmpty(ProductBrand)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductBrand), (ProductBrand));
            if (ProductCategory != BranchProductCategory.NONE) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductCategory), BranchEnumUtils.ProductCategoryToString(ProductCategory));
            if (Condition != BranchCondition.NONE) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Condition), (Condition.ToString()));
            if (!string.IsNullOrEmpty(ProductVariant)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductVariant), (ProductVariant));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Rating), (Rating));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingAverage), (RatingAverage));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingCount), (RatingCount));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingMax), (RatingMax));
            if (!string.IsNullOrEmpty(AddressStreet)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressStreet), (AddressStreet));
            if (!string.IsNullOrEmpty(AddressCity)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressCity), (AddressCity));
            if (!string.IsNullOrEmpty(AddressRegion)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressRegion), (AddressRegion));
            if (!string.IsNullOrEmpty(AddressCountry)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressCountry), (AddressCountry));
            if (!string.IsNullOrEmpty(AddressPostalCode)) data.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressPostalCode), (AddressPostalCode));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Latitude), (Latitude));
            data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Longitude), (Longitude));

            if (imageCaptions.Count > 0) {
                List<string> imageCaptionsList = new List<string>();
                foreach (string caption in imageCaptions) {
                    imageCaptionsList.Add(caption);
                }
                data.Add(BranchEnumUtils.GetKey(BranchJsonKey.ImageCaptions), imageCaptionsList);
            }

            if (customMetadata.Count > 0) {
                foreach (string customDataKey in customMetadata.Keys) {
                    data.Add(string.Format("{0}{1}", "~cd_", customDataKey), (customMetadata[customDataKey]));
                }
            }

            return data;
        }

        public Dictionary<string, object> ConvertToJson() {
            Dictionary<string, object> metadataJson = new Dictionary<string, object>();

            if (ContentSchema != BranchContentSchema.NONE) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentSchema), ContentSchema.ToString());
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Quantity), Quantity);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Price), Price);
            if (CurrencyType != BranchCurrencyType.NONE) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.PriceCurrency), CurrencyType.ToString());
            if (!string.IsNullOrEmpty(Sku)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.SKU), Sku);
            if (!string.IsNullOrEmpty(ProductName)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductName), ProductName);
            if (!string.IsNullOrEmpty(ProductBrand)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductBrand), ProductBrand);
            if (ProductCategory != BranchProductCategory.NONE) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductCategory), BranchEnumUtils.ProductCategoryToString(ProductCategory));
            if (Condition != BranchCondition.NONE) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Condition), Condition.ToString());
            if (!string.IsNullOrEmpty(ProductVariant)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ProductVariant), ProductVariant);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Rating), Rating);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingAverage), RatingAverage);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingCount), RatingCount);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.RatingMax), RatingMax);
            if (!string.IsNullOrEmpty(AddressStreet)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressStreet), AddressStreet);
            if (!string.IsNullOrEmpty(AddressCity)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressCity), AddressCity);
            if (!string.IsNullOrEmpty(AddressRegion)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressRegion), AddressRegion);
            if (!string.IsNullOrEmpty(AddressCountry)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressCountry), AddressCountry);
            if (!string.IsNullOrEmpty(AddressPostalCode)) metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.AddressPostalCode), AddressPostalCode);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Latitude), Latitude);
            metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.Longitude), Longitude);

            if (imageCaptions.Count > 0) {
                List<string> imageCaptionsList = new List<string>();
                metadataJson.Add(BranchEnumUtils.GetKey(BranchJsonKey.ImageCaptions), Json.Serialize(imageCaptionsList));
                foreach (string caption in imageCaptions) {
                    imageCaptionsList.Add(caption);
                }
            }

            if (customMetadata.Count > 0) {
                foreach (string customDataKey in customMetadata.Keys) {
                    metadataJson.Add(string.Format("{0}{1}", "~cd_", customDataKey), customMetadata[customDataKey]);
                }
            }

            return metadataJson;
        }

        private void LoadFromJson(string json) {
            if (string.IsNullOrEmpty(json))
                return;

            Dictionary<string, object> jsonObject = (Dictionary<string, object>)Json.Deserialize(json);

            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ContentSchema)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentSchema)])) {
                ContentSchema = EnumUtils.TryParse<BranchContentSchema>((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ContentSchema)]);
            }
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.PriceCurrency)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.PriceCurrency)])) {
                CurrencyType = EnumUtils.TryParse<BranchCurrencyType>((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.PriceCurrency)]);
            }
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ProductCategory)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductCategory)])) {
                ProductCategory = BranchEnumUtils.ParseToProductCategory((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductCategory)]);
            }
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Condition)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Condition)])) {
                Condition = EnumUtils.TryParse<BranchCondition>((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Condition)]);
            }

            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Quantity)))
                Console.WriteLine(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Quantity)]);
                Quantity = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Quantity)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Price)))
                Price = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Price)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.SKU)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.SKU)]))
                Sku = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.SKU)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ProductName)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductName)]))
                ProductName = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductName)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ProductBrand)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductBrand)]))
                ProductBrand = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductBrand)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ProductVariant)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductVariant)]))
                ProductVariant = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ProductVariant)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Rating)))
                Rating = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Rating)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.RatingAverage)))
                RatingAverage = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.RatingAverage)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.RatingCount)))
                RatingCount = Convert.ToInt32(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.RatingCount)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.RatingMax)))
                RatingMax = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.RatingMax)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.AddressStreet)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressStreet)]))
                AddressStreet = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressStreet)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.AddressCity)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressCity)]))
                AddressCity = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressCity)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.AddressRegion)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressRegion)]))
                AddressRegion = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressRegion)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.AddressCountry)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressCountry)]))
                AddressCountry = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressCountry)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.AddressPostalCode)) && !string.IsNullOrEmpty((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressPostalCode)]))
                AddressPostalCode = (string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.AddressPostalCode)];
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Latitude)))
                Latitude = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Latitude)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Longitude)))
                Longitude = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Longitude)]);
            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Longitude)))
                Longitude = Convert.ToDouble(jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.Longitude)]);

            if (jsonObject.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ImageCaptions)) && (List<string>)Json.Deserialize((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ImageCaptions)]) != null) {
                imageCaptions = (List<string>)Json.Deserialize((string)jsonObject[BranchEnumUtils.GetKey(BranchJsonKey.ImageCaptions)]);
            }

            foreach (string key in jsonObject.Keys) {
                if (key.StartsWith("~cd_")) {
                    customMetadata.Add(key.Replace("~cd_", ""), (string)jsonObject[key]);
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
