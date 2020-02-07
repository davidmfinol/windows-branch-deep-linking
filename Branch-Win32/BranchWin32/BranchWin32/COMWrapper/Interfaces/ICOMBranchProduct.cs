using System.Runtime.InteropServices;

namespace COMWrapper
{
    [ComVisible(true)]
    public interface ICOMBranchProduct
    {
        string Sku { get; set; }
        string Name { get; set; }
        double Price { get; set; }
        int Quantity { get; set; }
        string Brand { get; set; }
        string Variant { get; set; }
        string Category { get; set; }

        void SetSku(string sku);

        void SetName(string name);

        void SetPrice(double price);

        void SetQuantity(int quantity);

        void SetBrand(string brand);

        void SetVariant(string variant);

        void SetCategory(string category);
    }
}
