using Formatter.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            // HttpHelpers.FormatOne(Constants.Authors.FrTadros, false, 23, 2, true);

            HttpHelpers.FormatAll(Constants.Authors.FrAntonious, true);

            // ContentDownloader.DownloadAll();

            // BookNumbers.Format();
        }
    }
}
