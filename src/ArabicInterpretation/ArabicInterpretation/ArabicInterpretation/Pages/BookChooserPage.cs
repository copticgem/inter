using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class BookChooserPage : BasePage
    {
        bool isNT;
        BookChooser bookChooser;

        public BookChooserPage(
            bool isNT)
        {
            this.isNT = isNT;

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            // Add irrelevant bookNumber since no books are selected yet, so Authors shouldn't be disabled
            AuthorLabel authorLabel = new AuthorLabel(isNT, -1);
            layout.Children.Add(authorLabel);
            
            this.Content = layout;
        }

        protected override async void OnAppearing()
        {
            if (this.bookChooser == null)
            {
                this.bookChooser = new BookChooser();
                ((StackLayout)this.Content).Children.Add(this.bookChooser);

                await bookChooser.Initialize(isNT: this.isNT);
            }
        }
    }
}
