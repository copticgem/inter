using ArabicInterpretation.Helpers;
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
            BookChooser bookChooser = new BookChooser(Author.FrTadros);
            bookChooser.Initialize(false).Wait();
            this.Content = bookChooser;
        }
    }
}
