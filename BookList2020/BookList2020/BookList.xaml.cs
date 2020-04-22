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
    public partial class BookList : ContentPage
    {
        private BookDataManager _bdm;
        public BookList()
        {
            _bdm = new BookDataManager();
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();       
            var books = await _bdm.GetBooks();
            Books.ItemsSource = books;       
        }

        private void bt_CreateNewBook_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new NavigationPage(new CreateNewBookPage()));
         
        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
                var viewCell = (ViewCell)sender;
                var listView = (ListView)viewCell.Parent;
                var book = (Book)listView.SelectedItem;
                await _bdm.DeleteBook(book.id);
        }
    }
}