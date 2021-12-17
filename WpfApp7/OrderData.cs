using System.Collections.Generic;
using System.Linq;

namespace WpfApp7;

public class OrderMaster
{
    public string OrderNo { get; set; }
    public string CustomerName { get; set; }
    public string ShipAddress { get; set; }
    public string Express { get; set; }
    public decimal Freight { get; set; }

    public readonly List<Item> ItemList = new();

    public decimal TotalPrice
    {
        get { return ItemList.Sum(o => o.UnitPrice * o.Number) + Freight; }
    }
}

public class Item
{
    public string Sku { get; set; }
    public string Spec { get; set; }
    public decimal Number { get; set; }
    public string Unit { get; set; }
    public decimal UnitPrice { get; set; }
    public string Description { get; set; }
}