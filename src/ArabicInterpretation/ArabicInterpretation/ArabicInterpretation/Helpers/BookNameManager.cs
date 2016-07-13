using ArabicInterpretation.Model;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    public static class BookNameManager
    {
        private static List<BookInfo> ntBooks;
        private static List<BookInfo> otBooks;

        public static async Task<List<BookInfo>> GetBookNames(bool isNT)
        {
            if (isNT)
            {
                if (ntBooks == null)
                {
                    ntBooks = await LoadBooks(isNT);
                }

                return ntBooks;
            }
            else
            {
                if (otBooks == null)
                {
                    otBooks = await LoadBooks(isNT);
                }

                return otBooks;
            }
        }

        private static async Task<List<BookInfo>> LoadBooks(bool isNT)
        {
            string index = await FileHelper.GetIndex(isNT);
            string[] books = index.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return books.Select(b =>
            {
                string[] tokens = b.Split(':');
                return new BookInfo(tokens[0], tokens[1], int.Parse(tokens[2]));
            })
            .ToList();
        }
    }
}
