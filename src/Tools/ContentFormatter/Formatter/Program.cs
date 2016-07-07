using Formatter.Formatters;
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
            // HttpHelpers.FormatOne(Constants.Authors.FrAntonious, true, 1, 3, true);

            // HttpHelpers.FormatAll(Constants.Authors.FrAntonious, true);
            // ContentDownloader.DownloadAll();

            BookNumbers.Format();
        }
    }
}
