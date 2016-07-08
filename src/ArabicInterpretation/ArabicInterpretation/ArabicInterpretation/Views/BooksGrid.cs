using Core;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class BooksGrid : Grid
    {
        bool isNT;

        public BooksGrid(bool isNT)
        {
            this.isNT = isNT;

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

                Button button = new Button
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    Text = tokens[0] + "\r\n\r\n(" + tokens[1] + ")"
                };

                button.HeightRequest = button.Width;

                int top = (i - 1) / booksPerRow;
                this.Children.Add(button, left, top);

                left--;
                if (left == -1)
                {
                    left = 3;
                }
            }
        }
    }
}
