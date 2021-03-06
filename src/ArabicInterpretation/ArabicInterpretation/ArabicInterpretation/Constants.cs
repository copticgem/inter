﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation
{
    static class Constants
    {
        public const int DefaultPadding = 10;
        public const int ReadingPadding = 5;
        public const int DefaultBorderWidth = 2;

        public static class AuthorNames
        {
            public static string FrTadros = "القمص تادرس يعقوب";
            public static string FrAntonios = "القس أنطونيوس فكري";
        }

        public static class DataKeyNames
        {
            public static string Author = "Author";
        }

        public static class Properties
        {
            public static string LastPosition = "Last";
            public static string FontSize = "FontSize";
            public static string BackgroundColor = "Background";
            public static string OnDurationInMinutes = "OnDurationInMinutes";
        }
    }
}
