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
    public class ChapterChooserPage : BasePage
    {
        AuthorLabel authorLabel;
        ScrollView scrollView;

        public ChapterChooserPage(
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel(isNT, bookNumber);
            layout.Children.Add(this.authorLabel);

            this.scrollView = new ScrollView
            {
                Content = new ChaptersGrid(isNT, bookNumber, chaptersCount)
            };

            layout.Children.Add(scrollView);

            this.Content = layout;
        }
    }
}
