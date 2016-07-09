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
        AuthorChooserPage authorChooserPage;
        PageData data;

        public AuthorLabel(Author author)
        {
            this.data = new PageData();
            this.authorChooserPage = new AuthorChooserPage(data);

            this.TextColor = Color.Blue;

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.VerticalOptions = LayoutOptions.Start;

            this.BorderWidth = 0;
            this.BackgroundColor = Color.Transparent;

            this.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.currentAuthor = author;
            this.UpdateText();
        }

        public async Task OnClicked()
        {
            this.data.Data.Clear();
            await this.Navigation.PushModalAsync(this.authorChooserPage);

            // Read data saved from modal page
            Author author;
            string value;
            if (this.data.Data.TryGetValue(Constants.DataKeyNames.Author, out value) &&
                Enum.TryParse(value, out author))
            {
                if (this.currentAuthor != author)
                {
                    this.currentAuthor = author;
                    this.UpdateText();
                }
            }
        }

        private void UpdateText()
        {
            if (this.currentAuthor == Author.FrAntonios)
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
