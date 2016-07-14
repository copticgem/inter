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
        public BasePage()
        {
            this.BackgroundColor = ColorManager.Backgrounds.Default;
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
