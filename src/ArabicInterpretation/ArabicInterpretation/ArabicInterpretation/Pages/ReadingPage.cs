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

        AuthorLabel authorLabel;
        StackLayout chapterLayout;

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

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel(author);
            layout.Children.Add(this.authorLabel);

            this.chapterLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                HorizontalOptions = LayoutOptions.End,
            };

            ScrollView scrollView = new ScrollView
            {
                Content = this.chapterLayout,
            };

            layout.Children.Add(scrollView);

            this.Content = layout;

            // Listen to author changes
            MessagingCenter.Subscribe<AuthorsGrid, string>(this, "AuthorChanged", async (sender, arg) => {
                await this.OnAuthorChanging(sender, arg);
            });
        }

        private async Task OnAuthorChanging(AuthorsGrid sender, string authorName)
        {
            Author author;
            if (Enum.TryParse(authorName, out author))
            {
                if (this.author != author)
                {
                    this.author = author;

                    // Update label
                    this.authorLabel.UpdateText(author);

                    // Update content
                    await this.UpdateContent();
                }
            }
        }

        protected override async void OnAppearing()
        {
            if (!this.chapterLayout.Children.Any())
            {
                // No content is loaded yet
                await this.UpdateContent();
            }
        }

        private async Task UpdateContent()
        {
            // Update content
            string content = await FileHelper.GetFile(
                this.author,
                this.isNT,
                this.bookNumber,
                this.chapterNumber);

            Dictionary<int, Label> verses;
            List<View> views = ContentFormatter.FormatContent(content, out verses);

            this.chapterLayout.Children.Clear();
            foreach (View view in views)
            {
                this.chapterLayout.Children.Add(view);
            }
        }
    }
}
