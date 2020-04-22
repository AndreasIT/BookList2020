using Plugin.Connectivity;
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
    public partial class CreateNewBookPage : ContentPage
    {
        private BookDataManager _bdm;
        public CreateNewBookPage()
        {
            _bdm = new BookDataManager();
            InitializeComponent();
        }

        private async void Submit_btn_Clicked(object sender, EventArgs e)
        {
                var book = new Book(ent_ISBN.Text, ent_title.Text);
           await _bdm.CreateBook(book);
           await Navigation.PopModalAsync();
            
            
        }
    }
}