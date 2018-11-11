using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager { 

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUser : ContentPage
    {
        
        public CreateUser(IConnectionService connectionService)
        {
            InitializeComponent();
            BindingContext = new ViewModels.CreateUserViewModel(connectionService);
        }
        
    }
}

