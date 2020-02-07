using BranchSdk;
using BranchSdk.Enum;
using System;

namespace COMWrapper
{
    public class COMBranchProduct : ICOMBranchProduct
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public string Variant { get; set; }
        public string Category { get; set; }

        public COMBranchProduct()
        {
        }

        public COMBranchProduct(string sku, string name, double price, int quantity, string brand, string variant, string category)
        {
            Sku = sku;
            Name = name;
            Price = price;
            Quantity = quantity;
            Brand = brand;
            Variant = variant;
            Category = category;
        }

        public void SetBrand(string brand)
        {
            Brand = brand;
        }

        public void SetCategory(string category)
        {
            Category = category;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetPrice(double price)
        {
            Price = price;
        }

        public void SetQuantity(int quantity)
        {
            Quantity = quantity;
        }

        public void SetSku(string sku)
        {
            Sku = sku;
        }

        public void SetVariant(string variant)
        {
            Variant = variant;
        }

        public BranchProduct ParseProduct()
        {
            BranchProduct product = new BranchProduct();

            product.SetBrand(Brand);

            try {
                BranchProductCategory productCat = (BranchProductCategory)Enum.Parse(typeof(BranchCurrencyType), Category);
                product.SetCategory(productCat);
            } catch (Exception e) { }

            product.SetName(Name);
            product.SetPrice(Price);
            product.SetQuantity(Quantity);
            product.SetSku(Sku);
            product.SetVariant(Variant);

            return product;
        }
    }
}
