using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formatter
{
    static class NumbersFormatter
    {
        public static string ReplaceNumbers(string page)
        {
            for (int i = 0; i < page.Length; i++)
            {
                if (page[i] == '{' && page[i + 1] == '{')
                {
                    // Skip numbers between tags e.g. {{v2}}
                    i = page.IndexOf("}}", i);
                }
                else
                {
                    switch (page[i])
                    {
                        case '0':
                            page = ReplaceNumber(page, i, '٠');
                            break;
                        case '1':
                            page = ReplaceNumber(page, i, '١');
                            break;
                        case '2':
                            page = ReplaceNumber(page, i, '٢');
                            break;
                        case '3':
                            page = ReplaceNumber(page, i, '٣');
                            break;
                        case '4':
                            page = ReplaceNumber(page, i, '٤');
                            break;
                        case '5':
                            page = ReplaceNumber(page, i, '٥');
                            break;
                        case '6':
                            page = ReplaceNumber(page, i, '٦');
                            break;
                        case '7':
                            page = ReplaceNumber(page, i, '٧');
                            break;
                        case '8':
                            page = ReplaceNumber(page, i, '٨');
                            break;
                        case '9':
                            page = ReplaceNumber(page, i, '٩');
                            break;
                    }
                }
            }

            return page;
        }

        private static string ReplaceNumber(string page, int index, char newChar)
        {
            StringBuilder sb = new StringBuilder(page);
            sb[index] = newChar;
            return sb.ToString();
        }
    }
}
