using ArabicInterpretation.Resx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class BookChooser : Grid
    {
        public BookChooser(bool isNT)
        {
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

            int booksPerRow = 4;
            int left = 3;
            for (int i = 1; i < 7; i++)
            {
                string bookName = Books.ResourceManager.GetString("Book" + i);
                string bookShortName = Books.ResourceManager.GetString("Book" + i + "Short");

                Button button = new Button
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    Text = bookName + "\r\n\r\n(" + bookShortName + ")"
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

        private List<string> bookNames = new List<string>
        {
            "",
            ""
        };
    }
}
