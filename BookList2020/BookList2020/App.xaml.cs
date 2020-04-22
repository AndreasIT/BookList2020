using Plugin.Connectivity;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookList2020
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = CrossConnectivity.Current.IsConnected ? new BookList() : (Page)new NoNetworkPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
