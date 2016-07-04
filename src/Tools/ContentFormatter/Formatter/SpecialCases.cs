using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter
{
    static class SpecialCases
    {
        public static string Original(
            string url,
            string page)
        {
            if (url == "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-02-New-Testament/Father-Antonious-Fekry/12-Resalet-Kolosy/Tafseer-Resalat-Colosy__01-Chapter-01.html")
            {
                // Move divider up
                int index = page.IndexOf("<form name=\"commentaries1\">");
                page = page.Insert(index, Constants.Divider);

                index = page.LastIndexOf(Constants.Divider);
                page = page.Remove(index);
            }
            else if (url == "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-02-New-Testament/Father-Antonious-Fekry/24-Resalet-Youhana-2/Tafseer-Resalat-Yo7ana-2__01-Chapter-01.html")
            {
                // Close missing </b>
                string segment = "<font FACE=\"Times New Roman\" SIZE=\"5\" COLOR=\"#000000\"><b>";
                int index = page.IndexOf(segment);
                page = page.Insert(index + segment.Length, "</b>");
            }

            return page;
        }
    }
}
