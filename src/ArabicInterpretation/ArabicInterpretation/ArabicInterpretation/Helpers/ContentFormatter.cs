using ArabicInterpretation.Model;
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
        public static List<View> FormatContent(
            string content,
            ReadingColor color,
            out Dictionary<int, Grid> verses)
        {
            verses = new Dictionary<int, Grid>();
            List<View> views = new List<View>();

            NamedSize fontSize = SettingsManager.ToNamedSize(SettingsManager.GetFontSize());

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

                        Grid grid;

                        Grid lastGrid = views.LastOrDefault() as Grid;
                        if (lastGrid != null && lastGrid.ClassId == "v")
                        {
                            // Use same object for consecutive verses to avoid multiple spaces in content
                            grid = lastGrid;
                        }
                        else
                        {
                            grid = new Grid
                            {
                                ClassId = "v"
                            };

                            views.Add(grid);
                        }

                        if (!verses.ContainsKey(verseNumber))
                        {
                            verses.Add(verseNumber, grid);
                        }
                    }
                    else if (token == "{{t}}")
                    {
                        openLabel = CreateLabel(StringType.Subtitle, fontSize, color);
                    }
                    else if (token == "{{/t}}")
                    {
                        views.Add(openLabel);
                        openLabel = null;
                    }
                    else if (token == "{{b}}")
                    {
                        openLabel = CreateLabel(StringType.BoldText, fontSize, color);
                    }
                    else if (token == "{{/b}}")
                    {
                        MergeBoldText(openLabel, views, fontSize, color);
                        openLabel = null;
                    }
                    else if (token == "{{l}}")
                    {
                        views.Add(CreateLabel(StringType.NewLine, fontSize, color));
                    }
                    else if (token == "{{p}}")
                    {
                        views.Add(CreateLabel(StringType.NewParagraph, fontSize, color));
                    }
                    else if (token == "{{d}}")
                    {
                        views.Add(new BoxView() { Color = color.TextColor, HeightRequest = 1 });
                    }
                    else if (token == "{{g}}" || token == "{{gltr}}")
                    {
                        int gridEndIndex;
                        views.Add(GetGrid(
                            tokens.ToList(),
                            i,
                            fontSize,
                            color,
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
                            Span span = CreateSpan(CreateLabel(StringType.Text, fontSize, color));
                            span.Text = token;
                            lastLabel.FormattedText.Spans.Add(span);
                        }
                        else
                        {
                            Label textLabel = CreateLabel(StringType.Text, fontSize, color);
                            textLabel.Text = token;
                            views.Add(textLabel);
                        }
                    }
                }
            }

            return views;
        }

        private static void MergeVerses(List<int> openVerses, Label label, Dictionary<int, Label> verses)
        {
            openVerses.ForEach(verseNumber =>
            {
                if (!verses.ContainsKey(verseNumber))
                {
                    verses.Add(verseNumber, label);
                }
            });
        }

        private static Label CreateLabel(
            StringType type,
            NamedSize fontSize,
            ReadingColor color)
        {
            Label label = new Label
            {
                FontSize = Device.GetNamedSize(fontSize, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = color.TextColor,
                BackgroundColor = color.BackgroundColor
            };

            switch (type)
            {
                case StringType.Verse:
                    label.Text = string.Empty;
                    break;
                case StringType.Subtitle:
                    label.FontAttributes = FontAttributes.Bold;
                    break;
                case StringType.NewLine:
                    break;
                case StringType.NewParagraph:
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

        private static void MergeBoldText(
            Label boldLabel,
            List<View> views,
            NamedSize fontSize,
            ReadingColor color)
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
                Label label = CreateLabel(StringType.Text, fontSize, color);
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
                Label label = CreateLabel(StringType.Text, fontSize, color);
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
                ForegroundColor = label.TextColor,
            };
        }

        private static Grid GetGrid(
            List<string> tokens,
            int gridStartIndex,
            NamedSize fontSize,
            ReadingColor color,
            out int gridEndIndex)
        {
            bool isReversedGrid = false;
            if (tokens[gridStartIndex] == "{{gltr}}")
            {
                isReversedGrid = true;
            }

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

            // TODO: Add grid border once we have color modes
            Grid grid = new Grid
            {
                ColumnSpacing = 1,
                RowSpacing = 1,
                BackgroundColor = color.TextColor
            };

            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int i = 0; i < columnCount; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            foreach (var tuple in gridTuples)
            {
                Label label = CreateLabel(StringType.Text, fontSize, color);
                label.Text = tuple.Item1;

                // This a workaround since there is a bug with grid that it truncates the text
                StackLayout stack = new StackLayout
                {
                    BackgroundColor = color.BackgroundColor
                };

                stack.Children.Add(label);

                if (gridTuples.Where(t => t.Item2 == tuple.Item2).Count() == 1)
                {
                    // Only one, span all columns
                    grid.Children.Add(label, 0, tuple.Item2);
                    Grid.SetColumnSpan(label, columnCount + 1);
                }
                else
                {
                    int left = isReversedGrid ? tuple.Item3 : columnCount - tuple.Item3;
                    grid.Children.Add(stack, left, tuple.Item2);
                }
            }

            return grid;
        }
    }
}
