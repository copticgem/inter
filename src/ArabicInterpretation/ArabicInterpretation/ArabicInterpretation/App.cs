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
        
        public static Image LoadingImage { get; private set; }

        private ReadingPage readingPage;

        public App()
        {
            ColorManager.Initialize();
            SettingsManager.Initialize();

            LoadingImage = new Image
            {
                Source = ImageSource.FromResource("ArabicInterpretation.Resources.loading.png"),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            this.readingPage = new ReadingPage();
            this.MainPage = new NavigationPage(readingPage)
            {
                BarBackgroundColor = ColorManager.Backgrounds.NavigationBar,
                BarTextColor = ColorManager.Text.NavigationBar
            };

            Navigation = this.MainPage.Navigation;

            double x;
            double y;
            ReadingInfo readingInfo = ReadingPositionManager.GetLastPosition(out x, out y);

            readingPage.Initialize(readingInfo, x, y, true).Wait();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override async void OnSleep()
        {
            await this.readingPage.SaveLastPosition();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
