using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VisualBoxManager.ViewModels.Home;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemPage : ContentPage
    {
        private IConnectionService _connectionService;

        public ItemPage(IConnectionService connectionService, Move move)
        {
            InitializeComponent();
            _connectionService = connectionService;
            BindingContext = new ItemPageViewModel(connectionService, move.id);
        }
    }
}
