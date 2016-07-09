using ArabicInterpretation.Pages;
using Core;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class BooksGrid : Grid
    {
        bool isNT;
        Author author;

        public BooksGrid(Author author, bool isNT)
        {
            this.isNT = isNT;
            this.author = author;

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

        public async Task LoadBooks()
        {
            string index = await FileHelper.GetIndex(this.isNT);
            string[] books = index.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            int booksPerRow = 4;
            int left = 3;
            for (int i = 1; i <= books.Length; i++)
            {
                string[] tokens = books[i - 1].Split(':');

                int chaptersCount = int.Parse(tokens[2]);
                Button button = new Button
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    Text = tokens[0] + "\r\n\r\n(" + tokens[1] + ")"
                };

                button.HeightRequest = button.Width;

                int bookNumber = i;
                button.Clicked += async (sender, e) =>
                {
                    await this.OnBookClicked(this.author, this.isNT, bookNumber, chaptersCount);
                };

                int top = (i - 1) / booksPerRow;
                this.Children.Add(button, left, top);

                left--;
                if (left == -1)
                {
                    left = 3;
                }
            }
        }

        private async Task OnBookClicked(
            Author author,
            bool isNT, 
            int bookNumber,
            int chaptersCount)
        {
            await this.Navigation.PushAsync(new ChapterChooserPage(author, isNT, bookNumber, chaptersCount));
        }
    }
}
