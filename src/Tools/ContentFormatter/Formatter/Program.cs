using Formatter.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Formatter
{
    class Program
    {
        static void Main(string[] args)
        {
            // HttpHelpers.FormatOne(Constants.Authors.FrAntonious, false, 7, 13, true);

            HttpHelpers.FormatAll(Constants.Authors.FrTadros, true);

            // ContentDownloader.DownloadAll();

            // BookNumbers.Format();
        }
    }
}
