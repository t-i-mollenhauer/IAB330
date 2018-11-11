using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.Objects.Validations;
using Xamarin.Forms;

namespace VisualBoxManager.ViewModels
{
    public class CreateUserViewModel : RootViewModel
    {
        private string email;
        private string fName;
        private string lName;

        private string password;
        private string repassword;

        private bool busy;
        private bool error;

        private string errorFirst;
        private string errorLast;
        private string errorPass;
        private string errorEmail;

        public CreateUserViewModel(IConnectionService connectionService) : base(connectionService)
        {
            CreatNewUserCommand = new Command(async () => await OnCreateUser(), () => !(IsBusy));
            IsBusy = false;
        }

        private bool CheckAllEntrysFilled()
        {

            ErrorFirst = Validator.ValidateName(fName).Message;

            ErrorLast = Validator.ValidateName(lName).Message;

            ErrorPass = Validator.ValidatePass(password, repassword).Message;

            ErrorEmail = Validator.ValidateEmail(email).Message;

            return !checkeError();
        }
        private bool checkeError() {
            if (errorFirst.Equals(string.Empty) && errorLast.Equals(string.Empty) && errorPass.Equals(string.Empty)
                    && errorEmail.Equals(string.Empty))
            {
                Error = false;
            }
            else
            {
                Error = true;
            }
            return Error;
        }

        private async Task<bool> OnCreateUser()
        {
            IsBusy = true;
            bool success = false;
            if (CheckAllEntrysFilled())
            {

                success = await _connectionService.AddUser(fName,lName, email, password);
                if (success)
                {

                    App.PopNavAsync();
                    App.PushNavAsync(new MovePage(_connectionService));
                    
                }
                else {
                    ErrorPass = "Create user failed!";
                }
            }
            IsBusy = false;
            return success;
            
        }
        public bool IsBusy
        {
            get
            {
                return busy;
            }
            set
            {
                busy = value;
                onPropertyChanged(nameof(IsBusy));
                CreatNewUserCommand.ChangeCanExecute();

            }
        }
        public Command CreatNewUserCommand { get; }

        public string FName
        {
            get
            {
                return fName;
            }
            set
            {
                fName = value;
            }
        }
        public string LName
        {
            get
            {
                return lName;
            }
            set
            {
                lName = value;
            }
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
               
            }
        }
        public string Repassword
        {
            get
            {
                return repassword;
            }
            set
            {
                repassword = value;
                ErrorPass = Validator.ValidatePass(password, repassword).Message;
                
            }
        }

        public string ErrorPass
        {
            get
            {
                return errorPass;
            }
            set
            {

                System.Diagnostics.Debug.WriteLine(value);
                errorPass = value;
                onPropertyChanged(nameof(ErrorPass));
                
            }
        }
        
        public string ErrorFirst
        {
            get
            {
                return errorFirst;
            }
            set
            {
                errorFirst = value;
                onPropertyChanged(nameof(ErrorFirst));

            }
        }
        public string ErrorLast
        {
            get
            {
                return errorLast;
            }
            set
            {

                System.Diagnostics.Debug.WriteLine(value);
                errorLast = value;
                onPropertyChanged(nameof(ErrorLast));

            }
        }   
        public string ErrorEmail
        {
            get
            {
                return errorEmail;
            }
            set
            {
                errorEmail = value;
                onPropertyChanged(nameof(ErrorEmail));

            }
        }
        public bool Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
                onPropertyChanged(nameof(Error));
                CreatNewUserCommand.ChangeCanExecute();
            }
        }
        
    }
}
