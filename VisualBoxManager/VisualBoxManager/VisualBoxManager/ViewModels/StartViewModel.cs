using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace VisualBoxManager.ViewModels
{
    public class StartViewModel : RootViewModel
    {
        private string email;
        private string error;
        private string password;
        private bool busy;

        public StartViewModel(IConnectionService connectionService) : base(connectionService)
        {
            LoginCommand = new Command(async () => await OnLogin(), () => !IsBusy);
            CreatNewUserCommand = new Command(() => OnCreateNewUser(), () => !IsBusy);

            IsBusy = false;
            Email = "test@test.com";
            Password = "12345678";
        }

        internal async void LogOutAsync()
        {
            IsBusy = true;
            await _connectionService.Logout();
            IsBusy = false;
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
                if (value == null || value.Equals(string.Empty))
                {
                    Error = "Error!";
                }
                else
                {
                    Error = "";
                }

            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                if (value == null || value.Equals(string.Empty))
                {
                    Error = "Error!";
                }
                else
                {
                    Error = "";
                }
            }
        }
        public string Error
        {
            get
            {
                return error;
            }
            set
            {

                if ((email == null || email.Equals(string.Empty)) && (password == null || password.Equals(string.Empty)))
                {
                    error = "Email and password can not be empty.";
                }
                else if (email == null || email.Equals(string.Empty))
                {
                    error = "Email can not be empty.";
                }
                else if (password == null || password.Equals(string.Empty))
                {
                    error = "Password can not be empty.";
                }
                else
                {
                    error = value;
                }
                onPropertyChanged(nameof(Error));

            }
        }
        public bool IsBusy {
            get {
                return busy;
            }
            set {
                busy = value;
                onPropertyChanged(nameof(IsBusy));
                LoginCommand.ChangeCanExecute();
                CreatNewUserCommand.ChangeCanExecute();

            }
        }

        public Command LoginCommand { get; }
        public Command CreatNewUserCommand { get; }

        private async Task<Boolean> OnLogin()
        {
            // Maybe store username inside connectionservice? to me it sounds safer

            // We won't need to store the password in the running application.
            // The app should use a similar mechinisam to a web browser to maintain it's login
            // status i.e. a cookie and session variable. If we want auto login the we will need
            // to store the password in an encrypted store on the device. 
            // Future versions may use OAuth and it will use a token and only need the password
            // for the first login. OAuth also means the server can be stateless.


            bool loginSuccess = false;
            IsBusy = true;
            try
            {                 
                if (email == null || password == null)
                {
                    Error = "Username and password are required";
                } else
                {
                    loginSuccess = await _connectionService.Login(email, password);
                    if (!loginSuccess)
                    {
                        Error = "Invalid credentials";
                    } else
                    {
                        Error = "Success";
                        App.PushNavAsync(new MovePage(_connectionService));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("8==============D    Exception in Onlogin. Ex: " + ex.Message);
                Error = "Failed to login.";
            }
            IsBusy = false;
            return loginSuccess;
        }
        private void OnCreateNewUser()
        {
            App.PushNavAsync(new CreateUser(_connectionService));
        }
        
    }
}
