using System;
using VisualBoxManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovePage : ContentPage
    {
        public MovePage(IConnectionService connectionService)
        {
            InitializeComponent();
            BindingContext = new MoveViewModel(connectionService);
            
        }
       
        //TODO: ask if this is a ok way of doing MVVM
        protected override void OnAppearing()
        {
            ((MoveViewModel)BindingContext).Refresh();

        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            var x = ((MenuItem)sender).CommandParameter as Move;
             ((MoveViewModel)BindingContext).OnDelete(x.id);
        }
    }
}
