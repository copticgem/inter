using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formatter
{
    public static class Constants
    {
        public const string Divider = "<img border=\"0\" src=\"../../../../Pix/Priests/divider-3.jpg\" class=\"img-responsive";
        public const string DividerSuffix = "</b></p>\n\t";

        public const string Paragraph = "<p class=\"MsoNormal\" dir=\"RTL\" style=\"text-align:justify;direction:rtl;unicode-bidi:embed\">";

        public const string BoldStart = "<b>";
        public const string BoldEnd = "</b>";

        public static class Replacements
        {
            public const string Paragraph = "{{p}}";
            public const string BoldStart = "{{b}}";
            public const string BoldEnd = "{{/b}}";
        }

        public static class Authors
        {
            public const string FrAntonious = "Father-Antonious-Fekry";
            public const string FrTadros = "Father-Tadros-Yacoub-Malaty";
        }
    }
}
