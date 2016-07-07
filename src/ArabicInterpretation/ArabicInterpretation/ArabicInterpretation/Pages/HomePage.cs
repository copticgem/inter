using ArabicInterpretation.Helpers;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class HomePage : ContentPage
    {
        ScrollView scrollView;
        Dictionary<int, Label> verses;

        public HomePage()
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            string content = FileHelper.GetFile(Author.FrAntonios, true, 2, 13).Result;

            List<View> views = ContentFormatter.FormatContent(content, out verses);
            foreach (View view in views)
            {
                // layout.Children.Add(view);
            }

            scrollView = new ScrollView
            {
                // Content = new BookChooser(),
            };

            this.Content = new BookChooser();
        }

        private async Task OnFontButtonClicked(NamedSize newSize)
        {
            await scrollView.ScrollToAsync(verses[3], ScrollToPosition.MakeVisible, true);
        }
    }
}
