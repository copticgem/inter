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
        bool isNT;
        int bookNumber;
        BookChooserPage bookChooserPage;

        public BookLabel(
            bool isNT, 
            int bookNumber,
            string bookName)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;

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

            this.Text = bookName;

            this.bookChooserPage = new BookChooserPage(isNT);
        }

        public async Task OnClicked()
        {
            // TODO: The modal will create new page, see if this has perf problems
            await this.Navigation.PushAsync(this.bookChooserPage);
        }
    }
}
