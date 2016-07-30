using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Pages;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Xamarin.Forms.Grid;

namespace ArabicInterpretation.Views
{
    public class ChaptersGrid : Grid, IDisposable
    {
        List<Tuple<Button, EventHandler>> handlers;

        private const int ButtonsPerRow = 5;

        bool shouldPopTwice;
        Author author;

        public ChaptersGrid()
        {
            this.HorizontalOptions = LayoutOptions.FillAndExpand;

            this.ColumnDefinitions = new ColumnDefinitionCollection();
            for (int i = 0; i < ButtonsPerRow; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            this.RowDefinitions = new RowDefinitionCollection();
            for (int i = 0; i < 20; i++)
            {
                this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            this.handlers = new List<Tuple<Button, EventHandler>>();
        }

        // TODO: Highlight selected chapter
        public Task Initialize(
            bool shouldPopTwice,
            Author author,
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            this.shouldPopTwice = shouldPopTwice;
            this.author = author;

            this.Children.Clear();

            // Add the introduction button
            Button introButton = ColorManager.CreateButton();
            introButton.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));
            introButton.Text = "مقدمة";
            EventHandler introButtonHandler = async (sender, e) =>
            {
                await this.OnChapterClicked_Safe(
                    isNT: isNT,
                    bookNumber: bookNumber,
                    chapterNumber: 0);
            };

            introButton.Clicked += introButtonHandler;
            this.handlers.Add(new Tuple<Button, EventHandler>(introButton, introButtonHandler));

            this.Children.Add(introButton, ButtonsPerRow - 1, 0);

            int left = ButtonsPerRow - 1;
            for (int i = 1; i <= chaptersCount; i++)
            {
                Button button = ColorManager.CreateButton();
                button.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button));

                int chapterNumber = i;

                button.Text = DisplayNameHelper.GetChapterDisplayName(isNT, bookNumber, chapterNumber);

                EventHandler handler = async (sender, e) =>
                {
                    await this.OnChapterClicked_Safe(
                        isNT: isNT,
                        bookNumber: bookNumber,
                        chapterNumber: chapterNumber);
                };

                button.Clicked += handler;

                this.handlers.Add(new Tuple<Button, EventHandler>(button, handler));

                int top = (i - 1) / ButtonsPerRow;
                this.Children.Add(button, left, top + 1);

                left--;
                if (left == -1)
                {
                    left = ButtonsPerRow - 1;
                }
            }

            return Task.FromResult(true);
        }

        public void UpdateAuthor(Author author)
        {
            this.author = author;
        }

        private async Task OnChapterClicked_Safe(
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            await SynchronizationHelper.ExecuteOnce(
                this.OnChapterClicked(
                    isNT,
                    bookNumber,
                    chapterNumber));
        }

        private async Task OnChapterClicked(
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            // Show loading screen in reading page
            MessagingCenter.Send(App.Navigation, ReadingPage.ShowLoadingMessage);

            ReadingInfo readingInfo = new ReadingInfo(
                this.author,
                isNT,
                bookNumber,
                chapterNumber);

            Task pop1 = PageTransition.PopModalAsync(true);

            // This flag means the call is coming from BooksChooser, so need to pop this page as well
            if (this.shouldPopTwice)
            {
                Task pop2 = PageTransition.PopModalAsync(false);
                await Task.WhenAll(new Task[] { pop1, pop2 });
            }
            else
            {
                await pop1;
            }

            MessagingCenter.Send(this, ReadingPage.ChapterChangedMessage, readingInfo);
        }

        public void Dispose()
        {
            this.handlers.ForEach(t => t.Item1.Clicked -= t.Item2);
        }
    }
}
