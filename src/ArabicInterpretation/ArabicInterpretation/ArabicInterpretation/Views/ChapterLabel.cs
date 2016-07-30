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
    public class ChapterLabel : Button, IDisposable
    {
        EventHandler handler;

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

            this.handler = async (sender, e) =>
            {
                await SynchronizationHelper.ExecuteOnce(this.OnClicked());
            };

            this.Clicked += this.handler;
        }

        public Task Initialize(
            ReadingInfo readingInfo, 
            int chaptersCount,
            ReadingColor color)
        {
            this.BackgroundColor = color.SecondBarColor;

            this.UpdateText(readingInfo);

            this.readingInfo = readingInfo;
            this.chaptersCount = chaptersCount;

            return Task.FromResult(true);
        }

        public async Task OnClicked()
        {
            ChapterChooserPage chapterChooserPage = new ChapterChooserPage();

            await PageTransition.PushModalAsync(chapterChooserPage);

            await chapterChooserPage.Initialize(
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

        public void Dispose()
        {
            this.Clicked -= this.handler;
        }
    }
}
