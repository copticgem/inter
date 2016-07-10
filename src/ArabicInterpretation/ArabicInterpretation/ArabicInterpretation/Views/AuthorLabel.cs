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
        AuthorChooserPage authorChooserPage;

        public AuthorLabel(Author author)
        {
            this.authorChooserPage = new AuthorChooserPage(currentAuthor: author);

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
        }

        public async Task OnClicked()
        {
            await this.Navigation.PushModalAsync(this.authorChooserPage);
        }

        public void UpdateText(Author author)
        {
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
