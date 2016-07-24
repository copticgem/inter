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
    public class VerseChooserPage : BasePage
    {
        StackLayout layout;

        VersesGrid versesGrid;

        public VerseChooserPage()
            :base("اختر الآية ")
        {
            this.Content = App.LoadingImage;

            this.layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                Padding = Constants.DefaultPadding
            };

            this.versesGrid = new VersesGrid();
            ScrollView scrollView = new ScrollView
            {
                Content = this.versesGrid
            };

            layout.Children.Add(scrollView);
        }

        public async Task Initialize(Dictionary<int, Grid> verses)
        {
            await this.versesGrid.Initialize(verses);

            this.Content = layout;
        }
    }
}
