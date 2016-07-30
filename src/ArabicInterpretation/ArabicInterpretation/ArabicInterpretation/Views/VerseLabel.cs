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
    public class VerseLabel : Button, IDisposable
    {
        EventHandler handler;

        Dictionary<int, Grid> verses;

        public VerseLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button));

            this.Text = "آية";

            this.HorizontalOptions = LayoutOptions.End;

            this.BorderWidth = 0;

            this.handler = async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.Clicked += this.handler;
        }

        public void Dispose()
        {
            this.Clicked -= this.handler;
        }

        public Task Initialize(
            Dictionary<int, Grid> verses,
            ReadingColor color)
        {
            this.verses = verses;
            this.BackgroundColor = color.SecondBarColor;
            return Task.FromResult(true);
        }

        public async Task OnClicked()
        {
            VerseChooserPage verseChooserPage = new VerseChooserPage();

            await PageTransition.PushModalAsync(verseChooserPage);

            await verseChooserPage.Initialize(verses);
        }
    }
}
