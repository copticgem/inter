using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Formatter
{
    public static class HttpHelpers
    {
        public static string GetPageContent(string url)
        {
            string page = GetPage(url);

            // Remove header
            int startIndex = page.IndexOf(Constants.Divider);
            if (startIndex <= 0)
            {
                throw new InvalidOperationException("First divider not found");
            }

            int endIndex = page.IndexOf('>', startIndex);
            page = page.Substring(endIndex + 1);

            startIndex = page.IndexOf(Constants.DividerSuffix);
            if (startIndex == 0)
            {
                page = page.Substring(Constants.DividerSuffix.Length);
            }

            // Remove footer
            startIndex = page.LastIndexOf(Constants.Divider);
            if (startIndex <= 0)
            {
                throw new InvalidOperationException("Last divider not found");
            }

            page = page.Substring(
                startIndex: 0,
                length: startIndex - 1);

            // Replace paragraphs
            page = page.Replace(Constants.Paragraph, Constants.Replacements.Paragraph);
            page = page.Replace("</p>", string.Empty);

            // Remove special characters
            page = page.Replace("\n\t", string.Empty);

            // Replace bold characters
            page = page.Replace(Constants.BoldStart, Constants.Replacements.BoldStart);
            page = page.Replace(Constants.BoldEnd, Constants.Replacements.BoldEnd);

            // Replace quote
            page = page.Replace("&quot;", ":");

            return page;
        }

        public static string GetPage(string url)
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
