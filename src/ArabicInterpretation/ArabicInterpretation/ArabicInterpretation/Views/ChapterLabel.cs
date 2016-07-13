using ArabicInterpretation.Helpers;
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
    public class ChapterLabel : Button
    {
        bool isNT;
        int bookNumber;
        int chapterNumber;
        ChapterChooserPage chapterChooserPage;

        public ChapterLabel(
            bool isNT, 
            int bookNumber,
            int chapterNumber,
            int chaptersCount)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.chapterNumber = chapterNumber;

            this.TextColor = Color.Blue;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            this.BorderWidth = 0;
            this.BackgroundColor = Color.Transparent;

            this.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.UpdateText();

            this.chapterChooserPage = new ChapterChooserPage(isNT, bookNumber, chaptersCount);
        }

        public async Task OnClicked()
        {
            // TODO: The modal will create new page, see if this has perf problems
            await this.Navigation.PushAsync(this.chapterChooserPage);
        }

        private void UpdateText()
        {
            if (this.chapterNumber == 0)
            {
                this.Text = "مقدمة";
            }
            else
            {
                this.Text = NumbersHelper.TranslateNumber(this.chapterNumber);
            }
        }
    }
}
