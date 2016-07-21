using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    class SettingsPage : BasePage
    {
        CustomPicker fontSizePicker;

        public SettingsPage()
            : base("الاعدادات ")
        {
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            // Font size
            List<OptionItem> fontSizes = new List<OptionItem>
            {
                new OptionItem(FontSize.Small.ToString(), "صغير"),
                new OptionItem(FontSize.Medium.ToString(), "متوسط"),
                new OptionItem(FontSize.Large.ToString(), "كبير"),
            };

            this.fontSizePicker = new CustomPicker(
                title: "حجم الخط",
                options: fontSizes,
                settingName: Constants.Properties.FontSize);

            this.Initialize();

            stackLayout.Children.Add(fontSizePicker);
            this.Content = stackLayout;
        }

        public void Initialize()
        {
            string defaultFontSize = SettingsManager.GetSetting(Constants.Properties.FontSize);
            this.fontSizePicker.Initialize(defaultFontSize);
        }
    }
}
