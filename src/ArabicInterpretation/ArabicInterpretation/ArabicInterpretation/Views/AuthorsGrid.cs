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
    public class AuthorsGrid : Grid
    {
        public AuthorsGrid(Author currentAuthor)
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            this.ColumnDefinitions = new ColumnDefinitionCollection();
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            this.RowDefinitions = new RowDefinitionCollection();
            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            Button frTadros = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Text = Constants.AuthorNames.FrTadros
            };

            frTadros.Clicked += async (sender, e) =>
            {
                await this.OnAuthorClicked(author: Author.FrTadros);
            };

            Button frAntonios = new Button
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                Text = Constants.AuthorNames.FrAntonios
            };

            frAntonios.Clicked += async (sender, e) =>
            {
                await this.OnAuthorClicked(author: Author.FrAntonios);
            };

            this.Children.Add(frAntonios, 0, 0);
            this.Children.Add(frTadros, 1, 0);

            if (currentAuthor == Author.FrAntonios)
            {
                frAntonios.IsEnabled = false;
            }
            else
            {
                frTadros.IsEnabled = false;
            }
        }

        private async Task OnAuthorClicked(Author author)
        {
            // Send message to ReadingPage to update content
            MessagingCenter.Send(this, "AuthorChanged", author.ToString());

            await this.Navigation.PopModalAsync(animated: true);
        }
    }
}
