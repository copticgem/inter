using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-01-Old-Testament/Father-Antonious-Fekry/06-Sefr-Yashoue/Tafseer-Sefr-Yashou3__01-Chapter-03.html";
            // string url = "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-02-New-Testament/Father-Tadros-Yacoub-Malaty/26-Resalet-Yahooza/Tafseer-Resalat-Yahoza__01-Chapter-01.html";

            HttpHelpers.FormatAll();

            // ContentDownloader.DownloadAll();
        }
    }
}
