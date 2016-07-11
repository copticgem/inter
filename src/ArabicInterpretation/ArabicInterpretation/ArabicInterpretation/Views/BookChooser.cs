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

            BooksGrid ntGrid = new BooksGrid(true);
            this.ntScrollView = new ScrollView
            {
                Content = ntGrid,
                IsVisible = false
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
        
        public async Task Initialize(bool isNT)
        {
            // TODO: Load only the first one
            await ((BooksGrid)this.otScrollView.Content).LoadBooks();
            await ((BooksGrid)this.ntScrollView.Content).LoadBooks();

            this.OnTestamentSwitchClicked(isNT);
        }

        private void OnTestamentSwitchClicked(bool isNTClicked)
        {
            this.isNT = isNTClicked;
            if (isNTClicked)
            {
                this.otScrollView.IsVisible = false;
                this.ntScrollView.IsVisible = true;
            }
            else
            {
                this.ntScrollView.IsVisible = false;
                this.otScrollView.IsVisible = true;
            }
        }
    }
}
