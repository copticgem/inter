using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter.Formatters
{
    class SpacingFormatter
    {
        public static string FormatSpaces(string page)
        {
            // Remove empty {{b}}
            page = Regex.Replace(
                input: page,
                pattern: "{{b}} *{{/b}}",
                replacement: string.Empty);

            // Remove duplicate {{p}}
            page = Regex.Replace(
                input: page,
                pattern: "({{p}} *)+",
                replacement: "{{p}}");

            // Remove duplicate spaces
            char unicodeSpace = (char)160;
            page = Regex.Replace(
                input: page,
                pattern: string.Format("( |{0})+", unicodeSpace),
                replacement: " ");

            return page;
        }
    }
}
