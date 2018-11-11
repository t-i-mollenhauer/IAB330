using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage
    {
        private IConnectionService _connectionService;
        private Move _move;

        public ItemPage(IConnectionService connectionService, Move move)
        {
            InitializeComponent();
            _connectionService = connectionService;
            this._move = move;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        protected async override void OnAppearing()
        {
            // GetMoves should return Task<List<Move>> but it was difficult to mock a Task, should be fixed when ConnectionService is actualy connected
            //listView.ItemsSource = _connectionService.GetItems(_move);
        }
    }
}
