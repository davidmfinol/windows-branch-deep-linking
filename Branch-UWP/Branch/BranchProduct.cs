using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

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

        public JsonObject GetProductJSONObject()
        {
            JsonObject jsonObject = new JsonObject();
            try {
                jsonObject.Add("sku", JsonValue.CreateStringValue(Sku));
                jsonObject.Add("name", JsonValue.CreateStringValue(Name));
                jsonObject.Add("price", JsonValue.CreateNumberValue(Price));
                jsonObject.Add("quantity", JsonValue.CreateNumberValue(Quantity));
                jsonObject.Add("brand", JsonValue.CreateStringValue(Brand));
                jsonObject.Add("variant", JsonValue.CreateStringValue(Variant));
                jsonObject.Add("category", JsonValue.CreateStringValue(Category.ToString()));
            } catch (Exception e) {

            }
            return jsonObject;
        }
    }
}
