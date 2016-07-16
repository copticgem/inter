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

        bool shouldPopTwice;
        Author author;
        bool isNT;
        int bookNumber;
        int chaptersCount;

        AuthorLabel authorLabel;
        ChaptersGrid chaptersGrid;

        public ChapterChooserPage()
            :base("الاصحاحات")
        {
            StackLayout layout = new StackLayout
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

            this.Content = layout;

            // Listen to author changes
            MessagingCenter.Subscribe<AuthorsGrid, Author>(this, ChapterChooserPage.AuthorChangedMessage, async (sender, arg) =>
            {
                if (this.author != arg)
                {
                    this.author = arg;

                    // TODO: No need to initialize all these
                    await this.Initialize(
                        shouldPopTwice: this.shouldPopTwice,
                        author: arg,
                        isNT: this.isNT,
                        bookNumber: this.bookNumber,
                        chaptersCount: this.chaptersCount);
                }
            });
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
                bookNumber);

            await this.chaptersGrid.Initialize(
                shouldPopTwice,
                author,
                isNT,
                bookNumber,
                chaptersCount);
        }
    }
}
