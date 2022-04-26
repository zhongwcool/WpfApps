using System;

namespace App08.Metro.Models;

public class NotiModel
{
    public string Title { get; set; }
    public string ImageLocation { get; set; }
    public string Description { get; set; }
    public string Date { get; set; }

    public static NotiModel CreateDummy()
    {
        var rand = new Random();
        var index0 = rand.Next(0, 9);
        var index1 = rand.Next(0, 9);
        return new NotiModel()
        {
            Title = Foo[index0].Title,
            Description = Foo[index0].Description,
            ImageLocation = Bar[index1],
            Date = DateTime.Now.ToString("HH:mm:ss tt zz"),
        };
    }

    private static readonly string[] Bar =
    {
        "https://img-blog.csdnimg.cn/20190323161418695.png",
        "https://img-blog.csdnimg.cn/20190323161159133.png",
        "https://img-blog.csdnimg.cn/20190323161242873.png",
        "https://img-blog.csdnimg.cn/20190323161221635.png",
        "https://img-blog.csdnimg.cn/20190323161318683.png",
        "https://img-blog.csdnimg.cn/20190323161332979.png",
        "https://img-blog.csdnimg.cn/20190323161354536.png",
        "https://img-blog.csdnimg.cn/20190323161418695.png",
        "https://img-blog.csdnimg.cn/20190323161438219.png",
        "https://img-blog.csdnimg.cn/20190323161535761.png",
    };

    private static readonly Article[] Foo =
    {
        new()
        {
            Title = "20分钟详解 - 俄罗斯经济到底是怎么回事儿？",
            Description =
                "久等了，更新来啦！小Lin连夜肝稿，来解析一下错综复杂的俄罗斯经济到底是怎么回事儿~\n我的创业公司：Offer帮 - 求职学习平台\nhttps://offerbang.io?wpm=2.3.17.2",
        },
        new()
        {
            Title = "WPF C# Professional Modern Flat UI Tutorial",
            Description =
                "WPF C# Professional Modern Flat UI Tutorial this tutorial will show you how to create a flat modern ui  with a flat design using WPF and C# this goes really fast and it's stunning and beautiful and professional and works well for 2021 and 2022. This is a tutorial and the source code and the project files will be available for download. A simple, minimalistic futuristic look. \nPatreon: https://www.patreon.com/payloads ",
        },
        new()
        {
            Title = "华为 MatePad Paper 深度体验 ：这就是墨水屏天花板？",
            Description = "这，是买前生产力买后爱奇艺的平板，而这，是海鲜市场里被评为十大年度无用产品的墨水屏电纸书。啪的一下很快啊，合二为一了，能改变吃灰的命运吗？",
        },
        new()
        {
            Title = "最新｜澤連斯基：相互妥協才能與普京直接對話；烏方：超過1.2萬烏克蘭人回國加入國防軍；馬里烏波爾已被俄軍隔離；鳳凰記者前線直擊",
            Description =
                "俄軍宣佈7日開啟人道主義走廊\n烏軍稱奪回東部城市楚胡伊夫\n俄:烏境內發現美國資助軍事生物計劃\n撤離俄外交人員飛機離開美國\n美卿:北約國家獲准向烏提供戰機",
        },
        new()
        {
            Title = "Acer传奇X体验，一台重量堪比轻薄本的全能型笔记本",
            Description = "谢谢您的观看!期待您点赞订阅",
        },
        new()
        {
            Title = "给手机装Windows11！还能玩大型游戏？！",
            Description =
                "今天我们要给一台一加6T手机装上Windows11，不是远程桌面，不是虚拟机，是原生运行的Windows11哦！不仅如此，我们还要在上面尝试各种大型游戏，这颗845能坚持住吗？",
        },
        new()
        {
            Title = "5分钟速读《阿西莫夫最新科学指南》",
            Description =
                "泛读科普经典《阿西莫夫最新科学指南》丨一部跨越138亿年的史诗\n阿西莫夫不仅是个著名的科幻小说家，还是一位优秀的科普作者：他的《科学指南》系列在1960-1984年间更新了四版，成为了一代人的科学启蒙。就让我们跟随阿西莫夫的脚步，从138亿年前最初的起点开始，回顾这段波澜壮阔的史诗。",
        },
        new()
        {
            Title =
                "Delicate music, calms the nervous system and pleases the soul - healing music for the heart and...",
            Description =
                "Gentle music, calms the nervous system and pleases the soul - healing music for the heart and blood vessels\n Music for relaxation, meditation, study, reading, massage, spa or sleep. This music is ideal for dealing with anxiety, stress or insomnia as it promotes relaxation and helps eliminate bad vibes. They can also use this music as a background for meditation or relaxing in their sleep.",
        },
        new()
        {
            Title = "Steam the pork belly in a pot, it is nutritious and delicious, the method is very simple",
            Description =
                "Like my video, remember to subscribe, like, comment, share, thank you for your support",
        },
        new()
        {
            Title = "非常好聽👍2小時【放松心情的钢琴曲】早上最適合聽的輕音樂放鬆解壓 - 美妙的音樂 -純鋼琴輕音樂 - 輕音樂 睡覺 -放鬆音-Relaxing Piano",
            Description =
                "Thanks for watching this video, don't forget to like it, subscribe to my channel if you like it.\n感谢观看这个视频，不要忘记喜欢，订阅我的频道，如果你喜欢它。",
        },
    };

    private class Article
    {
        public string Title;
        public string Description;
    }
}