using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.views.Home;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovePage : ContentPage
    {
        private IConnectionService _connectionService;

        public MovePage(IConnectionService connectionService)
        {
            InitializeComponent();
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            if (!(e.SelectedItem is Move move))
                throw new NotSupportedException();

            var tab = new TabbedPage() { Title = move.name };
            tab.Children.Add(new ItemPage(_connectionService, move) { Title = "Items" });
            tab.Children.Add(new BoxPage(_connectionService, move) { Title = "Boxes" });
            tab.Children.Add(new Scanner(_connectionService, move) { Title = "Scanner" });
            Navigation.PushAsync(tab);
        }

        private void OnCreateNew(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateMove(_connectionService));
        }

        protected override async void OnAppearing()
        {
            // GetMoves should return Task<List<Move>> but it was difficult to mock a Task, should be fixed when ConnectionService is actualy connected
            listView.ItemsSource = await _connectionService.GetMoves();
        }
    }
}
