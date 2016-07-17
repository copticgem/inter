using ArabicInterpretation.Helpers;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    public class AuthorsGrid : Grid
    {
        // AuthorsGrid can be called from different components, this message will send result to calling one
        string messageTitle;

        Button frTadros;
        Button frAntonios;

        public AuthorsGrid()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            this.ColumnDefinitions = new ColumnDefinitionCollection();
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            this.RowDefinitions = new RowDefinitionCollection();
            this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            this.frTadros = ColorManager.CreateButton();
            frTadros.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            frTadros.Text = Constants.AuthorNames.FrTadros;
            frTadros.Clicked += async (sender, e) =>
            {
                await this.OnAuthorClicked_Safe(author: Author.FrTadros);
            };

            this.frAntonios = ColorManager.CreateButton();
            frAntonios.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            frAntonios.Text = Constants.AuthorNames.FrAntonios;
            frAntonios.Clicked += async (sender, e) =>
            {
                await this.OnAuthorClicked_Safe(author: Author.FrAntonios);
            };

            this.Children.Add(frAntonios, 0, 0);
            this.Children.Add(frTadros, 1, 0);
        }

        public Task Initialize(
            string messageTitle,
            Author currentAuthor,
            bool isNT,
            int bookNumber)
        {
            this.messageTitle = messageTitle;

            this.SetAuthorVisibility(frAntonios, Author.FrAntonios, currentAuthor, isNT, bookNumber);
            this.SetAuthorVisibility(frTadros, Author.FrTadros, currentAuthor, isNT, bookNumber);

            return Task.FromResult(true);
        }

        private void SetAuthorVisibility(
            Button button,
            Author buttonAuthor,
            Author currentAuthor,
            bool isNT,
            int bookNumber)
        {
            if (currentAuthor == buttonAuthor)
            {
                // Author is already selected, disable it
                ColorManager.DisableButton(button);
                return;
            }
            else
            {
                // Enable
                ColorManager.EnableButton(button);
            }

            List<int> missingBooks = MissingBooksHelper.GetMissingBooks(buttonAuthor, isNT);
            if (missingBooks.Contains(bookNumber))
            {
                // Current book is missing from that Author so disable the choice
                ColorManager.DisableButton(button);
            }
        }

        private async Task OnAuthorClicked_Safe(Author author)
        {
            await SynchronizationHelper.ExecuteOnce(this.OnAuthorClicked(author));
        }

        private async Task OnAuthorClicked(Author author)
        {
            // Send message to caller to update content
            MessagingCenter.Send(this, this.messageTitle, author);

            await PageTransition.PopModalAsync(animated: true);
        }
    }
}
