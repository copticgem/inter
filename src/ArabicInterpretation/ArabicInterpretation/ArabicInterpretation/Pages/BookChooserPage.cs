using ArabicInterpretation.Model;
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
        private const string AuthorChangedMessage = "BookChooserPageAuthorChanged";

        StackLayout layout;

        Author author;
        bool isNT;
        int bookNumber;

        BookChooser bookChooser;
        AuthorLabel authorLabel;

        public BookChooserPage()
            : base("اختر السفر ")
        {
            this.layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel();
            layout.Children.Add(authorLabel);

            this.bookChooser = new BookChooser();
            layout.Children.Add(this.bookChooser);

            this.Content = App.LoadingImage;

            // Listen to author changes to disable missing books
            MessagingCenter.Subscribe<AuthorsGrid, Author>(this, BookChooserPage.AuthorChangedMessage, async (sender, arg) =>
            {
                if (this.author != arg)
                {
                    this.author = arg;

                    await this.Initialize(
                        author: arg,
                        isNT: this.isNT,
                        bookNumber: this.bookNumber);
                }
            });
        }

        public async Task Initialize(
            Author author,
            bool isNT,
            int bookNumber)
        {
            this.author = author;
            this.isNT = isNT;
            this.bookNumber = bookNumber;

            // BookChooser, you're allowed to choose any author
            await this.authorLabel.Initialize(
                BookChooserPage.AuthorChangedMessage,
                author,
                isNT,
                -1,
                ColorManager.DefaultReadingColor);

            await bookChooser.Initialize(
                author: author,
                isNT: isNT,
                currentBookNumber: bookNumber);

            this.Content = this.layout;
        }
    }
}
