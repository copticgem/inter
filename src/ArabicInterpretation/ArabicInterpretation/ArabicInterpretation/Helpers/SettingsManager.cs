using ArabicInterpretation.Model;
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
    }
}
