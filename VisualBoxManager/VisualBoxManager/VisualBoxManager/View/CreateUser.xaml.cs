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
        private IConnectionService _connectionservice;

        public CreateUser(IConnectionService connectionService)
        {
            InitializeComponent();
            _connectionservice = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        }

        private bool CheckAllEntrysFilled()
        {
            bool valid = true;

            if (!Validator.validateName(entryFirstName, labelErrFirstName))    valid = false;

            if (!Validator.validateName(entryLastName, labelErrLastName))      valid = false;

            if (!Validator.validatePass(entryPass, entryPassRe, labelErrPass)) valid = false;

            if (!Validator.validateEmail(entryEmail, labelErrEmail)) valid = false;

            return valid;
        }

        private async void OnCreateUser(object sender, EventArgs e)
        {
            btnCreateUser.IsEnabled = false;
            IsBusy = true;

            if (!CheckAllEntrysFilled())
            {
                IsBusy = false;
                btnCreateUser.IsEnabled = true;
                return;
            }            

            bool success = await _connectionservice.AddUser(entryFirstName.Text, entryLastName.Text, entryEmail.Text, entryPass.Text);
            if (success)
            {
                IsBusy = false;
                Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 1]);
                //await Navigation.PushAsync(new MovePage(_connectionservice));
                await Navigation.PushAsync(new MovePage(new MockConnectionService()));
            }
            else
            {
                IsBusy = false;
                btnCreateUser.IsEnabled = true;
            }
        }

 
    }
}

