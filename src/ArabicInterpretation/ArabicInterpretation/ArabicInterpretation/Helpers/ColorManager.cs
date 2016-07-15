using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public static class ColorManager
    {
        public static void Initialize()
        {
            SetNormalModeColors();
        }

        private static void SetNormalModeColors()
        {
            Backgrounds.Default = Color.White;

            // Grey
            Backgrounds.AuthorBar = Color.FromRgb(233, 234, 235);
            Backgrounds.BookChapterBar = Color.FromRgb(249, 249, 249);
            Backgrounds.Button = Color.Transparent;

            // Blue
            Text.Author = Color.Blue;
            Text.BookChapter = Color.Blue;
            Text.Default = Color.Black;

            // Yellow
            Text.SelectedButton = Color.FromRgb(242, 231, 0);

            Border.Default = Color.Black;
            Border.Grid = Color.Black;
        }

        public static class Backgrounds
        {
            public static Color Default;
            public static Color AuthorBar;
            public static Color BookChapterBar;
            public static Color Button;
        }

        public static class Text
        {
            public static Color Default;
            public static Color Author;
            public static Color BookChapter;
            public static Color SelectedButton;
        }

        public static class Border
        {
            public static Color Default;
            public static Color Grid;
        }

        public static Button CreateButton()
        {
            return new Button
            {
                TextColor = ColorManager.Text.Default,
                BackgroundColor = ColorManager.Backgrounds.Button,
                BorderColor = ColorManager.Border.Default,
                BorderWidth = 2
            };
        }
    }
}
