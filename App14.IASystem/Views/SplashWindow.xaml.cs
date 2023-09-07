﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using App14.IASystem.Context;
using App14.IASystem.Enums;
using App14.IASystem.Models;

namespace App14.IASystem.Views;

public partial class SplashWindow : Window
{
    public SplashWindow()
    {
        InitializeComponent();
    }

    private async void CreateAndFill()
    {
        await Task.Run(() =>
        {
            using var context = new IaContext();
            // this is for demo purposes only, to make it easier
            // to get up and running
            context.Database.EnsureCreated();
            Print("创建成功");

            var farms = new[]
            {
                new Farm { Id = Guid.NewGuid(), Name = "阳澄湖1号", GongYang = 98, ShiFei = 90, TouWei = 30, XiaoDu = 50 }
            };

            var pools = new[]
            {
                new Pool
                {
                    PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 3, 25), Farm = farms[0], Name = "2号塘",
                    Size = 16.4f,
                    Note =
                        "高标准循环水池塘，主要养殖加州鲈、阳澄湖大闸蟹。\n水槽内高密度养殖加州鲈形成集约化，可视化设备、水质传感器、纳米增氧设备工作效率大大提高，劳动力投入大大减少。新型集污设备部署在水槽后半部分，提取出大量污染物，同时循环河道将养殖尾水带入净化区，净化区种植的水生蔬菜吸收养分净化水质，在净化区产出本地名品阳澄湖大闸蟹，个头饱满肉质鲜美。"
                },
                new Pool
                {
                    PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 3, 25), Farm = farms[0], Name = "5号塘",
                    Size = 12.7f,
                    Note =
                        "高标准循环水池塘，主要养殖加州鲈、阳澄湖大闸蟹。\n水槽内高密度养殖加州鲈形成集约化，可视化设备、水质传感器、纳米增氧设备工作效率大大提高，劳动力投入大大减少。新型集污设备为嵌入式部署，提取出大量污染物，同时循环河道将养殖尾水带入净化区，净化区种植的水草吸收养分净化水质，在净化区产出本地名品阳澄湖大闸蟹，个头饱满肉质鲜美。"
                },
                new Pool
                {
                    PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 8, 11), Farm = farms[0], Name = "9号塘",
                    Size = 17.9f,
                    Note =
                        "高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
                },
                new Pool
                {
                    PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 9, 12), Farm = farms[0], Name = "10号塘",
                    Size = 18.2f,
                    Note =
                        "高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
                },
                new Pool
                {
                    PoolId = Guid.NewGuid(), AddDate = new DateTime(2022, 10, 1), Farm = farms[0], Name = "11号塘",
                    Size = 17.5f,
                    Note =
                        "高标准循环水池塘，主要养殖阳澄湖大闸蟹。\n在传统的高标准蟹塘基础上改造，在蟹塘外侧的河道中部署推水车，形成水循环，种植菖蒲等植物净化水质，投放花白鲢调节藻类，为大闸蟹养殖区提供源源不断的优质水体。"
                }
            };

            var devices = new[]
            {
                new Device
                {
                    SerialNum = "J65430443", AddDate = new DateTime(2022, 4, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "水槽@2号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.11",
                    Firmware = "0", Protocol = "0", Pool = pools[0]
                },
                new Device
                {
                    SerialNum = "J90556302", AddDate = new DateTime(2022, 4, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "全景@2号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.10",
                    Firmware = "0", Protocol = "0", Pool = pools[0]
                },
                new Device
                {
                    SerialNum = "00000014", AddDate = new DateTime(2022, 4, 1), Type = ModelType.AFE,
                    ModelNum = "IAS-AFE-A1", NodeName = "投喂@2号塘", Mac = "44:17:93:d4:49:c8", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
                },
                new Device
                {
                    SerialNum = "00000017", AddDate = new DateTime(2022, 4, 1), Type = ModelType.CCB,
                    ModelNum = "IAS-CCB-A1", NodeName = "控制器-2@2号塘", Mac = "44:17:93:d4:1c:90", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
                },
                new Device
                {
                    SerialNum = "00000018", AddDate = new DateTime(2022, 4, 1), Type = ModelType.CCB,
                    ModelNum = "IAS-CCB-A1", NodeName = "控制器-1@2号塘", Mac = "44:17:93:d4:2e:84", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
                },
                new Device
                {
                    SerialNum = "0000001B", AddDate = new DateTime(2022, 4, 1), Type = ModelType.WQM,
                    ModelNum = "IAS-WQM-A1", NodeName = "水质@2号塘", Mac = "44:17:93:d4:4a:ec", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[0]
                },

                new Device
                {
                    SerialNum = "J65429053", AddDate = new DateTime(2022, 4, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "水槽@5号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.9",
                    Firmware = "0", Protocol = "0", Pool = pools[1]
                },
                new Device
                {
                    SerialNum = "J90556327", AddDate = new DateTime(2022, 4, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "全景@5号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.8",
                    Firmware = "0", Protocol = "0", Pool = pools[1]
                },
                new Device
                {
                    SerialNum = "00000015", AddDate = new DateTime(2022, 4, 1), Type = ModelType.AFE,
                    ModelNum = "IAS-AFE-A1", NodeName = "投喂@5号塘", Mac = "44:17:93:d4:4b:d8", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
                },
                new Device
                {
                    SerialNum = "00000019", AddDate = new DateTime(2022, 4, 1), Type = ModelType.CCB,
                    ModelNum = "IAS-CCB-A1", NodeName = "控制器-2@5号塘", Mac = "90:38:0c:3c:7f:8c", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
                },
                new Device
                {
                    SerialNum = "0000001A", AddDate = new DateTime(2022, 5, 1), Type = ModelType.CCB,
                    ModelNum = "IAS-CCB-A1", NodeName = "控制器-1@5号塘", Mac = "90:38:0c:3c:ea:c8", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
                },
                new Device
                {
                    SerialNum = "0000001C", AddDate = new DateTime(2022, 5, 1), Type = ModelType.WQM,
                    ModelNum = "IAS-WQM-A1", NodeName = "水质@5号塘", Mac = "90:38:0c:3e:c4:e4", TcpPort = 3333,
                    Firmware = "1.0", Protocol = "1.0", Pool = pools[1]
                },

                new Device
                {
                    SerialNum = "J65430447", AddDate = new DateTime(2022, 10, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "水槽@9号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.6",
                    Firmware = "0", Protocol = "0", Pool = pools[2]
                },
                new Device
                {
                    SerialNum = "J52537315", AddDate = new DateTime(2022, 10, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "全景@9号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.7",
                    Firmware = "0", Protocol = "0", Pool = pools[2]
                },

                new Device
                {
                    SerialNum = "J90869122", AddDate = new DateTime(2022, 8, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "水槽@10号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.5",
                    Firmware = "0", Protocol = "0", Pool = pools[3]
                },
                new Device
                {
                    SerialNum = "J65430117", AddDate = new DateTime(2022, 8, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "全景@10号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.4",
                    Firmware = "0", Protocol = "0", Pool = pools[3]
                },

                new Device
                {
                    SerialNum = "J99855527", AddDate = new DateTime(2022, 9, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "水槽@11号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.3",
                    Firmware = "0", Protocol = "0", Pool = pools[4]
                },
                new Device
                {
                    SerialNum = "J65425377", AddDate = new DateTime(2022, 9, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "全景@11号塘", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    HikIp = "166.55.0.2",
                    Firmware = "0", Protocol = "0", Pool = pools[4]
                },

                new Device
                {
                    SerialNum = "F0000005", AddDate = DateTime.Now, Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "Demo05", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    Firmware = "0", Protocol = "0"
                },
                new Device
                {
                    SerialNum = "F0000006", AddDate = DateTime.Now, Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "Demo06", Mac = "00:00:00:00:00:00", TcpPort = 8000,
                    Firmware = "0", Protocol = "0"
                },

                new Device
                {
                    SerialNum = "F0000001", AddDate = new DateTime(2023, 1, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "HIK-015", HikIp = "192.168.77.101", Mac = "00:00:00:00:00:00",
                    TcpPort = 8000, Firmware = "0", Protocol = "0"
                },
                new Device
                {
                    SerialNum = "F0000002", AddDate = new DateTime(2023, 1, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "HIK-016", HikIp = "192.168.77.105", Mac = "00:00:00:00:00:00",
                    TcpPort = 8000, Firmware = "0", Protocol = "0"
                },
                new Device
                {
                    SerialNum = "F0000003", AddDate = new DateTime(2023, 1, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "HIK-017", HikIp = "192.168.77.107", Mac = "00:00:00:00:00:00",
                    TcpPort = 8000, Firmware = "0", Protocol = "0"
                },
                new Device
                {
                    SerialNum = "F0000004", AddDate = new DateTime(2023, 1, 1), Type = ModelType.WC,
                    ModelNum = "IAS-WC-A2", NodeName = "HIK-018", HikIp = "192.168.77.102", Mac = "00:00:00:00:00:00",
                    TcpPort = 8000, Firmware = "0", Protocol = "0"
                }
            };

            var wqms = new[]
            {
                new RecWqm
                {
                    Id = Guid.NewGuid(), HtTemp = 25.3f, HtPh = 8.1f, HtDosat = 100.2f, HtDor = 95.0f, Pool = pools[0],
                    Device = devices[9], TimeStamp = DateTime.Now
                }
            };

            context.Farms.AddRange(farms);
            Print("插入基地数据");
            context.Pools.AddRange(pools);
            Print("插入鱼塘信息");
            context.Devices.AddRange(devices);
            Print("插入设备数据");
            context.RecWqms.AddRange(wqms);
            Print("插入设备记录");
            context.SaveChanges();
            Print("save data");
        });
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        File.Delete("ia-system.db"); // 删除文件
        var prefix = DateTime.Now.ToString("HH:mm:ss.fff");
        Block.Text += $"{prefix} 删除旧数据库文件\n";
        CreateAndFill();
    }

    private void Print(string message)
    {
        var prefix = DateTime.Now.ToString("HH:mm:ss.fff");
        Dispatcher.Invoke(() => { Block.Text += $"{prefix} {message}\n"; });
    }

    private void ButtonView_OnClick(object sender, RoutedEventArgs e)
    {
        var wind = new MainWindow();
        wind.Show();
    }
}