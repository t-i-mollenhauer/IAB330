using System;
using VisualBoxManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Start : ContentPage
    {      
        public Start(IConnectionService connectionService)
        {
            InitializeComponent();
            BindingContext = new StartViewModel(connectionService);

        }

        internal void OnPopped(object sender, NavigationEventArgs e)
        {
            if (e.Page is MovePage)
                ((StartViewModel)BindingContext).LogOutAsync();
        }
    }
}
