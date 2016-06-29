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
            Dictionary<int, Label> verses = new Dictionary<int, Label>();

            List<Label> labels = new List<Label>();

            string[] tokens = Regex.Split(
                input: content,
                pattern: @"({{/*\w+}})");

            Label openLabel = null;
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
                        int endIndex = token.IndexOf("}");
                        int verseNumber = int.Parse(token.Substring(3, endIndex - 3));
                        Label verseLabel = CreateLabel(StringType.Verse);
                        labels.Add(verseLabel);
                        if (!verses.ContainsKey(verseNumber))
                        {
                            verses.Add(verseNumber, verseLabel);
                        }
                    }
                    else if (token == "{{t}}")
                    {
                        openLabel = CreateLabel(StringType.Subtitle);
                    }
                    else if (token == "{{/t}}")
                    {
                        labels.Add(openLabel);
                        openLabel = null;
                    }
                    else if (token == "{{b}}")
                    {
                        openLabel = CreateLabel(StringType.BoldText);
                    }
                    else if (token == "{{/b}}")
                    {
                        MergeBoldText(openLabel, labels);
                        openLabel = null;
                    }
                    else if (token == "{{l}}")
                    {
                        labels.Add(CreateLabel(StringType.NewLine));
                    }
                    else if (token == "{{p}}")
                    {
                        labels.Add(CreateLabel(StringType.NewParagraph));
                    }
                    else if (token == "{{d}}")
                    {
                        labels.Add(CreateLabel(StringType.Divider));
                    }
                }
                else
                {
                    if (openLabel != null)
                    {
                        // It's part of an open tag, add it.
                        openLabel.Text = token;
                    }
                    else
                    {
                        Label lastLabel = labels.LastOrDefault();
                        if (lastLabel != null && lastLabel.FormattedText != null)
                        {
                            // Last label was bold, this needs to be on the same line
                            Span span = CreateSpan(CreateLabel(StringType.Text));
                            span.Text = token;
                            lastLabel.FormattedText.Spans.Add(span);
                        }
                        else
                        {
                            Label textLabel = CreateLabel(StringType.Text);
                            textLabel.Text = token;
                            labels.Add(textLabel);
                        }
                    }
                }
            }

            return labels;
        }

        private static Label CreateLabel(StringType type)
        {
            Label label = new Label
            {
                HorizontalTextAlignment = TextAlignment.End
            };

            switch (type)
            {
                case StringType.Verse:
                    label.IsVisible = false;
                    break;
                case StringType.Subtitle:
                    label.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                    break;
                case StringType.NewLine:
                    break;
                case StringType.NewParagraph:
                    // label.Text = "";
                    break;
                case StringType.Divider:
                    label.Text = "\r\n-------\r\n";
                    break;
                case StringType.BoldText:
                    label.FontAttributes = FontAttributes.Bold;
                    break;
                case StringType.Text:
                    break;
                default:
                    throw new InvalidOperationException("unsupported type: " + type);
            }

            return label;
        }

        private static void MergeBoldText(Label boldLabel, List<Label> labels)
        {
            if (!labels.Any())
            {
                labels.Add(boldLabel);
            }

            Span boldSpan = CreateSpan(boldLabel);
            Label lastLabel = labels.Last();
            if (lastLabel.FormattedText != null)
            {
                // it already has spans
                lastLabel.FormattedText.Spans.Add(boldSpan);
            }
            else
            {
                // Convert last text to span
                labels.Remove(lastLabel);
                Label label = CreateLabel(StringType.Text);
                label.FormattedText = new FormattedString();
                label.FormattedText.Spans.Add(CreateSpan(lastLabel));
                label.FormattedText.Spans.Add(boldSpan);
                labels.Add(label);
            }
        }

        private static Span CreateSpan(Label label)
        {
            return new Span
            {
                Text = label.Text,
                FontAttributes = label.FontAttributes,
                BackgroundColor = label.BackgroundColor,
                FontFamily = label.FontFamily,
                FontSize = label.FontSize,
                ForegroundColor = label.TextColor
            };
        }
    }
}
