using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Formatter
{
    public static class ContentDownloader
    {
        const string Author = "Father-Antonious-Fekry";

        public static void DownloadAll()
        {
            List<string> lines = File.ReadAllLines(@"Metadata\BookIds.txt").ToList();

            bool isNT = false;
            string baseDirectory = @"F:\git\inter\src\Data\Original";

            foreach (string line in lines)
            {
                string urlPrefix;

                if (line == string.Empty)
                {
                    isNT = true;
                    continue;
                }

                int bookNumber = int.Parse(line.Substring(0, 2));
                string directory;
                if (isNT)
                {
                    directory = Path.Combine(baseDirectory, Author, "nt", bookNumber.ToString());
                    urlPrefix =
                        "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-02-New-Testament/"
                        + Author +
                        "/"
                        + line;
                }
                else
                {
                    directory = Path.Combine(baseDirectory, Author, "ot", bookNumber.ToString());
                    urlPrefix =
                        "http://st-takla.org/pub_Bible-Interpretations/Holy-Bible-Tafsir-01-Old-Testament/"
                        + Author +
                        "/"
                        + line;
                }

                // Create directory
                Directory.CreateDirectory(directory);

                DownloadBook(
                    urlPrefix,
                    directory);
            }
        }

        private static void DownloadBook(
            string urlPrefix,
            string baseDirectory)
        {
            // Introduction
            string url = urlPrefix + "__00-introduction.html";
            string content = HttpHelpers.GetPage(url);
            File.WriteAllText(baseDirectory + @"\0.html", content, Encoding.UTF8);

            // Chapters
            for (int i = 1; i < 100; i++)
            {
                string chapterNumber = i.ToString("D2");
                string fileName = baseDirectory + @"\" + i + ".html";
                if (File.Exists(fileName))
                {
                    continue;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));

                url = urlPrefix + "__01-Chapter-" + chapterNumber + ".html";
                content = HttpHelpers.GetPage(url);

                if (content.Contains("404 File Not Found!"))
                {
                    break;
                }

                File.WriteAllText(fileName, content, Encoding.UTF8);
            }
        }

        public static void GetBooksIdentifiers()
        {
            string text = File.ReadAllText(@"Metadata\Books.txt");

            List<string> asfar = new List<string>();
            while (true)
            {
                int startIndex = text.IndexOf(Author) + Author.Length + 1;
                if (startIndex < Author.Length + 1)
                {
                    break;
                }

                int endIndex = text.IndexOf("__00");
                string sefrId = text.Substring(startIndex, endIndex - startIndex);
                asfar.Add(sefrId);

                text = text.Substring(endIndex + 2);
            }

            File.WriteAllLines(@"Metadata\BookIds.txt", asfar);
        }
    }
}
