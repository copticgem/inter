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
    public class ChapterLabel : Button
    {
        ChapterChooserPage chapterChooserPage;

        public ChapterLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button));

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;

            this.BorderWidth = 0;
            this.BackgroundColor = Color.Transparent;

            this.Clicked += async (sender, e) =>
            {
                await this.OnClicked();
            };

            this.chapterChooserPage = new ChapterChooserPage();
        }

        public async Task Initialize(ReadingInfo readingInfo, int chaptersCount)
        {
            this.UpdateText(readingInfo.ChapterNumber);

            await this.chapterChooserPage.Initialize(
                shouldPopTwice: false,
                author: readingInfo.Author,
                isNT: readingInfo.IsNT, 
                bookNumber: readingInfo.BookNumber, 
                chaptersCount: chaptersCount);
        }

        public async Task OnClicked()
        {
            await PageTransition.PushModalAsync(this.chapterChooserPage);
        }

        private void UpdateText(int chapterNumber)
        {
            if (chapterNumber == 0)
            {
                this.Text = "مقدمة";
            }
            else
            {
                this.Text = NumbersHelper.TranslateNumber(chapterNumber);
            }
        }
    }
}