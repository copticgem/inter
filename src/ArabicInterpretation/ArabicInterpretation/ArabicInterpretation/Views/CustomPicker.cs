using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    class CustomPicker : StackLayout, IDisposable
    {
        string title;
        List<OptionItem> options;

        Button button;
        EventHandler buttonHandler;

        Picker picker;
        EventHandler pickerHandler;

        public CustomPicker(
            string title,
            List<OptionItem> options,
            string settingName)
        {
            this.title = title;
            this.options = options;

            // Picker
            this.picker = new Picker
            {
                IsVisible = false
            };

            options.ForEach(o => this.picker.Items.Add(o.DisplayName));

            this.pickerHandler = (sender, args) =>
            {
                if (picker.SelectedIndex != -1)
                {
                    string displayName = picker.Items[picker.SelectedIndex];
                    this.SetButtonText(displayName);

                    string optionName = options.Single(o => o.DisplayName.Equals(displayName)).OptionName;
                    SettingsManager.UpdateSetting(settingName, optionName);
                }
            };

            this.picker.SelectedIndexChanged += this.pickerHandler;

            // Button
            this.button = new Button
            {
                BackgroundColor = ColorManager.Backgrounds.Default,
                HorizontalOptions = LayoutOptions.End,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                TextColor = ColorManager.Text.Default
            };

            this.buttonHandler = (sender, e) =>
            {
                this.picker.Focus();
            };

            this.button.Clicked += this.buttonHandler;

            this.Children.Add(this.button);
            this.Children.Add(this.picker);
        }

        public void Initialize(string optionName)
        {
            string displayName = this.options.Single(o => o.OptionName.Equals(optionName)).DisplayName;
            this.SetButtonText(displayName);
            this.picker.SelectedIndex = this.picker.Items.IndexOf(displayName);
        }

        private void SetButtonText(string displayName)
        {
            this.button.Text = string.Format(
                CultureInfo.InvariantCulture,
                "{0} [ {1} ]",
                this.title,
                displayName);
        }

        public void Dispose()
        {
            this.picker.SelectedIndexChanged -= this.pickerHandler;
            this.button.Clicked -= this.buttonHandler;
        }
    }
}
