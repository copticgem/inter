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
            // Set author
            AuthorManager.SetCurrentAuthor(Author.FrAntonios);

            BookChooserPage page = new BookChooserPage(true);
            this.Navigation.PushAsync(page).Wait();
        }
    }
}
