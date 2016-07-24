using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Pages;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    public class SettingsLabel : Button
    {
        SettingsPage settingsPage;

        public SettingsLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button));

            this.Text = "اعدادت";

            this.HorizontalOptions = LayoutOptions.Start;

            this.BorderWidth = 0;

            this.Clicked += async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };
        }

        public void Initialize(ReadingColor color)
        {
            this.BackgroundColor = color.SecondBarColor;
        }

        public async Task OnClicked()
        {
            // There is a bug in xamarin that picker gets disposed when modal is popped
            // so creating it each time here
            this.settingsPage = new SettingsPage();

            await PageTransition.PushModalAsync(this.settingsPage, true);

            this.settingsPage.Initialize();
        }
    }
}
