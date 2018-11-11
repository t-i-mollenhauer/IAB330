using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using VisualBoxManager.Objects.Validations;
using Xamarin.Forms;

namespace VisualBoxManager.ViewModels
{
    public class CreateMoveViewModel : RootViewModel
    {
        private User user = User.Instance();
        private bool busy;

        private string moveName;
        private string errMoveName;

        public CreateMoveViewModel(IConnectionService connectionService) : base(connectionService)
        {
            CancelCommand = new Command(() => OnCancel(), () => !(IsBusy));
            CreatNewMoveCommand = new Command(async () => await OnSave(), () => !(IsBusy));
            IsBusy = false;
        }

        private void OnCancel()
        {
            App.PopNavAsync();
        }
        //TODO; fix race condition when returning to Moves 
        private async Task<bool> OnSave()
        {
            IsBusy = true;
            bool success = false;
            ValidationResult result = Validator.ValidateName(moveName);
            ErrMoveName = result.Message;
            
            if (!result.Error)
            {
                if (! (await MoveController.AddMove(moveName, user)))
                {
                    ErrMoveName = "Failed to add new move. Please try again";                                 
                }
                else
                {
                    //user.addOrUpdateMove(newMove);
                    ErrMoveName = "Success!";                      
                    App.PopNavAsync();
                    success = true;
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
                CreatNewMoveCommand.ChangeCanExecute();
                CancelCommand.ChangeCanExecute();

            }
        }
        public string ErrMoveName
        {
            get
            {
                return errMoveName;
            }
            set
            {
                errMoveName = value;
                onPropertyChanged(nameof(ErrMoveName));
            }
        }
        public string MoveName
        {
            get
            {
                return moveName;
            }
            set
            {
                moveName = value;
                onPropertyChanged(nameof(MoveName));
            }
        }
        public Command CreatNewMoveCommand { get; }
        public Command CancelCommand { get; }

    }
}
