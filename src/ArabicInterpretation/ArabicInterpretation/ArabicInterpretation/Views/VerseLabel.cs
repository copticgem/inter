﻿using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Pages;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    public class VerseLabel : Button
    {
        VerseChooserPage verseChooserPage;

        public VerseLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.Text = "آية";

            this.HorizontalOptions = LayoutOptions.End;
            this.VerticalOptions = LayoutOptions.Start;

            this.BorderWidth = 0;
            this.BackgroundColor = Color.Transparent;

            this.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.verseChooserPage = new VerseChooserPage();
        }

        public async Task Initialize(Dictionary<int, Grid> verses)
        {
            if (!verses.Any())
            {
                ColorManager.DisableButton(this);
            }
            else
            {
                ColorManager.EnableButton(this);
                await this.verseChooserPage.Initialize(verses);

            }
        }

        public async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.verseChooserPage);
        }
    }
}
