using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Helpers
{
    static class ContentFormatter
    {
        public static List<Label> FormatContent(string content)
        {
            List<Label> labels = new List<Label>();

            string[] tokens = Regex.Split(
                input: content,
                pattern: @"({{/*\w+}})");

            StringBuilder builder = new StringBuilder();

            FormattedString formattedString = new FormattedString();
            Label label = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                FormattedText = formattedString
            };

            labels.Add(label);

            Span span = new Span();

            foreach (string token in tokens)
            {
                if (token == string.Empty)
                {
                    continue;
                }
                else if (token.StartsWith("{{"))
                {
                    if (token.StartsWith("{{v"))
                    {
                        // TODO
                        continue;
                    }
                    else if (token == "{{t}}")
                    {
                        span.Text = builder.ToString();
                        formattedString.Spans.Add(span);

                        builder = new StringBuilder();
                        builder.Append("\r\n");
                        span = new Span
                        {
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
                        };
                    }
                    else if (token == "{{/t}}")
                    {
                        span.Text = builder.ToString();
                        formattedString.Spans.Add(span);

                        builder = new StringBuilder();
                        span = new Span();
                    }
                    else if (token == "{{b}}")
                    {
                        span.Text = builder.ToString();
                        formattedString.Spans.Add(span);

                        builder = new StringBuilder();
                        span = new Span
                        {
                            FontAttributes = FontAttributes.Bold
                        };
                    }
                    else if (token == "{{/b}}")
                    {
                        span.Text = builder.ToString();
                        formattedString.Spans.Add(span);

                        builder = new StringBuilder();
                        span = new Span();
                    }
                    else if (token == "{{l}}")
                    {
                        builder.Append("\r\n");
                    }
                    else if (token == "{{p}}")
                    {
                        builder.Append("\r\n\r\n");
                    }
                    else if (token == "{{d}}")
                    {
                        builder.Append("\r\n-------\r\n");
                    }
                }
                else
                {
                    builder.Append(token);
                }
            }

            return labels;
        }
    }
}
