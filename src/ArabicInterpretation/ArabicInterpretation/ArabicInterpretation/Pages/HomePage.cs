using ArabicInterpretation.Helpers;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class HomePage : ContentPage
    {
        ScrollView scrollView;
        Dictionary<int, Label> verses;

        public HomePage()
        {
            BookChooser bookChooser = new BookChooser(Author.FrAntonios);
            bookChooser.Initialize(false).Wait();
            this.Content = bookChooser;
        }
    }
}
