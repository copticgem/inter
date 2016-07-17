using ArabicInterpretation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    class LoadingPage : BasePage
    {
        public LoadingPage()
            : base(null)
        {
            ActivityIndicator indicator = new ActivityIndicator
            {
                Color = ColorManager.Theme,
                IsRunning = true
            };

            StackLayout layout = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            layout.Children.Add(indicator);

            this.Content = layout;
        }
    }
}
