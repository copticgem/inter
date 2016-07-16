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
        AuthorsGrid authorsGrid;

        public AuthorChooserPage()
            : base("الكاتب")
        {
            this.authorsGrid = new AuthorsGrid();

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            layout.Children.Add(authorsGrid);
            this.Content = layout;
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
        }
    }
}
