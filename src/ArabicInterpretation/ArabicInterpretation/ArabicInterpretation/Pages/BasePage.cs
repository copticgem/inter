using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class BasePage : ContentPage
    {
        public BasePage(string title)
        {
            this.BackgroundColor = ColorManager.Backgrounds.Default;

            if (title != null)
            {
                NavigationPage.SetHasNavigationBar(this, true);
                this.Title = title;
            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, false);
            }
        }
    }
}
