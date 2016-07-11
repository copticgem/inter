using ArabicInterpretation.Helpers;
using ArabicInterpretation.Pages;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class HomePage : ContentPage
    {
        public HomePage()
        {
            BookChooserPage page = new BookChooserPage(Author.FrAntonios, true);
            this.Navigation.PushAsync(page).Wait();
        }
    }
}
