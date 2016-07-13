using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
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
        bool isNT;
        int bookNumber;
        int chapterNumber;

        AuthorLabel authorLabel;
        ScrollView scrollView;

        public ReadingPage(
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.chapterNumber = chapterNumber;

            NavigationPage.SetHasNavigationBar(this, false);

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            // TODO: Move to async method
            BookInfo bookInfo = BookNameManager.GetBookNames(isNT).Result[bookNumber -1];
            BookChapterLabel bookChapterLabel = new BookChapterLabel(
                isNT: isNT,
                bookNumber: bookNumber,
                chapterNumber: chapterNumber,
                bookInfo: bookInfo);

            layout.Children.Add(bookChapterLabel);

            this.authorLabel = new AuthorLabel(isNT, bookNumber);
            layout.Children.Add(this.authorLabel);
            
            this.scrollView = new ScrollView();
            layout.Children.Add(scrollView);

            this.Content = layout;

            // Listen to author changes
            MessagingCenter.Subscribe<AuthorsGrid, string>(this, "AuthorChanged", async (sender, arg) =>
            {
                await this.OnAuthorChanging(sender, arg);
            });
        }

        private async Task OnAuthorChanging(AuthorsGrid sender, string authorName)
        {
            // Update content
            await this.UpdateContent();
        }

        protected override async void OnAppearing()
        {
            if (this.scrollView.Content == null)
            {
                // No content is loaded yet
                await this.UpdateContent();
            }
        }

        private async Task UpdateContent()
        {
            // Update content
            Author currentAuthor = AuthorManager.GetCurrentAuthor();
            string content = await FileHelper.GetFile(
                currentAuthor,
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

            this.scrollView.Content = chapterLayout;

            View firstView = chapterLayout.Children.FirstOrDefault();
            if (firstView != null)
            {
                await this.scrollView.ScrollToAsync(firstView, ScrollToPosition.Start, false);
            }
        }
    }
}
