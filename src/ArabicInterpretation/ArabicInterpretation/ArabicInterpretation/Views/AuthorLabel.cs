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
    public class AuthorLabel : StackLayout
    {
        bool isNT;
        int bookNumber;
        Button button;

        public AuthorLabel(bool isNT, int bookNumber)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.BackgroundColor = ColorManager.Backgrounds.AuthorBar;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            this.button = new Button();
            this.Children.Add(button);
            this.button.TextColor = ColorManager.Text.Author;
            this.button.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.button.BorderWidth = 0;
            this.button.BackgroundColor = Color.Transparent;

            this.button.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.UpdateText();

            // Listen to author changes
            MessagingCenter.Subscribe<AuthorsGrid, string>(this, "AuthorChanged", (sender, arg) => {
                this.OnAuthorChanging(sender, arg);
            });
        }

        public async Task OnClicked()
        {
            // TODO: Avoid creating new page everytime
            await this.Navigation.PushModalAsync(new AuthorChooserPage(this.isNT, this.bookNumber));
        }

        private void OnAuthorChanging(AuthorsGrid sender, string authorName)
        {
            this.UpdateText();
        }

        private void UpdateText()
        {
            Author author = AuthorManager.GetCurrentAuthor();
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
