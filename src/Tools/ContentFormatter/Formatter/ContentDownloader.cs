using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Formatter
{
    public static class ContentDownloader
    {
        const string Author = Constants.Authors.FrTadros;

        public static void DownloadAll()
        {
            List<string> lines = File.ReadAllLines(@"Metadata\BookIds.txt").ToList();

            string baseDirectory = @"F:\git\inter\src\Data\Original";

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string urlPrefix;

                if (line == string.Empty)
                {
                    continue;
                }

                int bookNumber = int.Parse(line.Substring(0, 2));
                string directory;
                if (i > 46)
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
                    bookNumber,
                    urlPrefix,
                    directory);
            }
        }

        private static void DownloadBook(
            int bookNumber,
            string urlPrefix,
            string baseDirectory)
        {
            bool isMazameer = urlPrefix.Contains("Tafseer-Sefr-El-Mazamir");

            // Introduction
            string url = urlPrefix + "__00-introduction.html";
            if (isMazameer && Author == Constants.Authors.FrAntonious)
            {
                // Special case
                url = urlPrefix + "__00-introduction-2-Intro.html";
            }
            else if (Author == Constants.Authors.FrTadros && url.Contains("23-Resalet-Youhanna-1/Tafseer-Resalat-You7anna-1"))
            {
                urlPrefix = urlPrefix.Replace(
                    "23-Resalet-Youhanna-1/Tafseer-Resalat-You7anna-1", 
                    "23-Resalat-Youhanna-Al-Oula/Tafsir-Resalat-Youhana-I");

                url = url.Replace(
                    "23-Resalet-Youhanna-1/Tafseer-Resalat-You7anna-1", 
                    "23-Resalat-Youhanna-Al-Oula/Tafsir-Resalat-Youhana-I");
            }
            else if (Author == Constants.Authors.FrTadros && url.Contains("24-Resalet-Youhana-2/Tafseer-Resalat-Yo7ana-2"))
            {
                urlPrefix = urlPrefix.Replace(
                    "24-Resalet-Youhana-2/Tafseer-Resalat-Yo7ana-2",
                    "24-Risalat-Yohana-Al-Thania/Tafseer-John-II");

                url = url.Replace(
                    "24-Resalet-Youhana-2/Tafseer-Resalat-Yo7ana-2",
                    "24-Risalat-Yohana-Al-Thania/Tafseer-John-II");
            }
            else if (Author == Constants.Authors.FrTadros && url.Contains("25-Resalet-Yohanna-3/Tafseer-Resalat-Yo7ana-3"))
            {
                urlPrefix = urlPrefix.Replace(
                    "25-Resalet-Yohanna-3/Tafseer-Resalat-Yo7ana-3",
                    "25-Risalat-Uohana-El-Thaletha/Tafseer-John-III");

                url = url.Replace(
                    "25-Resalet-Yohanna-3/Tafseer-Resalat-Yo7ana-3",
                    "25-Risalat-Uohana-El-Thaletha/Tafseer-John-III");
            }

            string content = ContentDownloader.GetPage(url);
            File.WriteAllText(baseDirectory + @"\0.html", content, Encoding.UTF8);

            // Chapters
            for (int i = 1; i < 200; i++)
            {
                string chapterNumber = i.ToString("D2");
                if (isMazameer)
                {
                    chapterNumber = i.ToString("D3");
                }

                string fileName = baseDirectory + @"\" + i + ".html";
                if (File.Exists(fileName))
                {
                    continue;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));

                url = urlPrefix + "__01-Chapter-" + chapterNumber + ".html";
                content = ContentDownloader.GetPage(url);

                if (content.Contains("404 File Not Found!"))
                {
                    break;
                }

                // Apply special cases
                content = SpecialCases.Original(url, content);

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

        private static string GetPage(string url)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Invalid status code received: " + response.StatusCode);
            }

            Stream stream = response.Content.ReadAsStreamAsync().Result;

            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("windows-1256")))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
