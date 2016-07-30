using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public const string AuthorChangedMessage = "ReadingPageAuthorChanged";
        public const string SettingsChangedMessage = "ReadingPageSettingsChanged";
        public const string ShowLoadingMessage = "ReadingPageShowLoading";

        StackLayout layout;

        ReadingInfo readingInfo;

        AuthorLabel authorLabel;
        BookChapterLabel bookChapterLabel;

        ContentManager contentManager;
        EventHandler tapEventHandler;

        ScrollView scrollView;
        double scrollX;
        double scrollY;

        bool isFullScreen = true;

        public ReadingPage()
            : base(null)
        {
            this.layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.contentManager = new ContentManager();
            this.authorLabel = new AuthorLabel();
            layout.Children.Add(this.authorLabel);

            this.bookChapterLabel = new BookChapterLabel();
            layout.Children.Add(bookChapterLabel);

            this.scrollView = new ScrollView
            {
                Padding = Constants.ReadingPadding,
            };

            layout.Children.Add(scrollView);

            this.Content = App.LoadingImage;

            this.tapEventHandler = (s, e) =>
            {
                this.isFullScreen = !this.isFullScreen;
                this.AdjustFullScreen();
            };

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
                Grid verseMarker = this.contentManager.Verses[arg];
                await this.scrollView.ScrollToAsync(verseMarker, ScrollToPosition.Start, false);

                this.isFullScreen = true;
                this.AdjustFullScreen();
            });

            // Listen to show loading
            MessagingCenter.Subscribe<INavigation>(this, ShowLoadingMessage, (sender) =>
            {
                this.ToggleLoading(true);
            });

            // Listen to settings changed
            MessagingCenter.Subscribe<SettingsPage>(this, SettingsChangedMessage, async (sender) =>
            {
                await this.Initialize(this.readingInfo, this.scrollView.ScrollX, this.scrollView.ScrollY);
            });
        }

        protected override async void OnDisappearing()
        {
            await this.SaveLastPosition();
        }

        public async Task SaveLastPosition()
        {
            await ReadingPositionManager.SaveLastPosition(
                this.readingInfo,
                this.scrollView.ScrollX,
                this.scrollView.ScrollY);
        }

        public async Task Initialize(
            ReadingInfo readingInfo,
            double x = 0,
            double y = 0,
            bool isFirstTime = false)
        {
            this.readingInfo = readingInfo;
            this.scrollX = x;
            this.scrollY = y;

            // This has internal cache
            List<BookInfo> booksInfo = await BookNameManager.GetBookNames(readingInfo.IsNT);
            BookInfo bookInfo = booksInfo[readingInfo.BookNumber - 1];

            ReadingColor color = SettingsManager.ToColor(SettingsManager.GetBackgroundColor());
            this.BackgroundColor = color.BackgroundColor;

            await this.authorLabel.Initialize(
                ReadingPage.AuthorChangedMessage,
                this.readingInfo.Author,
                this.readingInfo.IsNT,
                this.readingInfo.BookNumber,
                color);

            await this.UpdateContent(isFirstTime, color);

            await this.bookChapterLabel.Initialize(
                readingInfo,
                bookInfo,
                this.contentManager.Verses,
                color);

            this.ToggleLoading(false);

            this.isFullScreen = true;
            this.AdjustFullScreen();
        }

        protected override async void OnAppearing()
        {
            if (this.scrollX != 0 || this.scrollY != 0)
            {
                await Task.Yield();

                try
                {
                    // We don't want page loading to be blocked for any reason
                    await this.scrollView.ScrollToAsync(this.scrollX, this.scrollY, false);
                }
                catch (Exception e)
                {
                }

                this.scrollX = 0;
                this.scrollY = 0;
            }
        }

        private void ToggleLoading(bool isVisible)
        {
            if (isVisible)
            {
                this.Content = App.LoadingImage;
            }
            else
            {
                this.Content = this.layout;
            }
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
            else
            {
                // CaptersGrid showed loading, unshowing it here
                this.ToggleLoading(false);
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

        private async Task UpdateContent(bool isFirstTime, ReadingColor color)
        {
            // Update content
            this.scrollView.Content = await this.contentManager.GetContent(
                isFirstTime,
                color,
                this.readingInfo,
                this.tapEventHandler);

            this.scrollView.BackgroundColor = color.BackgroundColor;
        }

        private void AdjustFullScreen()
        {
            if (!this.isFullScreen)
            {
                this.authorLabel.IsVisible = true;
                this.bookChapterLabel.IsVisible = true;
            }
            else
            {
                this.authorLabel.IsVisible = false;
                this.bookChapterLabel.IsVisible = false;
            }
        }
    }
}
