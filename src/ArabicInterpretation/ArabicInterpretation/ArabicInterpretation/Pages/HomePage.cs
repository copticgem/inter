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
        ScrollView scrollView;

        public HomePage()
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            string content = FileHelper.GetFile(Author.FrAntonios, true, 1, 2).Result;
            List<Label> labels = ContentFormatter.FormatContent(content);
            foreach (Label label in labels)
            {
                layout.Children.Add(label);
            }

            this.Content = new ScrollView
            {
                Content = layout,
            };
        }
    }
}
