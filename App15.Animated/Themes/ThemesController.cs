using System;
using System.Windows;

namespace App15.Animated.Themes
{
    public static class ThemesController
    {
        public enum ThemeTypes
        {
            AnimativeLight,
            AnimativeColourfulLight,
            AnimativeDark,
            AnimativeColourfulDark
        }

        public static ThemeTypes CurrentTheme { get; set; }

        private static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        private static void ChangeTheme(Uri uri)
        {
            ThemeDictionary = new ResourceDictionary() { Source = uri };
        }

        public static void SetTheme(ThemeTypes theme)
        {
            string themeName = null;
            CurrentTheme = theme;
            switch (theme)
            {
                case ThemeTypes.AnimativeDark:
                    themeName = "AnimativeDarkTheme";
                    break;
                case ThemeTypes.AnimativeLight:
                    themeName = "AnimativeLightTheme";
                    break;
                case ThemeTypes.AnimativeColourfulDark:
                    themeName = "AnimativeColourfulDarkTheme";
                    break;
                case ThemeTypes.AnimativeColourfulLight:
                    themeName = "AnimativeColourfulLightTheme";
                    break;
            }

            try
            {
                if (!string.IsNullOrEmpty(themeName))
                    ChangeTheme(new Uri($"Themes/{themeName}.xaml", UriKind.Relative));
            }
            catch
            {
            }
        }
    }
}