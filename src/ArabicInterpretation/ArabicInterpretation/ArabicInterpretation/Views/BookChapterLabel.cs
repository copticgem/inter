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

        public BookChapterLabel()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            this.chapterLabel = new ChapterLabel();
            this.Children.Add(chapterLabel);

            this.bookLabel = new BookLabel();
            this.Children.Add(this.bookLabel);
        }

        public async Task Initialize(
            ReadingInfo readingInfo,
            BookInfo bookInfo)
        {
            await this.chapterLabel.Initialize(
                readingInfo: readingInfo,
                chaptersCount: bookInfo.ChaptersCount);

            await this.bookLabel.Initialize(
                author: readingInfo.Author,
                isNT: readingInfo.IsNT,
                bookNumber: readingInfo.BookNumber, 
                bookName: bookInfo.Name);
        }
    }
}
