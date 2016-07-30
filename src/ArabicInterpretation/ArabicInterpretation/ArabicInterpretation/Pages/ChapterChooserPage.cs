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
        private const string AuthorChangedMessage = "ChapterChooserPageAuthorChanged";

        StackLayout layout;

        bool shouldPopTwice;
        Author author;
        bool isNT;
        int bookNumber;
        int chaptersCount;

        AuthorLabel authorLabel;
        ChaptersGrid chaptersGrid;

        public ChapterChooserPage()
            : base("اختر الاصحاح ")
        {
            this.layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel();
            layout.Children.Add(this.authorLabel);

            this.chaptersGrid = new ChaptersGrid();
            ScrollView scrollView = new ScrollView
            {
                Padding = Constants.DefaultPadding,
                Content = this.chaptersGrid
            };

            layout.Children.Add(scrollView);

            this.SetSubscriptions(true);

            this.Content = App.LoadingImage;
        }

        public async Task Initialize(
            bool shouldPopTwice,
            Author author,
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            this.shouldPopTwice = shouldPopTwice;
            this.author = author;
            this.isNT = isNT;
            this.bookNumber = bookNumber;
            this.chaptersCount = chaptersCount;

            await this.authorLabel.Initialize(
                ChapterChooserPage.AuthorChangedMessage,
                author,
                isNT,
                bookNumber,
                ColorManager.DefaultReadingColor);

            await this.chaptersGrid.Initialize(
                shouldPopTwice,
                author,
                isNT,
                bookNumber,
                chaptersCount);

            this.Content = this.layout;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.SetSubscriptions(false);
            this.authorLabel.Dispose();
            this.chaptersGrid.Dispose();
        }

        private void SetSubscriptions(bool isSubscribe)
        {
            if (isSubscribe)
            {
                // Listen to author changes
                MessagingCenter.Subscribe<AuthorsGrid, Author>(this, ChapterChooserPage.AuthorChangedMessage, async (sender, arg) =>
                {
                    if (this.author != arg)
                    {
                        this.author = arg;

                        await this.authorLabel.Initialize(
                            ChapterChooserPage.AuthorChangedMessage,
                            author,
                            isNT,
                            bookNumber,
                            ColorManager.DefaultReadingColor);

                        this.chaptersGrid.UpdateAuthor(this.author);
                    }
                });
            }
            else
            {
                MessagingCenter.Unsubscribe<AuthorsGrid, Author>(this, ChapterChooserPage.AuthorChangedMessage);
            }
        }
    }
}
