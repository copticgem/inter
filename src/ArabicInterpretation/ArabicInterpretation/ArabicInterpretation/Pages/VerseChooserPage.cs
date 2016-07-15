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
        VersesGrid versesGrid;

        public VerseChooserPage()
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.versesGrid = new VersesGrid();
            ScrollView scrollView = new ScrollView
            {
                Content = this.versesGrid
            };

            layout.Children.Add(scrollView);

            this.Content = layout;
        }

        public async Task Initialize(
            ScrollView scrollView,
            Dictionary<int, Label> verses)
        {
            await this.versesGrid.Initialize(
                scrollView,
                verses);
        }
    }
}
