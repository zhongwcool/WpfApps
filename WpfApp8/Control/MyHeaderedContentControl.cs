﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp8.Control;

[TemplatePart(Name = "PART_Content", Type = typeof(ContentPresenter))]
public class MyHeaderedContentControl : HeaderedContentControl
{
    private ContentPresenter _content;

    public static readonly DependencyProperty ClickToHideProperty =
        DependencyProperty.Register("ClickToHide", typeof(bool), typeof(MyHeaderedContentControl),
            new PropertyMetadata(null));

    public bool ClickToHide
    {
        get => (bool)GetValue(ClickToHideProperty);
        set => SetValue(ClickToHideProperty, value);
    }

    public static readonly DependencyProperty HideContentFromBootProperty =
        DependencyProperty.Register("HideContentFromBoot", typeof(bool), typeof(MyHeaderedContentControl),
            new PropertyMetadata(null));

    public bool HideContentFromBoot
    {
        get => (bool)GetValue(HideContentFromBootProperty);
        set => SetValue(HideContentFromBootProperty, value);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _content = GetTemplateChild("PART_Content") as ContentPresenter;
        if (null == _content) return;
        if (HideContentFromBoot) _content.Visibility = Visibility.Collapsed;
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);

        // Only a click on this control is handled, not children.
        if (_content is null || !e.Source.Equals(this))
            return;

        // If the child (Content) is visible, collapse it, if it is collapsed, make it visible.
        if (!ClickToHide) return;
        _content.Visibility = _content.Visibility == Visibility.Collapsed
            ? Visibility.Visible
            : Visibility.Collapsed;

        e.Handled = true;
    }
}