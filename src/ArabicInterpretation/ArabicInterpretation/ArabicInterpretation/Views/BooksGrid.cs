using ArabicInterpretation.Helpers;
using ArabicInterpretation.Pages;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using ArabicInterpretation.Model;

namespace ArabicInterpretation
{
    public class BooksGrid : Grid
    {
        bool isNT;

        // Index in the list must match index in ot.txt
        List<Button> buttons;

        public BooksGrid(bool isNT)
        {
            this.isNT = isNT;
            this.buttons = new List<Button>();

            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            };

            this.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            };

            // Listen to author changes to disable missing books
            MessagingCenter.Subscribe<AuthorsGrid, string>(this, "AuthorChanged", (sender, arg) =>
            {
                this.OnAuthorChanging(sender, arg);
            });
        }

        private void OnAuthorChanging(AuthorsGrid sender, string authorName)
        {
            // Get missing books
            Author currentAuthor = AuthorManager.GetCurrentAuthor();
            List<int> missingBooks = MissingBooksHelper.GetMissingBooks(currentAuthor, this.isNT);

            // Re-enable disabled buttons
            this.buttons
                .Where(b => !b.IsEnabled)
                .ForEach(b => b.IsEnabled = true);

            // Disable missing buttons
            missingBooks.ForEach(bookIndex =>
            {
                // Change index since list starts from 0 but files start from 1
                Button button = this.buttons.ElementAtOrDefault(bookIndex- 1);
                if (button != null)
                {
                    button.IsEnabled = false;
                }
            });
        }

        public async Task LoadBooks()
        {
            List<BookInfo> books = await BookNameManager.GetBookNames(this.isNT);

            int booksPerRow = 4;
            int left = 3;
            for (int i = 1; i <= books.Count; i++)
            {
                BookInfo book = books[i - 1];
                Button button = new Button
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    Text = book.Name  + "\r\n\r\n(" + book.ShortName + ")"
                };

                button.HeightRequest = button.Width;

                int bookNumber = i;
                button.Clicked += async (sender, e) =>
                {
                    await this.OnBookClicked(this.isNT, bookNumber, book.ChaptersCount);
                };

                int top = (i - 1) / booksPerRow;
                this.Children.Add(button, left, top);
                this.buttons.Add(button);

                left--;
                if (left == -1)
                {
                    left = 3;
                }
            }
        }

        private async Task OnBookClicked(
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            await this.Navigation.PushAsync(new ChapterChooserPage(isNT, bookNumber, chaptersCount));
        }
    }
}
