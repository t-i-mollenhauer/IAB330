using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager.views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scanner : ContentPage
    {
        private IConnectionService _connectionService;
        private Move _move;

        public Scanner(IConnectionService connectionService, Move move)
        {
            InitializeComponent();
            _connectionService = connectionService;
            this._move = move;
        }
    }
}
