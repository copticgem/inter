﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Formatter
{
    public static class HttpHelpers
    {
        public static void FormatAll()
        {
            string author = "Father-Antonious-Fekry";
            string baseDirectory = @"F:\git\inter\src\Data\Original";
            baseDirectory = Path.Combine(baseDirectory, author, "ot");

            int goodFiles = 0;
            string[] files = Directory.GetFiles(baseDirectory, "*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string page = File.ReadAllText(file);
                string formattedPage = GetFormattedPage(page);

                if (formattedPage.Contains(">") || formattedPage.Contains("<"))
                {
                    throw new InvalidOperationException("Content still contains html tags");
                }

                goodFiles++;
            }
        }

        public static string GetFormattedPage(string page)
        {
            page = RemoveHeaderAndFooter(page);

            // Replace paragraphs
            page = Regex.Replace(
                input: page,
                pattern: "<p [^>]*>",
                replacement: Constants.Replacements.Paragraph);

            page = page.Replace("<p>", "{{p}}");
            page = page.Replace("</p>", string.Empty);

            // Replace bold characters
            page = Regex.Replace(
                input: page,
                pattern: "<b [^>]*>",
                replacement: Constants.Replacements.BoldStart);

            page = page.Replace(Constants.BoldStart, Constants.Replacements.BoldStart);
            page = page.Replace(Constants.BoldEnd, Constants.Replacements.BoldEnd);

            // Replace quote
            page = page.Replace("&quot;", "\"");

            // Replace &
            page = page.Replace("&amp;", "&");

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

            // Remove markups
            page = RemoveMarkups(page);

            // Validate no nested tags
            page = ValidateTags(page);

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

            if (startIndex <= 0)
            {
                throw new InvalidOperationException("Footer not found");
            }

            page = page.Substring(
                startIndex: 0,
                length: startIndex - 1);

            return page;
        }

        private static string ValidateTags(string page)
        {
            List<string> newString = new List<string>();

            string[] tokens = Regex.Split(
                input: page,
                pattern: @"({{/*\w+}})");

            string endToken = null;
            foreach (string token in tokens)
            {
                newString.Add(token);

                if (token.StartsWith("{{"))
                {
                    if (endToken != null)
                    {
                        if (token.StartsWith("{{v"))
                        {
                            // We don't want verse tag in the middle since it's a separate label
                            // Move up till it's not nested
                            ReplaceVerseTag(newString, endToken.Replace("/", string.Empty));
                            continue;
                        }

                        if (token != endToken)
                        {
                            // Remove {{b}} from inside {{t}}
                            if (endToken == "{{/t}}" && (token == "{{b}}" || token == "{{/b}}"))
                            {
                                newString.RemoveAt(newString.Count - 1);
                                continue;
                            }

                            // Remove {{b}} from outside {{t}}
                            if (endToken == "{{/b}}" && token == "{{t}}")
                            {
                                newString.RemoveAt(newString.LastIndexOf("{{b}}"));
                                endToken = "{{/t}}";
                                continue;
                            }

                            if (endToken == "{{/b}}" && token == "{{p}}")
                            {
                                // Split {{b}} tag
                                newString.Insert(newString.Count - 1, endToken);
                                newString.Add("{{b}}");
                                continue;
                            }

                            throw new InvalidOperationException(
                                string.Format(
                                    "Nested tags detected, outer tag '{0}', inner tag '{1}'",
                                    endToken,
                                    token));
                        }
                        else
                        {
                            endToken = null;
                            continue;
                        }
                    }

                    if (token == "{{l}}" ||
                        token == "{{p}}" ||
                        token == "{{d}}" ||
                        token.StartsWith("{{v"))
                    {
                        continue;
                    }

                    if (token.StartsWith("{{/"))
                    {
                        // An end tag with no match, delete
                        newString.RemoveAt(newString.Count - 1);
                        continue;
                    }

                    endToken = token.Insert(2, "/");
                }
            }

            return string.Join(string.Empty, newString);
        }

        private static void ReplaceVerseTag(
            List<string> tags,
            string openTag)
        {
            string verseTag = tags.Last();
            tags.RemoveAt(tags.Count - 1);

            for (int i = tags.Count - 1; i >= 0; i--)
            {
                if (tags[i] == openTag)
                {
                    // Insert the verse before the openTag
                    tags.Insert(i, verseTag);
                    return;
                }
            }

            throw new InvalidOperationException("Unable to find a spot for tag: " + verseTag);
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

            //// Remove all remaining identifiers
            // Multiple verse refs
            // TODO: this doesn't work
            //page = Regex.Replace(
            //    input: page,
            //    pattern: "<a name=\"([0-9]|\\-)+\">([0-9]|\\-)*</a>",
            //    replacement: string.Empty);

            // Links to other chapters/topics
            page = ReplaceTag(page, "a", string.Empty);

            return page;
        }

        private static string RemoveMarkups(string page)
        {
            // Remove special characters
            page = page.Replace("\n", string.Empty);
            page = page.Replace("\r", string.Empty);
            page = page.Replace("\t", string.Empty);

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

            // Remove tables with images 
            // http://stackoverflow.com/questions/406230/regular-expression-to-match-line-that-doesnt-contain-a-word
            page = Regex.Replace(
                input: page,
                pattern: "<table[^>]*>((?!</table>).)*<img[^>]*>((?!</table>).)*</table>",
                replacement: string.Empty);

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

            // Remove table, tr, td
            page = ReplaceTag(page, "table", string.Empty);
            page = ReplaceTag(page, "tr", string.Empty);
            page = ReplaceTag(page, "td", string.Empty);

            // Remove div
            page = ReplaceTag(page, "div", string.Empty);

            // Remove empty tags
            page = page.Replace("{{b}}{{/b}}", string.Empty);
            page = page.Replace("{{t}}{{/t}}", string.Empty);

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
                replacement: replacement);

            page = Regex.Replace(
                input: page,
                pattern: "</" + tagName + ">",
                replacement: replacement);

            return page;
        }
    }
}
