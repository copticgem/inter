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
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            this.Clicked += async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };

            this.settingsPage = new SettingsPage();
        }

        public void Initialize()
        {
            this.settingsPage.Initialize();
        }

        public async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.settingsPage, true);
        }
    }
}
