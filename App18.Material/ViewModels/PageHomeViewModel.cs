using System;
using System.Collections.ObjectModel;
using App18.Material.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App18.Material.ViewModels;

public class PageHomeViewModel : ObservableObject
{
    public PageHomeViewModel()
    {
        // 获取当前时间
        var currentTime = DateTime.Now;
        var timeAgo = new TimeSpan(0, 0, 20, 0); // 20分钟前的时间间隔
        // 计算过去的时间
        var pastTime = currentTime.Subtract(timeAgo);

        EmailList.Add(new Email
        {
            Name = "老强",
            Time = pastTime,
            Subject = "豆花鱼",
            Content =
                "最近忙吗？昨晚我去了你最爱的那家饭馆，点了他们的特色豆花鱼，吃着吃着就想你了。"
        });

        // 获取当前时间
        timeAgo = new TimeSpan(29, 0, 20, 0); // 20分钟前的时间间隔
        // 计算过去的时间
        pastTime = currentTime.Subtract(timeAgo);

        EmailList.Add(new Email
        {
            Name = "So Durl",
            Time = pastTime,
            Subject = "Dinner Club",
            Content =
                "I think it's time for us to finally try that new noodle shop downtown that doesn't use menusAnyone else have other suggestions for dinner club this week? I'm so intrigued by this idea of a noodle restaurant where no one gets to order for themselves - could be fun, or terrible, or both :)"
        });

        // 获取当前时间
        timeAgo = new TimeSpan(0, 1, 1, 0); // 20分钟前的时间间隔
        // 计算过去的时间
        pastTime = currentTime.Subtract(timeAgo);

        EmailList.Add(new Email
        {
            Name = "Lily MacDonald",
            Time = pastTime,
            Subject = "This food show is made for you",
            Content =
                "Ping-you'd love this new food show i started watching. It's produced by a Thai drummer who star..We’re updating the cards and ranking all the time, so check back regularly. At first, you might need to follow some people or star some repositories to get started",
            Link1 =
                "https://media.cnn.com/api/v1/images/stellar/prod/210127235319-03-how-to-stay-warm-this-winter-wellness-clothes.jpg?q=w_3000,h_2000,x_0,y_0,c_fill"
        });
    }

    public ObservableCollection<Email> EmailList { get; set; } = new();
}