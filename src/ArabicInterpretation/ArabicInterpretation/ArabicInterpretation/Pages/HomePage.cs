using ArabicInterpretation.Helpers;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            string content = FileHelper.GetFile(Author.FrAntonios, true, 1, 3).Result;
            List<View> views = ContentFormatter.FormatContent(content);
            foreach (View view in views)
            {
                layout.Children.Add(view);
            }

            this.Content = new ScrollView
            {
                Content = layout,
            };
        }
    }
}
