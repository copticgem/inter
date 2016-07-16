using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Helpers
{
    public static class PageTransition
    {
        public static async Task PushModalAsync(Page page, bool animated = true)
        {
            NavigationPage navPage = new NavigationPage(page)
            {
                BarBackgroundColor = ColorManager.Backgrounds.NavigationBar,
                BarTextColor = ColorManager.Text.NavigationBar
            };

            await App.Navigation.PushModalAsync(navPage, animated);
        }

        public static async Task PopModalAsync(bool animated)
        {
            await App.Navigation.PopModalAsync(animated);
        }
    }
}
