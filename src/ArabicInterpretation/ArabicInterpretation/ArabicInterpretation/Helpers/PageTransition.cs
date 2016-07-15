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
            NavigationPage navPage = new NavigationPage(page);
            await App.Navigation.PushModalAsync(navPage, animated);
        }
    }
}
