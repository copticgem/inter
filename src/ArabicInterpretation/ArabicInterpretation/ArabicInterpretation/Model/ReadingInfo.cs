using Core;

namespace ArabicInterpretation.Model
{
    public class ReadingInfo
    {
        public ReadingInfo(
            Author author,
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            this.Author = author;
            this.IsNT = isNT;
            this.BookNumber = bookNumber;
            this.ChapterNumber = chapterNumber;
        }

        public Author Author { get; set; }

        public bool IsNT { get; set; }

        public int BookNumber { get; set; }

        public int ChapterNumber { get; set; }
    }
}
