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
        public static List<View> FormatContent(string content)
        {
            Dictionary<int, Label> verses = new Dictionary<int, Label>();

            List<View> views = new List<View>();

            string[] tokens = Regex.Split(
                input: content,
                pattern: @"({{/*\w+}})");

            Label openLabel = null;
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
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
                        views.Add(verseLabel);
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
                        views.Add(openLabel);
                        openLabel = null;
                    }
                    else if (token == "{{b}}")
                    {
                        openLabel = CreateLabel(StringType.BoldText);
                    }
                    else if (token == "{{/b}}")
                    {
                        MergeBoldText(openLabel, views);
                        openLabel = null;
                    }
                    else if (token == "{{l}}")
                    {
                        views.Add(CreateLabel(StringType.NewLine));
                    }
                    else if (token == "{{p}}")
                    {
                        views.Add(CreateLabel(StringType.NewParagraph));
                    }
                    else if (token == "{{d}}")
                    {
                        views.Add(new BoxView() { Color = Color.White, HeightRequest = 1 });
                    }
                    else if (token == "{{g}}")
                    {
                        int gridEndIndex;
                        views.Add(GetGrid(
                            tokens.ToList(),
                            i,
                            out gridEndIndex));

                        // Skip the grid
                        i = gridEndIndex;
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
                        Label lastLabel = views.LastOrDefault() as Label;
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
                            views.Add(textLabel);
                        }
                    }
                }
            }

            return views;
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
                    label.ClassId = "p";
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

        private static void MergeBoldText(Label boldLabel, List<View> views)
        {
            if (!views.Any())
            {
                views.Add(boldLabel);
            }

            Span boldSpan = CreateSpan(boldLabel);
            Label lastLabel = views.Last() as Label;
            if (lastLabel == null || lastLabel.ClassId == "p")
            {
                // Not a label, add bold as separate label
                Label label = CreateLabel(StringType.Text);
                label.FormattedText = new FormattedString();
                label.FormattedText.Spans.Add(boldSpan);
                views.Add(label);
            }
            else if (lastLabel.FormattedText != null)
            {
                // it already has spans
                lastLabel.FormattedText.Spans.Add(boldSpan);
            }
            else
            {
                // Convert last text to span
                views.Remove(lastLabel);
                Label label = CreateLabel(StringType.Text);
                label.FormattedText = new FormattedString();
                label.FormattedText.Spans.Add(CreateSpan(lastLabel));
                label.FormattedText.Spans.Add(boldSpan);
                views.Add(label);
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

        private static Grid GetGrid(
            List<string> tokens,
            int gridStartIndex,
            out int gridEndIndex)
        {
            gridEndIndex = tokens.IndexOf("{{/g}}", gridStartIndex);
            if (gridEndIndex <= 0)
            {
                throw new InvalidOperationException("End grid not found!");
            }

            int rowCount = 0;
            int columnCount = 0;
            int rowIndex = -1;
            int columnIndex = -1;
            List<Tuple<string, int, int>> gridTuples = new List<Tuple<string, int, int>>();
            for (int i = gridStartIndex + 1; i < gridEndIndex; i++)
            {
                string token = tokens[i];
                if (token == string.Empty)
                {
                    continue;
                }

                if (token.StartsWith("{{"))
                {
                    if (token == "{{gr}}")
                    {
                        rowIndex++;
                        rowCount = Math.Max(rowCount, rowIndex);
                        columnIndex = -1;
                    }
                    else if (token == "{{gc}}")
                    {
                        columnIndex++;
                        columnCount = Math.Max(columnCount, columnIndex);
                    }
                }
                else
                {
                    gridTuples.Add(new Tuple<string, int, int>(token, rowIndex, columnIndex));
                }
            }

            Grid grid = new Grid { ColumnSpacing = 1, RowSpacing = 1 };
            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < columnCount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            foreach (var tuple in gridTuples)
            {
                Label label = CreateLabel(StringType.Text);
                label.Text = tuple.Item1;
                grid.Children.Add(label, columnCount - tuple.Item3, tuple.Item2);
            }

            return grid;
        }
    }
}
