using ArabicInterpretation.Helpers;
using ArabicInterpretation.Model;
using ArabicInterpretation.Pages;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ArabicInterpretation
{
    public class App : Application
    {
        public static INavigation Navigation { get; private set; }

        public App()
        {
            ColorManager.Initialize();

            ReadingPage readingPage = new ReadingPage();
            this.MainPage = new NavigationPage(readingPage)
            {
                BarBackgroundColor = ColorManager.Backgrounds.NavigationBar,
                BarTextColor = ColorManager.Text.NavigationBar
            };

            Navigation = this.MainPage.Navigation;

            readingPage.Initialize(new ReadingInfo(
                Author.FrTadros,
                false,
                1,
                1),
                true).Wait();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
