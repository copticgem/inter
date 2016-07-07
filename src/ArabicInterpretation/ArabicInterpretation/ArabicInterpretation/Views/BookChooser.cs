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
        ScrollView ntBooks = null;
        ScrollView otBooks = null;

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

            ntButton.Clicked += async (sender, e) =>
            {
                await this.OnTestamentSwitchClicked(true);
            };

            Button otButton = new Button
            {
                Text = "العهد القديم"
            };

            otButton.Clicked += async (sender, e) =>
            {
                await this.OnTestamentSwitchClicked(false);
            };

            testamentSwitch.Children.Add(ntButton);
            testamentSwitch.Children.Add(otButton);

            this.Orientation = StackOrientation.Vertical;
            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.Children.Add(testamentSwitch);


            this.otBooks = new ScrollView
            {
                Content = new BooksGrid(isNT),
            };

            this.Children.Add(this.otBooks);
        }

        private async Task OnTestamentSwitchClicked(bool isNTClicked)
        {
            if (isNTClicked && !this.isNT)
            {
                this.otBooks.IsVisible = false;
                if (this.ntBooks == null)
                {
                    this.ntBooks = new ScrollView { Content = new BooksGrid(true) };
                    this.Children.Add(this.ntBooks);
                }

                this.ntBooks.IsVisible = true;
                this.isNT = true;
            }
            else if (!isNTClicked && this.isNT)
            {
                this.ntBooks.IsVisible = false;
                if (this.otBooks == null)
                {
                    this.otBooks = new ScrollView { Content = new BooksGrid(false) };
                    this.Children.Add(this.otBooks);
                }

                this.otBooks.IsVisible = true;
                this.isNT = false;
            }
        }
    }
}
