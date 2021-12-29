namespace WpfApp7.Models;

public static class Dummy
{
    static Dummy()
    {
        OrderExample.ItemList.Add(new Item
        {
            Sku = "MBP1304",
            Spec = "黑色",
            Number = 1,
            Unit = "台",
            UnitPrice = 5000.00m,
            Description = "送你的"
        });
        OrderExample.ItemList.Add(new Item
        {
            Sku = "DDR1600",
            Spec = "",
            Number = 4,
            Unit = "盒",
            UnitPrice = 200.00m,
            Description = ""
        });
        OrderExample.ItemList.Add(new Item
        {
            Sku = "FAN68",
            Spec = "",
            Number = 2,
            Unit = "个",
            UnitPrice = 40.00m,
            Description = "fan fan"
        });
        OrderExample.ItemList.Add(new Item
        {
            Sku = "MANUALBOOK",
            Spec = "",
            Number = 1,
            Unit = "本",
            UnitPrice = 30.00m,
            Description = ""
        });
        OrderExample.ItemList.Add(new Item
        {
            Sku = "KK103-AF",
            Spec = "USB接口",
            Number = 1,
            Unit = "个",
            UnitPrice = 35.00m,
            Description = ""
        });
        OrderExample.ItemList.Add(new Item
        {
            Sku = "DISP-1200",
            Spec = "白色",
            Number = 2,
            Unit = "台",
            UnitPrice = 1600.00m,
            Description = ""
        });
    }

    public static readonly OrderMaster OrderExample = new()
    {
        OrderNo = "106587",
        CustomerName = "BPMF公司",
        ShipAddress = "外高桥AA大道XXX号B厂房X座",
        Express = "顺丰速运",
        Freight = 120.00m
    };
}