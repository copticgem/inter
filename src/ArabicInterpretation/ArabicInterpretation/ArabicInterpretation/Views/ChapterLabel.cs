using ArabicInterpretation.Helpers;
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
        ReadingInfo readingInfo;
        int chaptersCount;

        public ChapterLabel()
        {
            this.TextColor = ColorManager.Text.BookChapter;
            this.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

            this.HorizontalOptions = LayoutOptions.StartAndExpand;

            this.BorderRadius = 1;
            this.BorderWidth = Constants.DefaultBorderWidth;
            this.BorderColor = ColorManager.Text.BookChapter;

            this.Clicked += async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };
        }

        public async Task Initialize(
            ReadingInfo readingInfo, 
            int chaptersCount,
            ReadingColor color)
        {
            this.BackgroundColor = color.SecondBarColor;

            this.UpdateText(readingInfo);

            this.readingInfo = readingInfo;
            this.chaptersCount = chaptersCount;
        }

        public async Task OnClicked()
        {
            this.chapterChooserPage = new ChapterChooserPage();

            await PageTransition.PushModalAsync(this.chapterChooserPage);

            await this.chapterChooserPage.Initialize(
                shouldPopTwice: false,
                author: readingInfo.Author,
                isNT: readingInfo.IsNT,
                bookNumber: readingInfo.BookNumber,
                chaptersCount: chaptersCount);
        }

        private void UpdateText(ReadingInfo readingInfo)
        {
            if (readingInfo.ChapterNumber == 0)
            {
                this.Text = "مقدمة";
            }
            else
            {
                this.Text = DisplayNameHelper.GetChapterDisplayName(
                    readingInfo.IsNT,
                    readingInfo.BookNumber,
                    readingInfo.ChapterNumber);
            }
        }
    }
}
