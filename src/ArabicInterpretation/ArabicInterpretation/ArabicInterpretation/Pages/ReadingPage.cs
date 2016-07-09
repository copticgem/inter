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
    public class ReadingPage : ContentPage
    {
        Author author;
        bool isNT;
        int bookNumber;
        int chapterNumber;

        StackLayout layout;

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

            NavigationPage.SetHasNavigationBar(this, false);

            this.layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.layout.Children.Add(new AuthorLabel(author));

            this.Content = this.layout;
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

            ScrollView scrollView = new ScrollView
            {
                Content = chapterLayout,
            };

            this.layout.Children.Add(scrollView);
        }
    }
}
