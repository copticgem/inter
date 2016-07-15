using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Views
{
    public class VersesPanel : StackLayout
    {
        public VersesPanel()
        {
            Padding = 15;
            VerticalOptions = LayoutOptions.FillAndExpand;
            HorizontalOptions = LayoutOptions.EndAndExpand;
            BackgroundColor = Color.FromRgba(0, 0, 0, 180);

            Label title = new Label
            {
                Text = "ايات",
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White
            };

            Button firstOption = new Button
            {
                Text = "ايه 1"
            };

            this.Children.Add(title);
            this.Children.Add(firstOption);
        }
    }
}
