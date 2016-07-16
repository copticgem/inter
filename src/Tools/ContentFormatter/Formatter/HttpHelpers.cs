using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using Formatter.Formatters;
using System.Web;

namespace Formatter
{
    public static class HttpHelpers
    {
        public static void FormatOne(
            string author,
            bool isNT,
            int bookNumber,
            int chapterNumber,
            bool save = true)
        {
            string file = @"F:\git\inter\src\Data\Original";
            string type = isNT ? "nt" : "ot";
            file = Path.Combine(
                file,
                author,
                type,
                bookNumber.ToString(),
                chapterNumber + ".html");

            string page;

            string modifiedFile = file.Replace("Original", "Modified");
            if (File.Exists(modifiedFile))
            {
                page = File.ReadAllText(modifiedFile);
            }
            else
            {
                page = File.ReadAllText(file);
            }

            string formattedPage = GetFormattedPage(page);

            if (formattedPage.Contains(">") || formattedPage.Contains("<"))
            {
                throw new InvalidOperationException("Content still contains html tags");
            }

            if (save)
            {
                string newPath = file.Replace(
                    @"Data\Original\",
                    @"ArabicInterpretation\ArabicInterpretation\Core\Resources\");

                newPath = Path.ChangeExtension(newPath, ".txt");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                File.WriteAllText(newPath, formattedPage, Encoding.UTF8);
            }
        }

        public static void FormatAll(string author, bool save = true)
        {
            string baseDirectory = @"F:\git\inter\src\Data\Original";
            baseDirectory = Path.Combine(baseDirectory, author);

            string[] files = Directory.GetFiles(baseDirectory, "*", SearchOption.AllDirectories);
            files = files.OrderBy(f => f).ToArray();

            int badFiles = 0;
            foreach (string file in files)
            {
                try
                {
                    FormatFile(file, save);
                }
                catch(Exception e)
                {
                    badFiles++;
                }

                Console.Write(".");
            }

            Console.WriteLine("bad files: " + badFiles);
            Console.ReadLine();
        }

        private static void FormatFile(string file, bool save)
        {
            string modifiedFile = file.Replace("Original", "Modified");

            string page;
            if (File.Exists(modifiedFile))
            {
                page = File.ReadAllText(modifiedFile);
            }
            else
            {
                page = File.ReadAllText(file);
            }

            string formattedPage = GetFormattedPage(page);

            if (formattedPage.Contains(">") || formattedPage.Contains("<"))
            {
                throw new InvalidOperationException("Content still contains html tags");
            }

            if (save)
            {
                string newPath = file.Replace(
                    @"Data\Original\",
                    @"ArabicInterpretation\ArabicInterpretation\Core\Resources\");

                newPath = Path.ChangeExtension(newPath, ".txt");
                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                File.WriteAllText(newPath, formattedPage, Encoding.UTF8);
            }
        }

        public static string GetFormattedPage(string page)
        {
            // HTML Decode
            page = HttpUtility.HtmlDecode(page);

            // Remove headers and footers
            page = RemoveHeaderAndFooter(page);

            // Remove special characters
            page = page.Replace("\r\n", " ");
            page = page.Replace("\n", string.Empty);
            page = page.Replace("\r", string.Empty);
            page = page.Replace("\t", string.Empty);

            // Remove tables with images 
            // http://stackoverflow.com/questions/406230/regular-expression-to-match-line-that-doesnt-contain-a-word
            page = Regex.Replace(
                input: page,
                pattern: "<table[^>]*>((?!</table>).)*<img[^>]*>((?!</table>).)*</table>",
                replacement: string.Empty);

            // Replace paragraphs
            page = Regex.Replace(
                input: page,
                pattern: "<p [^>]*>",
                replacement: Constants.Replacements.Paragraph);

            page = Regex.Replace(
                input: page,
                pattern: "<p>",
                replacement: "{{p}}",
                options: RegexOptions.IgnoreCase);

            page = Regex.Replace(
                input: page,
                pattern: "</p>",
                replacement: "{{p}}",
                options: RegexOptions.IgnoreCase);

            // Replace bold characters
            page = Regex.Replace(
                input: page,
                pattern: "<b [^>]*>",
                replacement: Constants.Replacements.BoldStart);

            page = page.Replace(Constants.BoldStart, Constants.Replacements.BoldStart);
            page = page.Replace(Constants.BoldEnd, Constants.Replacements.BoldEnd);

            page = page.Replace("<strong>", "{{b}}");
            page = page.Replace("</strong>", "{{/b}}");

            // Replace quote
            page = page.Replace("&quot;", "\"");

            // Replace &
            page = page.Replace("&amp;", "&");

            // Replace symbols
            page = page.Replace("&#8595;", "↓");

            // Replace subtitles
            page = Regex.Replace(
               input: page,
               pattern: "<h\\d[^>]*>",
               replacement: "{{t}}");

            page = Regex.Replace(
                input: page,
                pattern: "</h\\d>",
                replacement: "{{/t}}");

            // Replace divider
            page = Regex.Replace(
               input: page,
               pattern: "<img border=\"0\" src=\"../../../../Pix/Priests/divider-3.jpg\"[^>]*>",
               replacement: "{{d}}");

            // Add Verse identifier
            page = ReplaceVerseIdentifier(page);

            // Replace hr with divider
            page = Regex.Replace(
               input: page,
               pattern: "<hr[^>]*>",
               replacement: "{{d}}");

            // Replace tables
            page = TableFormatter.ReplaceTableTags(page);

            // Replace numbers
            page = NumbersFormatter.ReplaceNumbers(page);

            // Remove markups
            page = RemoveMarkups(page);

            // Validate no nested tags
            page = Validation.ValidateTags(page);

            // Format spaces
            page = SpacingFormatter.FormatSpaces(page);

            // Validate info
            page = Validation.ValidateInfo(page);

            return page;
        }

        private static string RemoveHeaderAndFooter(string page)
        {
            // Remove header
            int startIndex;
            int endIndex;
            if (page.Contains("اذهب مباشرةً لتفسير الآية"))
            {
                // In this case, we look for the first divider
                startIndex = page.IndexOf(Constants.Divider);
                if (startIndex <= 0)
                {
                    throw new InvalidOperationException("First divider not found");
                }

                endIndex = page.IndexOf('>', startIndex);
                page = page.Substring(endIndex + 1);

                startIndex = page.IndexOf(Constants.DividerSuffix);
                if (startIndex == 0)
                {
                    page = page.Substring(Constants.DividerSuffix.Length);
                }
            }
            else
            {
                // We look for the options
                startIndex = page.IndexOf("<form name=\"commentaries1\">");
                if (startIndex <= 0)
                {
                    throw new InvalidOperationException("Commentaries form not found");
                }

                string endTag = "</form>";
                endIndex = page.IndexOf(endTag, startIndex);
                page = page.Substring(endIndex + endTag.Length);
            }

            // Remove footer
            string references = "<a name=\"الحواشي_والمراجع";
            if (page.Contains(references))
            {
                startIndex = page.IndexOf(references);
            }
            else
            {
                startIndex = page.LastIndexOf(Constants.Divider);
            }

            startIndex = page.LastIndexOf(Constants.Divider);


            if (startIndex <= 0)
            {
                throw new InvalidOperationException("Footer not found");
            }

            page = page.Substring(
                startIndex: 0,
                length: startIndex - 1);

            return page;
        }

        private static string ReplaceVerseIdentifier(string page)
        {
            HashSet<int> verses = new HashSet<int>();

            MatchCollection matchCollection = Regex.Matches(page, "<a name=\"(?<numberGroup>[0-9]+)\">(?<visibleNumber>[0-9]*)</a>", RegexOptions.IgnoreCase);
            foreach (Match match in matchCollection)
            {
                if (match.Success)
                {
                    int verseNumber = int.Parse(match.Groups["numberGroup"].Value);
                    if (verses.Contains(verseNumber))
                    {
                        // Repeated identifier
                        continue;
                    }

                    page = page.Replace(
                        oldValue: match.Value,
                        newValue: "{{v" + verseNumber + "}}" + match.Groups["visibleNumber"].Value);

                    verses.Add(verseNumber);
                }
            }

            // Links to other chapters/topics
            page = ReplaceTag(page, "a", string.Empty);

            return page;
        }

        private static string RemoveMarkups(string page)
        {
            // Remove span
            page = ReplaceTag(page, "span", string.Empty);

            // Replace &nbsp;
            page = Regex.Replace(
                input: page,
                pattern: "(&nbsp;)+",
                replacement: " ");

            // Remove newlines
            page = ReplaceTag(page, "br", string.Empty);

            // Remove blockquotes
            page = ReplaceTag(page, "blockquote", string.Empty);

            // Remove more info 
            page = Regex.Replace(
                input: page,
                pattern: "(انظر المزيد عن هذا الموضوع هنا في<font color=\"#000000\">موقع الأنبا تكلا</font> في أقسام المقالات والتفاسير الأخرى)",
                replacement: string.Empty);

            page = Regex.Replace(
                input: page,
                pattern: @"http://st-takla.org/(\w*\-*_*/*)+.html",
                replacement: string.Empty);

            // Remove body
            page = ReplaceTag(page, "body", string.Empty);

            // Remove other images
            page = ReplaceTag(page, "img", string.Empty);

            // Remove list
            page = ReplaceTag(page, "li", string.Empty);
            page = ReplaceTag(page, "ul", string.Empty);
            page = ReplaceTag(page, "ol", string.Empty);

            // Remove bstyle
            page = ReplaceTag(page, "bstyle", string.Empty);

            // Remove font
            page = ReplaceTag(page, "font", string.Empty);

            // Remove if tags 
            page = ReplaceTag(page, "!\\[", string.Empty);

            // Remove word related tags
            page = ReplaceTag(page, "o:p", string.Empty);
            page = ReplaceTag(page, "!--", string.Empty);
            page = Regex.Replace(
                input: page,
                pattern: "<o:[^>]*>",
                replacement: string.Empty);

            page = Regex.Replace(
               input: page,
               pattern: "<v:[^>]*>",
               replacement: string.Empty);

            page = Regex.Replace(
               input: page,
               pattern: "</v:[^>]*>",
               replacement: string.Empty);

            // Remove dir
            page = ReplaceTag(page, "dir", string.Empty);

            // TODO: Remove tags for now
            page = ReplaceTag(page, "u", string.Empty);
            page = ReplaceTag(page, "i", string.Empty);
            page = ReplaceTag(page, "sup", string.Empty);
            page = ReplaceTag(page, "sub", string.Empty);

            page = ReplaceTag(page, "ins", string.Empty);

            // Remove div
            page = ReplaceTag(page, "div", string.Empty);

            // Remove empty tags
            page = page.Replace("{{b}}{{/b}}", string.Empty);
            page = page.Replace("{{t}}{{/t}}", string.Empty);

            // TODO: Move to specialCases original
            page = page.Replace("{{t}}{{b}}    {{d}}{{/b}}{{/t}}", "{{d}}");
            page = page.Replace("<sub>&#8592;</sub>&#8593;", string.Empty);

            // Remove {{b}} surrounding {{d}}
            page = Regex.Replace(
                input: page,
                pattern: "{{b}} *{{d}}{{/b}}",
                replacement: "{{d}}");
            
            return page;
        }

        private static string ReplaceTag(
            string page,
            string tagName,
            string replacement)
        {
            page = Regex.Replace(
                input: page,
                pattern: "<" + tagName + "[^>]*>",
                replacement: replacement,
                options: RegexOptions.IgnoreCase);

            page = Regex.Replace(
                input: page,
                pattern: "</" + tagName + ">",
                replacement: replacement,
                options: RegexOptions.IgnoreCase);

            return page;
        }
    }
}
