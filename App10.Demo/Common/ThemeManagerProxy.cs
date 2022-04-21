using System.ComponentModel;
using System.Windows.Media;
using App10.Demo.Data;
using App10.Demo.Helper;
using ModernWpf;

namespace App10.Demo.Common;

public class ThemeManagerProxy : BindableBase
{
    private ThemeManagerProxy()
    {
        DispatcherHelper.RunOnMainThread(() =>
        {
            DependencyPropertyDescriptor.FromProperty(ThemeManager.ApplicationThemeProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateApplicationTheme(); });

            DependencyPropertyDescriptor.FromProperty(ThemeManager.ActualApplicationThemeProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateActualApplicationTheme(); });

            DependencyPropertyDescriptor.FromProperty(ThemeManager.AccentColorProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateAccentColor(); });

            DependencyPropertyDescriptor.FromProperty(ThemeManager.ActualAccentColorProperty, typeof(ThemeManager))
                .AddValueChanged(ThemeManager.Current, delegate { UpdateActualAccentColor(); });

            UpdateApplicationTheme();
            UpdateActualApplicationTheme();
            UpdateAccentColor();
            UpdateActualAccentColor();
        });
    }

    private static readonly AppConfig Config = AppConfig.CreateInstance();
    public static ThemeManagerProxy Current { get; } = new ThemeManagerProxy();

    #region ApplicationTheme

    public ApplicationTheme? ApplicationTheme
    {
        get
        {
            var theme = Config.GetValue("AppTheme");
            _applicationTheme = theme switch
            {
                "Dark" => ModernWpf.ApplicationTheme.Dark,
                "Light" => ModernWpf.ApplicationTheme.Light,
                _ => null
            };

            return _applicationTheme;
        }
        set
        {
            Set(ref _applicationTheme, value);

            if (!_updatingApplicationTheme)
            {
                DispatcherHelper.RunOnMainThread(() => { ThemeManager.Current.ApplicationTheme = _applicationTheme; });
            }

            // 存入配置
            var theme = ThemeManager.Current.ApplicationTheme;
            if (null == theme)
            {
                Config.SetValue("AppTheme", "");
                return;
            }

            Config.SetValue("AppTheme", theme.ToString());
        }
    }

    private void UpdateApplicationTheme()
    {
        _updatingApplicationTheme = true;
        ApplicationTheme = ThemeManager.Current.ApplicationTheme;
        _updatingApplicationTheme = false;
    }

    private ApplicationTheme? _applicationTheme;
    private bool _updatingApplicationTheme;

    #endregion

    #region ActualApplicationTheme

    public ApplicationTheme ActualApplicationTheme
    {
        get => _actualApplicationTheme;
        private set => Set(ref _actualApplicationTheme, value);
    }

    private void UpdateActualApplicationTheme()
    {
        ActualApplicationTheme = ThemeManager.Current.ActualApplicationTheme;
    }

    private ApplicationTheme _actualApplicationTheme;

    #endregion

    #region AccentColor

    private Color? _accentColor;
    private bool _updatingAccentColor;

    public Color? AccentColor
    {
        get
        {
            var accent = Config.GetValue("AccentColor");
            if (!string.IsNullOrEmpty(accent) && accent.StartsWith("#"))
            {
                _accentColor = (Color)ColorConverter.ConvertFromString(accent)!;
            }
            else
            {
                _accentColor = null;
                Config.SetValue("AccentColor", "");
            }

            return _accentColor;
        }
        set
        {
            if (_accentColor != value)
            {
                // 存入配置文件
                if (null != value)
                {
                    Config.SetValue("AccentColor", value.ToString());
                }
                else
                {
                    Config.SetValue("AccentColor", "");
                }

                Set(ref _accentColor, value);

                if (!_updatingAccentColor)
                {
                    DispatcherHelper.RunOnMainThread(() => { ThemeManager.Current.AccentColor = _accentColor; });
                }
            }
        }
    }

    private void UpdateAccentColor()
    {
        _updatingAccentColor = true;
        AccentColor = ThemeManager.Current.AccentColor;
        _updatingAccentColor = false;
    }

    #endregion

    #region ActualAccentColor

    public Color ActualAccentColor
    {
        get => _actualAccentColor;
        private set => Set(ref _actualAccentColor, value);
    }

    private void UpdateActualAccentColor()
    {
        ActualAccentColor = ThemeManager.Current.ActualAccentColor;
    }

    private Color _actualAccentColor;

    #endregion
}