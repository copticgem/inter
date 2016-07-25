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
    public class AuthorLabel : StackLayout
    {
        Button button;
        string messageTitle;
        Author author;
        bool isNT;
        int bookNumber;

        public AuthorLabel()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            this.button = new Button();
            this.Children.Add(button);
            this.button.TextColor = ColorManager.Text.Author;
            this.button.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.button.BorderWidth = 0;

            this.button.Clicked += async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };

        }

        public Task Initialize(
            string messageTitle,
            Author author,
            bool isNT, 
            int bookNumber,
            ReadingColor color)
        {
            this.messageTitle = messageTitle;
            this.author = author;
            this.isNT = isNT;
            this.bookNumber = bookNumber;

            this.BackgroundColor = color.FirstBarColor;
            this.button.BackgroundColor = color.FirstBarColor;
            this.UpdateText(author);

            return Task.FromResult(true);
        }

        private async Task OnClicked()
        {
            AuthorChooserPage authorChooserPage = new AuthorChooserPage();

            await PageTransition.PushModalAsync(authorChooserPage);

            await authorChooserPage.Initialize(
                this.messageTitle,
                this.author,
                this.isNT,
                this.bookNumber);
        }

        private void UpdateText(Author author)
        {
            if (author == Author.FrAntonios)
            {
                this.button.Text = Constants.AuthorNames.FrAntonios;
            }
            else
            {
                this.button.Text = Constants.AuthorNames.FrTadros;
            }
        }
    }
}
