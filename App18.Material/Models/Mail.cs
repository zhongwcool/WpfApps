using System;
using System.Collections.Generic;
using System.Windows;
using App18.Material.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace App18.Material.Models;

public class Mail : ObservableObject
{
    public string From { get; set; }
    public List<string> ToWhom { get; set; }
    public DateTime Time { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Link1 { get; set; }

    private object? _content;

    public object? Content => _content ??= CreateContent();

    private object? CreateContent()
    {
        var content = Activator.CreateInstance(typeof(PageMail));
        if (content is FrameworkElement element) element.DataContext = this;
        return content;
    }

    private Thickness _marginRequirement = new(0);

    public Thickness MarginRequirement
    {
        get => _marginRequirement;
        set => SetProperty(ref _marginRequirement, value);
    }
}

public class MailMode
{
    public List<Mail> Mails { set; get; }
    public string Last_Update { set; get; }
}