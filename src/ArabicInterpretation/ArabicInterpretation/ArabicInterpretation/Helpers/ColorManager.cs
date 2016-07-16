﻿using System;
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
            Color testamentBlue = Color.FromRgb(55, 141, 204);
            Backgrounds.Default = Color.White;

            // Grey
            Backgrounds.AuthorBar = Color.FromRgb(233, 234, 235);
            Backgrounds.BookChapterBar = Color.FromRgb(249, 249, 249);
            Backgrounds.Button = Color.Transparent;
            Backgrounds.DisabledButton = Color.Gray;
            Backgrounds.SelectedTestament = testamentBlue;
            Backgrounds.NavigationBar = testamentBlue;

            Text.Author = testamentBlue;
            Text.BookChapter = testamentBlue;
            Text.Default = Color.Black;
            Text.SelectedTestament = Color.White;
            Text.UnSelectedTestament = testamentBlue;
            Text.NavigationBar = testamentBlue;

            // Yellow
            Text.SelectedButton = Color.FromRgb(204, 168, 0);

            Border.Default = Color.Black;
            Border.Grid = Color.Black;
            Border.Button = testamentBlue;
        }

        public static class Backgrounds
        {
            public static Color Default;
            public static Color NavigationBar;
            public static Color AuthorBar;
            public static Color BookChapterBar;
            public static Color Button;
            public static Color DisabledButton;
            public static Color SelectedTestament;
        }

        public static class Text
        {
            public static Color Default;
            public static Color NavigationBar;
            public static Color Author;
            public static Color BookChapter;
            public static Color SelectedButton;
            public static Color SelectedTestament;
            public static Color UnSelectedTestament;
        }

        public static class Border
        {
            public static Color Default;
            public static Color Grid;
            public static Color Button;
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

        public static void DisableButton(Button button)
        {
            button.BackgroundColor = Backgrounds.DisabledButton;
            button.IsEnabled = false;
        }

        public static void EnableButton(Button button)
        {
            button.BackgroundColor = Backgrounds.Button;
            button.IsEnabled = true;
        }
    }
}