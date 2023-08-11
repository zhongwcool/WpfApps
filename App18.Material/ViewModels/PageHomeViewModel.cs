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
    }

    public ObservableCollection<Email> EmailList { get; set; } = new();
}