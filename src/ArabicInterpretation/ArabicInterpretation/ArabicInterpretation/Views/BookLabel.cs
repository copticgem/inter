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
    public class BookLabel : Button
    {
        bool isNT;
        int bookNumber;
        BookChooserPage bookChooserPage;
        List<string> ntBooks;
        List<string> otBooks;

        public BookLabel(
            bool isNT, 
            int bookNumber,
            List<string> ntBooks,
            List<string> otBooks)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.ntBooks = ntBooks;
            this.otBooks = otBooks;

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

            this.bookChooserPage = new BookChooserPage(isNT);
        }

        public async Task OnClicked()
        {
            // TODO: The modal will create new page, see if this has perf problems
            await this.Navigation.PushModalAsync(this.bookChooserPage);
        }

        private void UpdateText()
        {
            if (this.isNT)
            {
                this.Text = this.ntBooks[this.bookNumber - 1];
            }
            else
            {
                this.Text = this.otBooks[this.bookNumber - 1];
            }
        }
    }
}
