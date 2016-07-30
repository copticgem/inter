using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class BookChooser : StackLayout, IDisposable
    {
        bool isNT = false;

        ScrollView ntScrollView;
        ScrollView otScrollView;

        BooksGrid ntGrid;
        BooksGrid otGrid;

        Button ntButton;
        EventHandler ntButtonHandler;

        Button otButton;
        EventHandler otButtonHandler;

        public BookChooser()
        {
            StackLayout testamentSwitch = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            this.ntButton = ColorManager.CreateButton();
            ntButton.Text = "العهد الجديد";
            ntButton.BorderColor = ColorManager.Border.Button;

            this.otButton = ColorManager.CreateButton();
            otButton.Text = "العهد القديم";
            otButton.BorderColor = ColorManager.Border.Button;
            this.ntButtonHandler = (sender, e) =>
            {
                this.OnTestamentSwitchClicked(true);
            };

            ntButton.Clicked += this.ntButtonHandler;
            
            this.otButtonHandler = (sender, e) =>
            {
                this.SetSelectedButton(otButton);
                this.SetUnselectedButton(ntButton);
                this.OnTestamentSwitchClicked(false);
            };

            otButton.Clicked += this.otButtonHandler;

            testamentSwitch.Children.Add(ntButton);
            testamentSwitch.Children.Add(otButton);

            this.Orientation = StackOrientation.Vertical;
            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.Children.Add(testamentSwitch);

            this.ntGrid = new BooksGrid(true);
            this.ntScrollView = new ScrollView
            {
                Padding = Constants.DefaultPadding,
                Content = ntGrid,
                IsVisible = false,
            };

            this.otGrid = new BooksGrid(false);
            this.otScrollView = new ScrollView
            {
                Padding = Constants.DefaultPadding,
                Content = otGrid,
                IsVisible = false
            };

            this.Children.Add(this.ntScrollView);
            this.Children.Add(this.otScrollView);
        }

        public async Task Initialize(
            Author author,
            bool isNT, 
            int currentBookNumber)
        {
            // TODO: Load only the first one
            BooksGrid otGrid = (BooksGrid)this.otScrollView.Content;
            BooksGrid ntGrid = (BooksGrid)this.ntScrollView.Content;

            // Set selected book
            if (isNT)
            {
                await otGrid.Initialize(author, -1);
                await ntGrid.Initialize(author, currentBookNumber);
            }
            else
            {
                await otGrid.Initialize(author, currentBookNumber);
                await ntGrid.Initialize(author, -1);
            }

            this.OnTestamentSwitchClicked(isNT);
        }

        private void OnTestamentSwitchClicked(bool isNTClicked)
        {
            this.isNT = isNTClicked;
            if (isNTClicked)
            {
                this.otScrollView.IsVisible = false;
                this.ntScrollView.IsVisible = true;

                this.SetSelectedButton(ntButton);
                this.SetUnselectedButton(otButton);
            }
            else
            {
                this.ntScrollView.IsVisible = false;
                this.otScrollView.IsVisible = true;

                this.SetSelectedButton(otButton);
                this.SetUnselectedButton(ntButton);
            }
        }

        private void SetSelectedButton(Button button)
        {
            button.BackgroundColor = ColorManager.Backgrounds.SelectedTestament;
            button.TextColor = ColorManager.Text.SelectedTestament;
        }

        private void SetUnselectedButton(Button button)
        {
            button.BackgroundColor = ColorManager.Backgrounds.Default;
            button.TextColor = ColorManager.Text.UnSelectedTestament;
        }

        public void Dispose()
        {
            this.ntButton.Clicked -= this.ntButtonHandler;
            this.otButton.Clicked -= this.otButtonHandler;
            this.ntGrid.Dispose();
            this.otGrid.Dispose();
        }
    }
}
