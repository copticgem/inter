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
    public class ChaptersGrid : Grid
    {
        public ChaptersGrid(
            Author author,
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            int buttonsPerRow = 5;

            this.ColumnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < buttonsPerRow; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            this.RowDefinitions = new RowDefinitionCollection();
            for (int i = 0; i < 20; i++)
            {
                this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            // Add the introduction button
            Button introButton = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Text = "مقدمة"
            };

            introButton.Clicked += async (sender, e) =>
            {
                await this.OnChapterClicked(
                    author: author,
                    isNT: isNT, 
                    bookNumber: bookNumber,
                    chapterNumber: 0);
            };

            this.Children.Add(introButton, buttonsPerRow - 1, 0);

            int left = buttonsPerRow - 1;
            for (int i = 1; i <= chaptersCount; i++)
            {
                Button button = new Button
                {
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                    Text = NumbersHelper.TranslateNumber(i),
                    
                };

                button.HeightRequest = button.Width;

                int chapterNumber = i;
                button.Clicked += async (sender, e) =>
                {
                    await this.OnChapterClicked(
                        author: author,
                        isNT: isNT,
                        bookNumber: bookNumber,
                        chapterNumber: chapterNumber);
                };

                int top = (i - 1) / buttonsPerRow;
                this.Children.Add(button, left, top + 1);

                left--;
                if (left == -1)
                {
                    left = buttonsPerRow - 1;
                }
            }
        }

        private async Task OnChapterClicked(
            Author author,
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            await this.Navigation.PushAsync(new ReadingPage(author, isNT, bookNumber, chapterNumber));
        }
    }
}
