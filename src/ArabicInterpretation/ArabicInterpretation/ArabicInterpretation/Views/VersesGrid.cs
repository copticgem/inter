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
    public class VersesGrid : Grid
    {
        private const int ButtonsPerRow = 5;

        public VersesGrid()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            this.ColumnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < ButtonsPerRow; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            this.RowDefinitions = new RowDefinitionCollection();
            for (int i = 0; i < 20; i++)
            {
                this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }
        }

        public Task Initialize(Dictionary<int, Grid> verses)
        {
            this.Children.Clear(); 

            var orderedVerses = verses.OrderBy(k => k.Key);

            int left = ButtonsPerRow - 1;
            for (int i = 1; i <= orderedVerses.Count(); i++)
            {
                int verseNumber = orderedVerses.ElementAt(i - 1).Key;

                Button button = ColorManager.CreateButton();
                button.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
                button.Text = NumbersHelper.TranslateNumber(verseNumber);

                button.Clicked += async (sender, e) =>
                {
                    await SynchronizationHelper.ExecuteOnce(this.OnVerseClicked(verseNumber: verseNumber));
                };

                int top = (i - 1) / ButtonsPerRow;
                this.Children.Add(button, left, top);

                left--;
                if (left == -1)
                {
                    left = ButtonsPerRow - 1;
                }
            }

            return Task.FromResult(true);
        }

        private async Task OnVerseClicked(int verseNumber)
        {
            MessagingCenter.Send(this, ReadingPage.VerseChangedMessage, verseNumber);

            await PageTransition.PopModalAsync(true);
        }
    }
}
