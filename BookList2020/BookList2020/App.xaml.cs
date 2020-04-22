using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
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
            MainPage = new BookList();
        }

        protected override void OnStart()
        {
            base.OnStart();
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }
        void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                MainPage.DisplayAlert("ALARM", "Du har simpelthen ingen net #poorfag få net asap.", "Okay boss");
            }
        }


        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
