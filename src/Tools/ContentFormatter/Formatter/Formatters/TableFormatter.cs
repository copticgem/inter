using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter
{
    static class TableFormatter
    {
        public static string GetGrid(
            List<string> tokens,
            int gridStartIndex,
            out int gridEndIndex)
        {
            gridEndIndex = tokens.IndexOf("{{/g}}", gridStartIndex);
            if (gridEndIndex <= 0)
            {
                throw new InvalidOperationException("End grid not found!");
            }

            // TODO: Use thead
            // Remove bold and empty lines
            List<string> grid = tokens
                .GetRange(gridStartIndex, gridEndIndex - gridStartIndex + 1)
                .Where(s => !s.Equals("{{b}}", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals("{{/b}}", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals("{{p}}", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals("{{t}}", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals("{{/t}}", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals(string.Empty) &&
                            !string.IsNullOrWhiteSpace(s) &&
                            !s.Equals("<thead>", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals(" <thead> ", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals("</thead>", StringComparison.InvariantCultureIgnoreCase) &&
                            !s.Equals(" </thead> ", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (grid[grid.Count - 2] != "{{/gr}}" ||
                grid[grid.Count - 3] != "{{/gc}}")
            {
                throw new InvalidOperationException("Invalid grid end");
            }

            List<string> expectedTags = new List<string>
            {
                "{{gr}}",
                "{{gc}}",
                "{{/gc}}",
                "{{/gr}}"
            };

            int index = 0;
            for (int i = 1; i < grid.Count - 1; i++)
            {
                string token = grid[i];
                if (token.StartsWith("{{"))
                {
                    if (token != expectedTags[index])
                    {
                        if (expectedTags[index] == "{{/gr}}" && token == "{{gc}}")
                        {
                            index = 2;
                            continue;
                        }

                        throw new InvalidOperationException(
                            string.Format(
                                "Invalid grid detected, expecting '{0}', found '{1}'",
                                expectedTags[index],
                                token));
                    }

                    index = (index + 1) % expectedTags.Count;
                }
                else
                {
                    if (expectedTags[index] != "{{/gc}}")
                    {
                        throw new InvalidOperationException(
                            string.Format(
                                "Grid text outside column, text '{0}'",
                                token));
                    }
                }
            }

            string gridString = string.Concat(grid);

            // Remove empty columns
            gridString = Regex.Replace(
                input: gridString,
                pattern: "{{gc}} *{{/gc}}",
                replacement: string.Empty,
                options: RegexOptions.IgnoreCase);

            // Remove empty rows
            gridString = Regex.Replace(
                input: gridString,
                pattern: "{{gr}} *{{/gr}}",
                replacement: string.Empty,
                options: RegexOptions.IgnoreCase);

            return gridString;
        }
    }
}
