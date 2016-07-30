using ArabicInterpretation.Model;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Helpers
{
    class ContentManager
    {
        TapGestureRecognizer tapGestureRecognizer;
        EventHandler tapHandler;

        Dictionary<int, Grid> verses;

        public Dictionary<int, Grid> Verses
        {
            get { return this.verses; }
        }

        public async Task<StackLayout> GetContent(
            ReadingColor color,
            ReadingInfo readingInfo,
            EventHandler tapHandler)
        {
            // Update content
            string content = await FileHelper.GetFile(
                readingInfo.Author,
                readingInfo.IsNT,
                readingInfo.BookNumber,
                readingInfo.ChapterNumber);

            List<View> views = ContentFormatter.FormatContent(content, color, out this.verses);

            StackLayout chapterLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            foreach (View view in views)
            {
                chapterLayout.Children.Add(view);
            }
            
            if (this.tapGestureRecognizer != null)
            {
                this.tapGestureRecognizer.Tapped -= this.tapHandler;
            }

            this.tapHandler = tapHandler;
            this.tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += this.tapHandler;

            chapterLayout.GestureRecognizers.Add(tapGestureRecognizer);

            return chapterLayout;
        }
    }
}
