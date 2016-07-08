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
        BooksGrid ntGrid = null;
        BooksGrid otGrid = null;

        public BookChooser()
        {
            StackLayout testamentSwitch = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            Button ntButton = new Button
            {
                Text = "العهد الجديد",
            };

            ntButton.Clicked += (sender, e) =>
            {
                this.OnTestamentSwitchClicked(true);
            };

            Button otButton = new Button
            {
                Text = "العهد القديم"
            };

            otButton.Clicked += (sender, e) =>
            {
                this.OnTestamentSwitchClicked(false);
            };

            testamentSwitch.Children.Add(ntButton);
            testamentSwitch.Children.Add(otButton);

            this.Orientation = StackOrientation.Vertical;
            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.Children.Add(testamentSwitch);

            this.ntGrid = new BooksGrid(true);
            this.ntGrid.IsVisible = false;
            ScrollView ntScrollView = new ScrollView
            {
                Content = this.ntGrid
            };

            this.otGrid = new BooksGrid(false);
            this.otGrid.IsVisible = false;
            ScrollView otScrollView = new ScrollView
            {
                Content = this.otGrid
            };

            this.Children.Add(ntScrollView);
            this.Children.Add(otScrollView);
        }

        public async Task Initialize(bool isNT)
        {
            // TODO: Load only the first one
            await this.otGrid.LoadBooks();
            await this.ntGrid.LoadBooks();

            this.OnTestamentSwitchClicked(isNT);
        }

        private void OnTestamentSwitchClicked(bool isNTClicked)
        {
            this.isNT = isNTClicked;
            if (isNTClicked)
            {
                this.otGrid.IsVisible = false;
                this.ntGrid.IsVisible = true;
            }
            else
            {
                this.ntGrid.IsVisible = false;
                this.otGrid.IsVisible = true;
            }
        }
    }
}
