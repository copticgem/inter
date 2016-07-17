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
    public class VerseLabel : Button
    {
        VerseChooserPage verseChooserPage;

        public VerseLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button));

            this.Text = "آية";

            this.HorizontalOptions = LayoutOptions.End;

            this.BorderWidth = 0;
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            this.Clicked += async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };

            this.verseChooserPage = new VerseChooserPage();
        }

        public async Task Initialize(Dictionary<int, Grid> verses)
        {
            await this.verseChooserPage.Initialize(verses);
        }

        public async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.verseChooserPage);
        }
    }
}
