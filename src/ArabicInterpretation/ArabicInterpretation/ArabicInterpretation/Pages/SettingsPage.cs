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
        CustomPicker backgroundColorPicker;

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
                title: "حجم النص",
                options: fontSizes,
                settingName: Constants.Properties.FontSize);

            // Background color
            List<OptionItem> backgroundColors = new List<OptionItem>
            {
                new OptionItem(ReadingBackgroundColor.White.ToString(), "ابيض"),
                new OptionItem(ReadingBackgroundColor.Black.ToString(), "اسود"),
                new OptionItem(ReadingBackgroundColor.Sepia.ToString(), "بيج"),
            };

            this.backgroundColorPicker = new CustomPicker(
                title: "خلفية النص",
                options: backgroundColors,
                settingName: Constants.Properties.BackgroundColor);

            stackLayout.Children.Add(this.fontSizePicker);
            stackLayout.Children.Add(this.backgroundColorPicker);
            this.Content = stackLayout;
        }

        public void Initialize()
        {
            string currentFontSize = SettingsManager.GetSetting(Constants.Properties.FontSize);
            this.fontSizePicker.Initialize(currentFontSize);

            string currentBackgroundColor = SettingsManager.GetSetting(Constants.Properties.BackgroundColor);
            this.backgroundColorPicker.Initialize(currentBackgroundColor);
        }
    }
}
