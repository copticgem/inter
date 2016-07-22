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

            // Book & chapter layout
            StackLayout bookChapter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 0,
                HorizontalOptions = LayoutOptions.CenterAndExpand
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
            Dictionary<int, Grid> verses,
            ReadingColor color)
        {
            this.BackgroundColor = color.SecondBarColor;

            await this.chapterLabel.Initialize(
                readingInfo: readingInfo,
                chaptersCount: bookInfo.ChaptersCount,
                color: color);

            await this.bookLabel.Initialize(
                author: readingInfo.Author,
                isNT: readingInfo.IsNT,
                bookNumber: readingInfo.BookNumber, 
                bookName: bookInfo.Name,
                color: color);

            await this.verseLabel.Initialize(verses, color);

            this.settingsLabel.Initialize(color);
        }
    }
}
