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

        FontSize currentFontSize;
        ReadingBackgroundColor currentBackgroundColor;

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
            this.currentFontSize = SettingsManager.GetFontSize();
            this.fontSizePicker.Initialize(currentFontSize.ToString());

            this.currentBackgroundColor = SettingsManager.GetBackgroundColor();
            this.backgroundColorPicker.Initialize(currentBackgroundColor.ToString());
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            FontSize newFontSize = SettingsManager.GetFontSize();
            ReadingBackgroundColor newBackgroundColor = SettingsManager.GetBackgroundColor();

            if (this.currentFontSize != newFontSize ||
                this.currentBackgroundColor != newBackgroundColor)
            {
                this.currentFontSize = newFontSize;
                this.currentBackgroundColor = newBackgroundColor;

                // Send message to reload readingPage
                // Show loading screen in reading page
                MessagingCenter.Send(App.Navigation, ReadingPage.ShowLoadingMessage);
                MessagingCenter.Send(this, ReadingPage.SettingsChangedMessage);
            }
        }
    }
}
