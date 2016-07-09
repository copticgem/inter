using ArabicInterpretation.Helpers;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class ReadingPage : ContentPage
    {
        Author author;
        bool isNT;
        int bookNumber;
        int chapterNumber;

        public ReadingPage(
            Author author,
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            this.author = author;
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.chapterNumber = chapterNumber;
        }

        protected override async void OnAppearing()
        {
            string content = await FileHelper.GetFile(
                this.author, 
                this.isNT, 
                this.bookNumber, 
                this.chapterNumber);

            Dictionary<int, Label> verses;
            List<View> views = ContentFormatter.FormatContent(content, out verses);

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            foreach (View view in views)
            {
                layout.Children.Add(view);
            }

            ScrollView scrollView = new ScrollView
            {
                Content = layout,
            };

            this.Content = scrollView;
        }
    }
}
