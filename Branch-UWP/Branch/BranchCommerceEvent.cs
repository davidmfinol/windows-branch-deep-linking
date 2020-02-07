using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace BranchSdk
{
    public class BranchCommerceEvent
    {
        private double Revenue;
        private BranchCurrencyType CurrencyType;
        private string TransactionID;
        private double Shipping;
        private double Tax;
        private string Coupon;
        private string Affiliation;
        private List<BranchProduct> Products;

        public double GetRevenue()
        {
            return Revenue;
        }

        public void SetRevenue(double revenue)
        {
            this.Revenue = revenue;
        }

        public BranchCurrencyType GetCurrencyType()
        {
            return CurrencyType;
        }

        public void SetCurrencyType(BranchCurrencyType currency)
        {
            this.CurrencyType = currency;
        }

        public string GetTransactionID()
        {
            return TransactionID;
        }

        public void SetTransactionID(string transactionID)
        {
            this.TransactionID = transactionID;
        }

        public double GetShipping()
        {
            return Shipping;
        }

        public void SetShipping(double shipping)
        {
            this.Shipping = shipping;
        }

        public double GetTax()
        {
            return Tax;
        }

        public void SetTax(double tax)
        {
            this.Tax = tax;
        }

        public string GetCoupon()
        {
            return Coupon;
        }

        public void SetCoupon(string coupon)
        {
            this.Coupon = coupon;
        }

        public string GetAffiliation()
        {
            return Affiliation;
        }

        public void SetAffiliation(string affiliation)
        {
            this.Affiliation = affiliation;
        }

        public void SetProducts(List<BranchProduct> products)
        {
            this.Products = products;
        }

        public void AddProduct(BranchProduct product)
        {
            if (this.Products == null) {
                this.Products = new List<BranchProduct>();
            }
            this.Products.Add(product);
        }

        public BranchCommerceEvent()
        {
        }

        public BranchCommerceEvent(double revenue, BranchCurrencyType currency, string transactionID, double shipping, double tax, string coupon, String affiliation, List<BranchProduct> products)
        {
            this.Revenue = revenue;
            this.CurrencyType = currency;
            this.TransactionID = transactionID;
            this.Shipping = shipping;
            this.Tax = tax;
            this.Coupon = coupon;
            this.Affiliation = affiliation;
            this.Products = products;
        }

        public BranchCommerceEvent(double revenue, BranchCurrencyType currency, string transactionID, double shipping, double tax, string coupon, String affiliation, BranchProduct product)
        {
            this.Revenue = revenue;
            this.CurrencyType = currency;
            this.TransactionID = transactionID;
            this.Shipping = shipping;
            this.Tax = tax;
            this.Coupon = coupon;
            this.Affiliation = affiliation;
            this.Products = new List<BranchProduct>();
            this.Products.Add(product);
        }

        public JsonObject GetCommerceJSONObject()
        {
            JsonObject jsonObject = new JsonObject();
            JsonArray jsonProducts = new JsonArray();

            foreach (JsonObject json in GetProducts()) {
                jsonProducts.Add(json);
            }

            try {
                jsonObject.Add("revenue", JsonValue.CreateNumberValue(Revenue));
                jsonObject.Add("currency", JsonValue.CreateStringValue(CurrencyType.ToString()));
                jsonObject.Add("transaction_id", JsonValue.CreateStringValue(TransactionID));
                jsonObject.Add("shipping", JsonValue.CreateNumberValue(Shipping));
                jsonObject.Add("tax", JsonValue.CreateNumberValue(Tax));
                jsonObject.Add("coupon", JsonValue.CreateStringValue(Coupon));
                jsonObject.Add("affiliation", JsonValue.CreateStringValue(Affiliation));
                if (GetProducts() != null) jsonObject.Add("products", jsonProducts);
            } catch (Exception e) {

            }

            return jsonObject;
        }

        public List<JsonObject> GetProducts()
        {
            if (this.Products == null) {
                return null;
            }
            List<JsonObject> products = new List<JsonObject>();
            foreach (BranchProduct product in this.Products) {
                products.Add(product.GetProductJSONObject());
            }
            return products;
        }
    }
}
