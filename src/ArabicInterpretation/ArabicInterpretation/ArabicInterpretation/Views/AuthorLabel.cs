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
    public class AuthorLabel : Button
    {
        bool isNT;
        int bookNumber;

        public AuthorLabel(bool isNT, int bookNumber)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;

            this.TextColor = Color.Blue;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            this.BorderWidth = 0;
            this.BackgroundColor = Color.Transparent;

            this.Clicked += async (sender, e) =>
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
                this.Text = Constants.AuthorNames.FrAntonios;
            }
            else
            {
                this.Text = Constants.AuthorNames.FrTadros;
            }
        }
    }
}
