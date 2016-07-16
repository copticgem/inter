using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class ReadingPage : BasePage
    {
        public const string ChapterChangedMessage = "ReadingPageChapterChanged";
        public const string VerseChangedMessage = "ReadingPageVerseChanged";
        private const string AuthorChangedMessage = "ReadingPageAuthorChanged";

        ReadingInfo readingInfo;

        AuthorLabel authorLabel;
        BookChapterLabel bookChapterLabel;
        ScrollView scrollView;
        Dictionary<int, Grid> verses;

        bool isFullScreen = true;

        public ReadingPage()
            : base("التفسير")
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel();
            layout.Children.Add(this.authorLabel);

            this.bookChapterLabel = new BookChapterLabel();
            layout.Children.Add(bookChapterLabel);

            this.scrollView = new ScrollView();
            layout.Children.Add(scrollView);
            
            this.Content = layout;

            // Listen to author changes intended to ReadingPage
            MessagingCenter.Subscribe<AuthorsGrid, Author>(this, AuthorChangedMessage, async (sender, arg) =>
            {
                await this.OnAuthorChanged(sender, arg);
            });

            // Listen to chapter changes
            MessagingCenter.Subscribe<ChaptersGrid, ReadingInfo>(this, ChapterChangedMessage, async (sender, arg) =>
            {
                await this.OnChapterChanged(sender, arg);
            });

            // Listen to verse changes
            MessagingCenter.Subscribe<VersesGrid, int>(this, VerseChangedMessage, async (sender, arg) =>
            {
                Grid verseMarker = this.verses[arg];
                await this.scrollView.ScrollToAsync(verseMarker, ScrollToPosition.Start, false);
            });
        }

        public async Task Initialize(ReadingInfo readingInfo, bool isFirstTime = false)
        {
            this.readingInfo = readingInfo;

            // This has internal cache
            List<BookInfo> booksInfo = await BookNameManager.GetBookNames(readingInfo.IsNT);
            BookInfo bookInfo = booksInfo[readingInfo.BookNumber - 1];

            await this.UpdateAuthorLabel();

            await this.UpdateContent(isFirstTime);

            await this.bookChapterLabel.Initialize(
                readingInfo,
                bookInfo,
                this.verses);

            this.isFullScreen = true;
            this.AdjustFullScreen();
        }

        private async Task UpdateAuthorLabel()
        {
            await this.authorLabel.Initialize(
                ReadingPage.AuthorChangedMessage,
                this.readingInfo.Author,
                this.readingInfo.IsNT,
                this.readingInfo.BookNumber);
        }

        private async Task OnChapterChanged(ChaptersGrid sender, ReadingInfo readingInfo)
        {
            if (this.readingInfo.Author != readingInfo.Author ||
                this.readingInfo.IsNT != readingInfo.IsNT ||
                this.readingInfo.BookNumber != readingInfo.BookNumber ||
                this.readingInfo.ChapterNumber != readingInfo.ChapterNumber)
            {
                await this.Initialize(readingInfo);
            }
        }

        private async Task OnAuthorChanged(AuthorsGrid sender, Author author)
        {
            if (this.readingInfo.Author != author)
            {
                this.readingInfo.Author = author;

                await this.Initialize(this.readingInfo);
            }
        }

        private async Task UpdateContent(bool isFirstTime)
        {
            // Update content
            string content = await FileHelper.GetFile(
                this.readingInfo.Author,
                this.readingInfo.IsNT,
                this.readingInfo.BookNumber,
                this.readingInfo.ChapterNumber);

            List<View> views = ContentFormatter.FormatContent(content, out this.verses);

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

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                this.isFullScreen = !this.isFullScreen;
                this.AdjustFullScreen();
            };

            chapterLayout.GestureRecognizers.Add(tapGestureRecognizer);

            View firstView = chapterLayout.Children.FirstOrDefault();

            // For some reason, when app starts, scrollToAsync hangs
            if (!isFirstTime && firstView != null)
            {
                await this.scrollView.ScrollToAsync(firstView, ScrollToPosition.Start, false);
            }
        }

        private void AdjustFullScreen()
        {
            if (!this.isFullScreen)
            {
                NavigationPage.SetHasNavigationBar(this, true);
                this.authorLabel.IsVisible = true;
                this.bookChapterLabel.IsVisible = true;
            }
            else
            {
                this.authorLabel.IsVisible = false;
                this.bookChapterLabel.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, false);
            }
        }
    }
}
