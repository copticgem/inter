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
        Author author;
        bool isNT;
        int selectedBook = -1;

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
        }

        public async Task Initialize(Author author, int selectedBook)
        {
            this.author = author;

            if (!this.buttons.Any())
            {
                // First time, load books
                await this.LoadBooks();
            }

            this.FormatButtons(author, selectedBook);
        }

        private async Task LoadBooks()
        {
            List<BookInfo> books = await BookNameManager.GetBookNames(this.isNT);

            int booksPerRow = 4;
            int left = 3;
            for (int i = 1; i <= books.Count; i++)
            {
                BookInfo book = books[i - 1];
                Button button = ColorManager.CreateButton();
                button.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
                button.Text = book.Name + "\r\n\r\n(" + book.ShortName + ")";

                button.HeightRequest = button.Width;

                int bookNumber = i;
                button.Clicked += async (sender, e) =>
                {
                    await this.OnBookClicked(bookNumber, book.ChaptersCount);
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

        private void FormatButtons(Author author, int bookNumber)
        {
            // Get missing books
            List<int> missingBooks = MissingBooksHelper.GetMissingBooks(author, this.isNT);

            // Re-enable disabled buttons
            this.buttons
                .Where(b => !b.IsEnabled)
                .ForEach(b => b.IsEnabled = true);

            // Disable missing buttons
            missingBooks.ForEach(bookIndex =>
            {
                // Change index since list starts from 0 but files start from 1
                Button button = this.buttons.ElementAtOrDefault(bookIndex - 1);
                if (button != null)
                {
                    button.IsEnabled = false;
                }
            });

            // Reset previously selected one if any
            if (this.selectedBook != -1)
            {
                this.buttons[this.selectedBook - 1].TextColor = ColorManager.Text.Default;
            }

            // Set selected book if any
            if (bookNumber != -1)
            {
                this.buttons[bookNumber - 1].TextColor = ColorManager.Text.SelectedButton;
            }

            this.selectedBook = bookNumber;
        }

        private async Task OnBookClicked(
            int bookNumber,
            int chaptersCount)
        {
            ChapterChooserPage chapterChooserPage = new ChapterChooserPage();
            await chapterChooserPage.Initialize(
                this.author,
                this.isNT,
                bookNumber,
                chaptersCount);

            await App.Navigation.PushModalAsync(
                page: chapterChooserPage,
                animated: false);
        }
    }
}
