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
        public const string AuthorChangedMessage = "ReadingPageAuthorChanged";
        public const string SettingsChangedMessage = "ReadingPageSettingsChanged";
        public const string ShowLoadingMessage = "ReadingPageShowLoading";

        ReadingInfo readingInfo;

        AuthorLabel authorLabel;
        BookChapterLabel bookChapterLabel;
        Image loadingImage;

        Dictionary<int, Grid> verses;

        ScrollView scrollView;
        double scrollX;
        double scrollY;

        bool isFullScreen = true;

        public ReadingPage()
            : base(null)
        {
            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
            };

            this.authorLabel = new AuthorLabel();
            layout.Children.Add(this.authorLabel);

            this.bookChapterLabel = new BookChapterLabel();
            layout.Children.Add(bookChapterLabel);

            this.loadingImage = new Image
            {
                Source = ImageSource.FromResource("ArabicInterpretation.Resources.loading.png"),
                IsVisible = false,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            layout.Children.Add(this.loadingImage);

            this.scrollView = new ScrollView
            {
                Padding = Constants.ReadingPadding,
            };

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
                this.verses,
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
                await this.scrollView.ScrollToAsync(this.scrollX, this.scrollY, false);

                this.scrollX = 0;
                this.scrollY = 0;
            }
        }

        private void ToggleLoading(bool isVisible)
        {
            if (isVisible)
            {
                this.scrollView.IsVisible = false;
                this.loadingImage.IsVisible = true;
            }
            else
            {
                this.loadingImage.IsVisible = false;
                this.scrollView.IsVisible = true;
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
            string content = await FileHelper.GetFile(
                this.readingInfo.Author,
                this.readingInfo.IsNT,
                this.readingInfo.BookNumber,
                this.readingInfo.ChapterNumber);

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

            this.scrollView.BackgroundColor = color.BackgroundColor;
            this.scrollView.Content = chapterLayout;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
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
