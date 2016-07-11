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
    public class BookChooserPage : ContentPage
    {
        Author author;
        bool isNT;
        BookChooser bookChooser;

        public BookChooserPage(
            Author author,
            bool isNT)
        {
            this.author = author;
            this.isNT = isNT;

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            AuthorLabel authorLabel = new AuthorLabel(author);
            layout.Children.Add(authorLabel);
            
            this.Content = layout;
        }

        protected override async void OnAppearing()
        {
            if (this.bookChooser == null)
            {
                this.bookChooser = new BookChooser(this.author);
                ((StackLayout)this.Content).Children.Add(this.bookChooser);

                await bookChooser.Initialize(isNT: this.isNT);
            }
        }
    }
}
