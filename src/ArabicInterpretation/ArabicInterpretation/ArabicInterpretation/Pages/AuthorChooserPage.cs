using ArabicInterpretation.Helpers;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class AuthorChooserPage : BasePage
    {
        StackLayout layout;

        AuthorsGrid authorsGrid;

        public AuthorChooserPage()
            : base("اختر الكاتب ")
        {
            this.Content = App.LoadingImage;

            this.authorsGrid = new AuthorsGrid();

            this.layout = new StackLayout
            {
                Padding = Constants.DefaultPadding,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            layout.Children.Add(authorsGrid);
        }

        public async Task Initialize(
            string messageTitle,
            Author currentAuthor,
            bool isNT,
            int bookNumber)
        {
            await this.authorsGrid.Initialize(
                messageTitle,
                currentAuthor,
                isNT,
                bookNumber);

            this.Content = this.layout;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.authorsGrid.Dispose();
        }
    }
}
