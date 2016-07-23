using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    public static class DisplayNameHelper
    {
        public static string GetChapterDisplayName(
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            string text = NumbersHelper.TranslateNumber(chapterNumber);
            if (!isNT && bookNumber == 21)
            {
                if (chapterNumber >= 119 && chapterNumber <= 140)
                {
                    string partNumber = NumbersHelper.TranslateNumber(chapterNumber - 118);
                    text = partNumber + "-" + NumbersHelper.TranslateNumber(119);
                }
                else if (chapterNumber > 140)
                {
                    text = NumbersHelper.TranslateNumber(chapterNumber - 21);
                }
            }

            return text;
        }
    }
}
