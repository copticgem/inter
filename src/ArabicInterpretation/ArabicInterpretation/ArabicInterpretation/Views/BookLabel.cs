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
    public class BookLabel : Button, IDisposable
    {
        EventHandler handler;

        Author author;
        bool isNT;
        int bookNumber;

        public BookLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.HorizontalOptions = LayoutOptions.EndAndExpand;

            this.BorderRadius = 1;
            this.BorderWidth = Constants.DefaultBorderWidth;
            this.BorderColor = ColorManager.Text.BookChapter;

            this.handler = async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };

            this.Clicked += this.handler;
        }

        public Task Initialize(
            Author author,
            bool isNT,
            int bookNumber,
            string bookName,
            ReadingColor color)
        {
            this.BackgroundColor = color.SecondBarColor;
            this.Text = bookName;
            this.author = author;
            this.bookNumber = bookNumber;
            this.isNT = isNT;

            return Task.FromResult(true);
        }

        public async Task OnClicked()
        {
            BookChooserPage bookChooserPage = new BookChooserPage();

            await PageTransition.PushModalAsync(bookChooserPage);

            await bookChooserPage.Initialize(
                author,
                isNT,
                bookNumber);
        }

        public void Dispose()
        {
            this.Clicked -= this.handler;
        }
    }
}
