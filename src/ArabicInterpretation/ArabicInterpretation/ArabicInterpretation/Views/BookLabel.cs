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
    public class BookLabel : Button
    {
        BookChooserPage bookChooserPage;

        public BookLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;

            this.BorderRadius = 1;
            this.BorderWidth = Constants.DefaultBorderWidth;
            this.BorderColor = ColorManager.Text.BookChapter;
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            this.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.bookChooserPage = new BookChooserPage();
        }

        public async Task Initialize(
            Author author,
            bool isNT,
            int bookNumber,
            string bookName)
        {
            this.Text = bookName;

            await this.bookChooserPage.Initialize(
                author,
                isNT,
                bookNumber);
        }

        public async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.bookChooserPage);
        }
    }
}
