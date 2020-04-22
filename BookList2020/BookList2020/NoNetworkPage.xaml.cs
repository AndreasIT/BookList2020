using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BookList2020
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoNetworkPage : ContentPage
    {
        public NoNetworkPage()
        {
            InitializeComponent();
            NetworkStatus.Text = "No Network Connection Available";
        }
    }
}