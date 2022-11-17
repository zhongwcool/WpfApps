using System;
using System.ComponentModel;
using System.Windows;
using App14.IASystem.Context;
using App14.IASystem.Models;
using App14.IASystem.ViewModels;

namespace App14.IASystem.Views;

public partial class MainWindow : Window
{
    private IaContext Context { get; }

    public MainWindow()
    {
        InitializeComponent();

        Context = new IaContext();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // this is for demo purposes only, to make it easier
        // to get up and running
        Context.Database.EnsureCreated();
        // uncomment if you want to fill database with default values
        // Fill();

        // bind to the source
        PoolsTab.DataContext = new PoolsTabViewModel(Context);
        DevicesTab.DataContext = new DevicesTabViewModel(Context);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        // clean up database connections
        Context.Dispose();
        base.OnClosing(e);
    }

    private void Fill()
    {
        var farms = new[]
        {
            new Farm { Id = Guid.NewGuid(), Name = "阳澄湖1号", GongYang = 98, ShiFei = 90, TouWei = 30, XiaoDu = 50 },
        };

        var pools = new[]
        {
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 1, 1), Farm = farms[0], Name = "产业园",
                Note = "预计2022年9月15号开始捕捞，预计产量3吨。此为定量养殖，注意控制品质。"
            },
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 3, 25), Farm = farms[0], Name = "2号塘",
                Note =
                    "16.4亩\n高标准循环水池塘，主要养殖加州鲈、阳澄湖大闸蟹。\n水槽内高密度养殖加州鲈形成集约化，可视化设备、水质传感器、纳米增氧设备工作效率大大提高，劳动力投入大大减少。新型集污设备部署在水槽后半部分，提取出大量污染物，同时循环河道将养殖尾水带入净化区，净化区种植的水生蔬菜吸收养分净化水质，在净化区产出本地名品阳澄湖大闸蟹，个头饱满肉质鲜美。"
            },
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 3, 25), Farm = farms[0], Name = "5号塘",
                Note =
                    "12.7亩\n高标准循环水池塘，主要养殖加州鲈、阳澄湖大闸蟹。\n水槽内高密度养殖加州鲈形成集约化，可视化设备、水质传感器、纳米增氧设备工作效率大大提高，劳动力投入大大减少。新型集污设备为嵌入式部署，提取出大量污染物，同时循环河道将养殖尾水带入净化区，净化区种植的水草吸收养分净化水质，在净化区产出本地名品阳澄湖大闸蟹，个头饱满肉质鲜美。"
            },
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 8, 11), Farm = farms[0], Name = "9号塘",
                Note =
                    "17.9亩\n高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
            },
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 9, 12), Farm = farms[0], Name = "10号塘",
                Note =
                    "18.2亩\n高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
            },
            new Pool
            {
                PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 10, 1), Farm = farms[0], Name = "11号塘",
                Note =
                    "17.5亩\n高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
            }
        };

        var devices = new[]
        {
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 1, 1), SerialNum = "F0000001", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "HIK-015", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "0.1", Pool = pools[0]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 1, 1), SerialNum = "F0000002", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "HIK-016", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 1, 1), SerialNum = "F0000003", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "HIK-017", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 1, 1), SerialNum = "F0000004", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "HIK-018", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "J65430443", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "水槽@2号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "J90556302", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "全景@2号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "00000014", Type = ModelType.AFE,
                ModelNum = "IAS-AFE-A1", NodeName = "投喂@2号塘", Mac = "44:17:93:d4:49:c8", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "00000017", Type = ModelType.CCB,
                ModelNum = "IAS-CCB-A1", NodeName = "控制器-2@2号塘", Mac = "44:17:93:d4:1c:90", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "00000018", Type = ModelType.CCB,
                ModelNum = "IAS-CCB-A1", NodeName = "控制器-1@2号塘", Mac = "44:17:93:d4:2e:84", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "0000001B", Type = ModelType.WQM,
                ModelNum = "IAS-WQM-A1", NodeName = "水质@2号塘", Mac = "44:17:93:d4:4a:ec", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "J65429053", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "水槽@5号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "J90556327", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "全景@5号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "00000015", Type = ModelType.AFE,
                ModelNum = "IAS-AFE-A1", NodeName = "投喂@5号塘", Mac = "44:17:93:d4:4b:d8", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 4, 1), SerialNum = "00000019", Type = ModelType.CCB,
                ModelNum = "IAS-CCB-A1", NodeName = "控制器-2@5号塘", Mac = "90:38:0c:3c:7f:8c", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 5, 1), SerialNum = "0000001A", Type = ModelType.CCB,
                ModelNum = "IAS-CCB-A1", NodeName = "控制器-1@5号塘", Mac = "90:38:0c:3c:ea:c8", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 5, 1), SerialNum = "0000001C", Type = ModelType.WQM,
                ModelNum = "IAS-WQM-A1", NodeName = "水质@5号塘", Mac = "90:38:0c:3e:c4:e4", TcpPort = 3333,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[2]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 10, 1), SerialNum = "J65430447", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "水槽@9号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[3]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 10, 1), SerialNum = "J52537315", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "全景@9号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[3]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 8, 1), SerialNum = "J90869122", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "水槽@10号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[4]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 8, 1), SerialNum = "J65430117", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "全景@10号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[4]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 9, 1), SerialNum = "J99855527", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "水槽@11号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[5]
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = new DateTime(2022, 9, 1), SerialNum = "J65425377", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "全景@11号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0", Pool = pools[5]
            },

            new Device
            {
                Id = Guid.NewGuid(), AddDate = DateTime.Now, SerialNum = "F0000005", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "Demo05", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0"
            },
            new Device
            {
                Id = Guid.NewGuid(), AddDate = DateTime.Now, SerialNum = "F0000006", Type = ModelType.WC,
                ModelNum = "IAS-WC-A2", NodeName = "Demo06", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                Firmware = "1.0", Protocol = "1.0"
            },
        };

        Context.Farms.AddRange(farms);
        Context.Pools.AddRange(pools);
        Context.Devices.AddRange(devices);
        Context.SaveChanges();
    }

    // to avoid exception "某个 ItemsControl 与它的项源不一致"
    // {
    private void PoolsTab_OnSelected(object sender, RoutedEventArgs e)
    {
        DataGridPools.Items.Refresh();
        DataGridSelectedPool.Items.Refresh();
    }

    private void DevicesTab_OnSelected(object sender, RoutedEventArgs e)
    {
        DataGridDevices.Items.Refresh();
    }
    // }
}