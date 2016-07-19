using Core;
using System;
using System.Globalization;

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

        public static ReadingInfo FromPositionString(
            string position,
            out double x, 
            out double y)
        {
            x = 0;
            y = 0;

            if (string.IsNullOrWhiteSpace(position))
            {
                return null;
            }

            string[] tokens = position.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);

            x = double.Parse(tokens[4]);
            y = double.Parse(tokens[5]);

            return new ReadingInfo(
                author: (Author)Enum.Parse(typeof(Author), tokens[0]),
                isNT: bool.Parse(tokens[1]),
                bookNumber: int.Parse(tokens[2]),
                chapterNumber: int.Parse(tokens[3]));
        }

        public string ToPositionString(double x, double y)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}_{1}_{2}_{3}_{4}_{5}",
                this.Author,
                this.IsNT,
                this.BookNumber,
                this.ChapterNumber,
                x,
                y);
        }
    }
}
