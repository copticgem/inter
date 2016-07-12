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
            int chaptersCount,
            List<string> ntBooks,
            List<string> otBooks)
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.Center;

            ChapterLabel chapterLabel = new ChapterLabel(
                isNT: isNT, 
                bookNumber: bookNumber, 
                chapterNumber: chapterNumber,
                chaptersCount: chaptersCount);

            this.Children.Add(chapterLabel);

            BookLabel bookLabel = new BookLabel(isNT, bookNumber, ntBooks, otBooks);
            this.Children.Add(bookLabel);
        }
    }
}
