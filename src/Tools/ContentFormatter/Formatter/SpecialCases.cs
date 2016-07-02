using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            return page;
        }
    }
}
