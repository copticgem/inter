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
        AuthorChooserPage authorChooserPage;
        Button button;

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

            this.authorChooserPage = new AuthorChooserPage();
        }

        public async Task Initialize(
            string messageTitle,
            Author author,
            bool isNT, 
            int bookNumber,
            ReadingColor color)
        {
            this.BackgroundColor = color.FirstBarColor;
            this.button.BackgroundColor = color.FirstBarColor;

            await this.authorChooserPage.Initialize(
                messageTitle,
                author, 
                isNT, 
                bookNumber);

            this.UpdateText(author);
        }

        private async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.authorChooserPage);
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
