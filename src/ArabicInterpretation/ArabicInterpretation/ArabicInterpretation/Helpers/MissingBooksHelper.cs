using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    static class MissingBooksHelper
    {
        public static List<int> GetMissingBooks(
            Author author,
            bool isNT)
        {
            List<int> missingBooks = new List<int>();
            if (author == Author.FrTadros && isNT == false)
            {
                // This number matches ot.txt/book files (index starts from 1)
                missingBooks.Add(14);
                missingBooks.Add(17);
                missingBooks.Add(26);
                missingBooks.Add(30);
                missingBooks.Add(45);
                missingBooks.Add(46);
            }

            return missingBooks;
        }
    }
}
