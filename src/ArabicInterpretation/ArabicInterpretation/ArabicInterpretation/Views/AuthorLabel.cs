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
        Author currentAuthor; 

        public AuthorLabel(Author author)
        {
            this.currentAuthor = author;

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

            this.UpdateText(author);

            // Listen to author changes
            MessagingCenter.Subscribe<AuthorsGrid, string>(this, "AuthorChanged", (sender, arg) => {
                this.OnAuthorChanging(sender, arg);
            });
        }

        public async Task OnClicked()
        {
            // TODO: Avoid creating new page everytime
            await this.Navigation.PushModalAsync(new AuthorChooserPage(currentAuthor: this.currentAuthor));
        }

        private void OnAuthorChanging(AuthorsGrid sender, string authorName)
        {
            Author author;
            if (Enum.TryParse(authorName, out author))
            {
                this.UpdateText(author);
            }
        }

        private void UpdateText(Author author)
        {
            this.currentAuthor = author;
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
