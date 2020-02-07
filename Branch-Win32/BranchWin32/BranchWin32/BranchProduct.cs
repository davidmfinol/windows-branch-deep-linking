using BranchSdk.Enum;
using System;
using System.Collections.Generic;

namespace BranchSdk
{
    public class BranchProduct
    {
        private string Sku;
        private string Name;
        private double Price;
        private int Quantity;
        private string Brand;
        private string Variant;
        private BranchProductCategory Category;

        public string GetSku()
        {
            return Sku;
        }

        public void SetSku(string sku)
        {
            Sku = sku;
        }

        public string GetName()
        {
            return Name;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public double GetPrice()
        {
            return Price;
        }

        public void SetPrice(double price)
        {
            Price = price;
        }

        public int GetQuantity()
        {
            return Quantity;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public string GetBrand()
        {
            return Brand;
        }

        public void SetBrand(string brand)
        {
            Brand = brand;
        }

        public string GetVariant()
        {
            return Variant;
        }

        public void SetVariant(string variant)
        {
            Variant = variant;
        }

        public BranchProductCategory GetCategory()
        {
            return Category;
        }

        public void SetCategory(BranchProductCategory category)
        {
            Category = category;
        }

        public BranchProduct()
        {
        }

        public BranchProduct(string sku, string name, double price, int quantity, string brand, string variant, BranchProductCategory category)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            Brand = brand;
            Variant = variant;
            Category = category;
        }

        public Dictionary<string, object> GetProductJSONObject()
        {
            Dictionary<string, object> jsonObject = new Dictionary<string, object>();
            try {
                if (string.IsNullOrEmpty(Sku)) {
                    jsonObject.Add("sku", Sku);
                }

                if (string.IsNullOrEmpty(Name)) {
                    jsonObject.Add("name", Name);
                }

                jsonObject.Add("price", Price);

                jsonObject.Add("quantity", Quantity);

                if (string.IsNullOrEmpty(Brand)) {
                    jsonObject.Add("brand", Brand);
                }

                if (string.IsNullOrEmpty(Variant)) {
                    jsonObject.Add("variant", Variant);
                }

                jsonObject.Add("category", Category.ToString());
            } catch (Exception e) {

            }
            return jsonObject;
        }
    }
}
