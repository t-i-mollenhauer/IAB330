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
    public partial class Start : ContentPage
    {

        private IConnectionService _connectionService;

        public Start(IConnectionService connectionService)
        {
            BindingContext = IsBusy;
            InitializeComponent();

            _connectionService = connectionService ?? throw new ArgumentException(nameof(connectionService));
        }
        private void OnCreateNewUser(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateUser(_connectionService));
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            // Maybe store username inside connectionservice? to me it sounds safer

            // We won't need to store the password in the running application.
            // The app should use a similar mechinisam to a web browser to maintain it's login
            // status i.e. a cookie and session variable. If we want auto login the we will need
            // to store the password in an encrypted store on the device. 
            // Future versions may use OAuth and it will use a token and only need the password
            // for the first login. OAuth also means the server can be stateless.
            try
            {
                bool loginSuccess = false;
                this.loginBtn.IsEnabled = false;
                IsBusy = true;
                string uname = labelUname.Text, password = labelPass.Text;

                if (uname == null || password == null)
                {
                    labelErr.Text = "Username and password are required";
                    this.loginBtn.IsEnabled = true;
                    IsBusy = false;
                    return;
                }

                loginSuccess = await _connectionService.Login(uname, password);

                if (!loginSuccess)
                {
                    labelErr.Text = "Invalid credentials";
                    this.loginBtn.IsEnabled = true;
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    this.loginBtn.IsEnabled = true;
                    labelErr.Text = "Success";
                    await Navigation.PushAsync(new MovePage(_connectionService));
                }
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("8==============D    Exception in Onlogin. Ex: " + ex.Message);
                labelErr.Text = "Failed to login.";
                this.loginBtn.IsEnabled = true;
                IsBusy = false;
            }
        }
    }
}
