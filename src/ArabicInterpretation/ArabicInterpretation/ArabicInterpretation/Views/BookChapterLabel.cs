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
        public BookChapterLabel(
            bool isNT,
            int bookNumber,
            int chapterNumber,
            BookInfo bookInfo)
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.BackgroundColor = ColorManager.Backgrounds.BookChapterBar;

            ChapterLabel chapterLabel = new ChapterLabel(
                isNT: isNT, 
                bookNumber: bookNumber, 
                chapterNumber: chapterNumber,
                chaptersCount: bookInfo.ChaptersCount);

            this.Children.Add(chapterLabel);

            BookLabel bookLabel = new BookLabel(isNT, bookNumber, bookInfo.Name);
            this.Children.Add(bookLabel);
        }
    }
}
