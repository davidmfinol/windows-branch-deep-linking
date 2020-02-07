using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace COMWrapper
{
    [ComVisible(true)]
    public interface ICOMBranchCommerceEvent
    {
        double Revenue { get; set; }
        string CurrencyType { get; set; }
        string TransactionID { get; set; }
        double Shipping { get; set; }
        double Tax { get; set; }
        string Coupon { get; set; }
        string Affiliation { get; set; }
        List<ICOMBranchProduct> Products { get; set; }

        void SetRevenue(double revenue);

        void SetCurrencyType(string currency);

        void SetTransactionID(string transactionID);

        void SetShipping(double shipping);

        void SetTax(double tax);

        void SetCoupon(string coupon);

        void SetAffiliation(string affiliation);

        void SetProducts(List<ICOMBranchProduct> products);

        void AddProduct(ICOMBranchProduct product);
    }
}
