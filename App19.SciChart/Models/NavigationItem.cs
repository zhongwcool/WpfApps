using System;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MaterialDesignThemes.Wpf;

namespace App19.SciChart00.Models;

public class NavigationItem : ObservableObject
{
    public NavigationItem(string name, Type contentType, PackIconKind selectedIcon, PackIconKind unselectedIcon)
    {
        Name = name;
        _contentType = contentType;
        SelectedIcon = selectedIcon;
        UnselectedIcon = unselectedIcon;
    }

    public string Name { get; }

    public PackIconKind SelectedIcon { get; set; }
    public PackIconKind UnselectedIcon { get; set; }
    private readonly int _notificationNumber = 0;

    public object? Notifications
    {
        get
        {
            if (_notificationNumber == 0) return null;
            return _notificationNumber < 100 ? _notificationNumber : "99+";
        }
    }

    private object? _content;

    public object? Content => _content ??= CreateContent();

    private readonly Type _contentType;

    private object? CreateContent()
    {
        var content = Activator.CreateInstance(_contentType);

        return content;
    }

    private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;

    public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
    {
        get => _horizontalScrollBarVisibilityRequirement;
        set => SetProperty(ref _horizontalScrollBarVisibilityRequirement, value);
    }

    private ScrollBarVisibility _verticalScrollBarVisibilityRequirement = ScrollBarVisibility.Auto;

    public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
    {
        get => _verticalScrollBarVisibilityRequirement;
        set => SetProperty(ref _verticalScrollBarVisibilityRequirement, value);
    }

    private Thickness _marginRequirement = new(0);

    public Thickness MarginRequirement
    {
        get => _marginRequirement;
        set => SetProperty(ref _marginRequirement, value);
    }
}