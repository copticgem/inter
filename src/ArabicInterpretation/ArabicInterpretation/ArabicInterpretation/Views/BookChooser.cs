using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class BookChooser : StackLayout
    {
        bool isNT = false;

        ScrollView ntScrollView;
        ScrollView otScrollView;

        Button ntButton;
        Button otButton;

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

            ntButton.Clicked += (sender, e) =>
            {
                this.OnTestamentSwitchClicked(true);
            };
            
            otButton.Clicked += (sender, e) =>
            {
                this.SetSelectedButton(otButton);
                this.SetUnselectedButton(ntButton);
                this.OnTestamentSwitchClicked(false);
            };

            testamentSwitch.Children.Add(ntButton);
            testamentSwitch.Children.Add(otButton);

            this.Orientation = StackOrientation.Vertical;
            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.Children.Add(testamentSwitch);

            BooksGrid ntGrid = new BooksGrid(true);
            this.ntScrollView = new ScrollView
            {
                Content = ntGrid,
                IsVisible = false,
            };

            BooksGrid otGrid = new BooksGrid(false);
            this.otScrollView = new ScrollView
            {
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
    }
}
