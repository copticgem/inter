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
    public class ChapterChooserPage : ContentPage
    {
        public ChapterChooserPage(
            Author author,
            bool isNT,
            int bookNumber,
            int chaptersCount)
        {
            this.Content = new ScrollView
            {
                Content = new ChaptersGrid(author, isNT, bookNumber, chaptersCount)
            };
        }
    }
}
