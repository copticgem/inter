using ArabicInterpretation.Helpers;
using ArabicInterpretation.Views;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Pages
{
    public class AuthorChooserPage : ContentPage
    {
        public AuthorChooserPage(bool isNT, int bookNumber)
        {
            this.Content = new AuthorsGrid(isNT, bookNumber);
        }
    }
}
