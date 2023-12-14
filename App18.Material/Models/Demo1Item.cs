using System;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace App18.Material.Models;

public class Demo1Item : ObservableObject
{
    public Demo1Item(string name, Type contentType, PackIconKind selectedIcon, PackIconKind unselectedIcon)
    {
        Name = name;
        _contentType = contentType;
        SelectedIcon = selectedIcon;
        UnselectedIcon = unselectedIcon;
    }

    public string Name { get; }

    public PackIconKind SelectedIcon { get; set; }
    public PackIconKind UnselectedIcon { get; set; }

    private object? _content;

    public object? Content => _content ??= CreateContent();

    private readonly Type _contentType;

    private object? CreateContent()
    {
        var content = Activator.CreateInstance(_contentType);

        return content;
    }
}