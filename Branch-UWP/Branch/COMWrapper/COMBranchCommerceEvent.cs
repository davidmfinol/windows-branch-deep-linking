using BranchSdk;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    public class COMBranchCommerceEvent : ICOMBranchCommerceEvent
    {
        public double Revenue { get; set; }
        public string CurrencyType { get; set; }
        public string TransactionID { get; set; }
        public double Shipping { get; set; }
        public double Tax { get; set; }
        public string Coupon { get; set; }
        public string Affiliation { get; set; }
        public List<ICOMBranchProduct> Products { get; set; }

        public void AddProduct(ICOMBranchProduct product)
        {
            Products.Add(product);
        }

        public void SetAffiliation(string affiliation)
        {
            Affiliation = affiliation;
        }

        public void SetCoupon(string coupon)
        {
            Coupon = coupon;
        }

        public void SetCurrencyType(string currency)
        {
            CurrencyType = currency;
        }

        public void SetProducts(List<ICOMBranchProduct> products)
        {
            Products = products;
        }

        public void SetRevenue(double revenue)
        {
            Revenue = revenue;
        }

        public void SetShipping(double shipping)
        {
            Shipping = shipping;
        }

        public void SetTax(double tax)
        {
            Tax = tax;
        }

        public void SetTransactionID(string transactionID)
        {
            TransactionID = transactionID;
        }

        public BranchCommerceEvent ParseCommerceEvent()
        {
            BranchCommerceEvent commerceEvent = new BranchCommerceEvent();

            commerceEvent.SetAffiliation(Affiliation);
            commerceEvent.SetCoupon(Coupon);

            if (Enum.TryParse(CurrencyType, out BranchCurrencyType currencyType)) {
                commerceEvent.SetCurrencyType(currencyType);
            }

            List<BranchProduct> products = new List<BranchProduct>();
            foreach (ICOMBranchProduct product in Products) {
                products.Add((product as COMBranchProduct).ParseProduct());
            }

            commerceEvent.SetProducts(products);
            commerceEvent.SetRevenue(Revenue);
            commerceEvent.SetShipping(Shipping);
            commerceEvent.SetTax(Tax);
            commerceEvent.SetTransactionID(TransactionID);

            return commerceEvent;
        }
    }
}
