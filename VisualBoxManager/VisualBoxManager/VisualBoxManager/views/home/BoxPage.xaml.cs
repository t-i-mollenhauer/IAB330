using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VisualBoxManager.ViewModels.Home;
using System.Collections.Specialized;
using VisualBoxManager.ViewModels;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxPage : ContentPage
    {
        
        public BoxPage(IConnectionService connectionService, Move move)
        {
            InitializeComponent();
            BindingContext = new BoxPageViewModel(connectionService, move.id);
        }


        protected override void OnAppearing()
        {
            ((BoxPageViewModel)BindingContext).Refresh();

        }

        private void Delete_OnClicked(object sender, EventArgs e)
        {
            var x = ((MenuItem)sender).CommandParameter as Box;
            ((BoxPageViewModel)BindingContext).DeleteBox(x.id);
        }

        private void Edit_OnClicked(object sender, EventArgs e)
        {
            var x = ((MenuItem)sender).CommandParameter as Box;
            ((BoxPageViewModel)BindingContext).EditBox(x);
        }

    }
}
