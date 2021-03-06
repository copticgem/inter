﻿using ArabicInterpretation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Helpers
{
    public static class SettingsManager
    {
        public static void UpdateSetting(string settingName, string settingValue)
        {
            Application.Current.Properties[settingName] = settingValue;
        }

        public static string GetSetting(string settingName)
        {
            if (Application.Current.Properties.ContainsKey(settingName))
            {
                return (string)Application.Current.Properties[settingName];
            }

            return null;
        }

        public static void Initialize()
        {
            if (!Application.Current.Properties.ContainsKey(Constants.Properties.FontSize))
            {
                Application.Current.Properties[Constants.Properties.FontSize] = FontSize.Medium.ToString();
            }

            if (!Application.Current.Properties.ContainsKey(Constants.Properties.BackgroundColor))
            {
                Application.Current.Properties[Constants.Properties.BackgroundColor] = ReadingBackgroundColor.White.ToString();
            }
        }

        public static FontSize GetFontSize()
        {
            FontSize fontSize;
            string size = SettingsManager.GetSetting(Constants.Properties.FontSize);
            if (!Enum.TryParse(size, out fontSize))
            {
                fontSize = FontSize.Medium;
            }

            return fontSize;
        }

        public static ReadingBackgroundColor GetBackgroundColor()
        {
            ReadingBackgroundColor backgroundColor;
            string color = SettingsManager.GetSetting(Constants.Properties.BackgroundColor);
            if (!Enum.TryParse(color, out backgroundColor))
            {
                backgroundColor = ReadingBackgroundColor.White;
            }

            return backgroundColor;
        }

        public static NamedSize ToNamedSize(FontSize fontSize)
        {
            switch (fontSize)
            {
                case FontSize.Small:
                    return NamedSize.Small;
                case FontSize.Medium:
                    return NamedSize.Medium;
                case FontSize.Large:
                    return NamedSize.Large;
                default:
                    return NamedSize.Medium;
            }
        }

        public static ReadingColor ToColor(ReadingBackgroundColor backgroundColor)
        {
            switch (backgroundColor)
            {
                case ReadingBackgroundColor.White:
                    return ColorManager.DefaultReadingColor;
                case ReadingBackgroundColor.Black:
                    return new ReadingColor(
                        textColor: Color.White, 
                        backgroundColor: Color.Black,
                        firstBarColor: Color.FromRgb(30, 30, 30),
                        secondBarColor: Color.FromRgb(20, 20, 20));
                case ReadingBackgroundColor.Sepia:
                    return new ReadingColor(
                        textColor: Color.FromRgb(95, 75, 50), 
                        backgroundColor: Color.FromRgb(251, 240, 217),
                        firstBarColor: Color.FromRgb(240, 225, 193),
                        secondBarColor: Color.FromRgb(242, 229, 203));
                default:
                    return ColorManager.DefaultReadingColor;
            }
        }
    }
}
