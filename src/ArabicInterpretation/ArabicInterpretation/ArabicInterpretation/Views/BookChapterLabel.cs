using ArabicInterpretation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    public class BookChapterLabel : StackLayout
    {
        ChapterLabel chapterLabel;
        BookLabel bookLabel;
        VerseLabel verseLabel;
        SettingsLabel settingsLabel;

        public BookChapterLabel()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.VerticalOptions = LayoutOptions.StartAndExpand;
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            // Book & chapter layout
            StackLayout bookChapter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.Center
            };

            this.chapterLabel = new ChapterLabel();
            bookChapter.Children.Add(chapterLabel);

            this.bookLabel = new BookLabel();
            bookChapter.Children.Add(this.bookLabel);

            // Main layout
            this.settingsLabel = new SettingsLabel();
            this.Children.Add(this.settingsLabel);

            this.Children.Add(bookChapter);

            this.verseLabel = new VerseLabel();
            this.Children.Add(this.verseLabel);
        }

        public async Task Initialize(
            ReadingInfo readingInfo,
            BookInfo bookInfo,
            Dictionary<int, Grid> verses)
        {
            await this.chapterLabel.Initialize(
                readingInfo: readingInfo,
                chaptersCount: bookInfo.ChaptersCount);

            await this.bookLabel.Initialize(
                author: readingInfo.Author,
                isNT: readingInfo.IsNT,
                bookNumber: readingInfo.BookNumber, 
                bookName: bookInfo.Name);

            await this.verseLabel.Initialize(verses);
        }
    }
}
