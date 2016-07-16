using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter
{
    class Validation
    {
        public static string ValidateInfo(string page)
        {
            page = Regex.Replace(
                page,
                "(انظر *المزيد *عن *هذا *الموضوع *هنا *في *موقع *الأنبا *تكلا *في *أقسام *المقالات *و *التفاسير *الأخرى).",
                string.Empty);

            page = Regex.Replace(
                page,
                "وستجد المزيد عن هذا الموضوع هنا في موقع الأنبا تكلاهيمانوت في صفحات قاموس وتفاسير الكتاب المقدس الأخرى.",
                string.Empty);

            page = Regex.Replace(
                page,
                "\\([^\\)]*موقع الأنبا تكلا[^\\)]*\\)",
                string.Empty);

            if (page.Contains("موقع الأنبا تكلا"))
            {
                throw new InvalidOperationException("موقع الأنبا تكلا");
            }

            return page;
        }

        public static string ValidateTags(string page)
        {
            List<string> newString = new List<string>();

            string[] tokens = Regex.Split(
                input: page,
                pattern: @"({{/*\w+}})");

            string endToken = null;
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (!string.IsNullOrWhiteSpace(token))
                {
                    newString.Add(token);
                }

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

                            if (endToken == "{{/b}}" && (token == "{{p}}" || token == "{{d}}"))
                            {
                                // Split {{b}} tag
                                newString.Insert(newString.Count - 1, endToken);
                                newString.Add("{{b}}");
                                continue;
                            }

                            if (endToken == "{{/b}}" && token == "{{b}}")
                            {
                                // {{b}} inside {{b}}, remove the inner one, closing tag will be removed
                                newString.Remove(newString.Last());
                                continue;
                            }

                            if (endToken == "{{/t}}" && (token == "{{p}}" || token == "{{d}}"))
                            {
                                // Split {{t}} tag
                                newString.Insert(newString.Count - 1, endToken);
                                newString.Add("{{t}}");
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

                    if (token == "{{g}}" || token == "{{gltr}}")
                    {
                        // Handle grid separately
                        int gridEndIndex;
                        string grid = TableFormatter.GetGrid(
                            tokens: tokens.ToList(),
                            gridStartIndex: i,
                            gridEndIndex: out gridEndIndex);

                        i = gridEndIndex;
                        newString.RemoveAt(newString.Count - 1);
                        newString.Add(grid);
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
    }
}
