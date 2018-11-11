using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.ConnectionServices;
using VisualBoxManager.Objects.Validations;
using Xamarin.Forms;

namespace VisualBoxManager.ViewModels
{
    class CreateRoomViewModel : RootViewModel
    {
        private User user = User.Instance();
        private string roomName;
        private string errRoomName;
        private string _moveId;
        private bool busy;

        public CreateRoomViewModel(IConnectionService connectionService, String moveId) : base(connectionService)
        {
            _moveId = moveId;
            CancelCommand = new Command(() => OnCancel(), () => !(IsBusy));
            CreatNewRoomCommand = new Command(async () => await OnSave(), () => !(IsBusy));
            IsBusy = false;
        }

        private void OnCancel()
        {
            App.PopNavAsync();
        }

        private async Task<bool> OnSave()
        {
            IsBusy = true;
            bool success = false;
            ValidationResult result = Validator.ValidateName(roomName);
            ErrRoomName = result.Message;

            if (!result.Error)
            {
                if (!(await RoomDAO.AddRoomToMove(_moveId, new Room(roomName))))
                {
                    ErrRoomName = "Failed to add new room. Please try again";
                }
                else
                {
                    ErrRoomName = "Success!";
                    App.PopNavAsync();
                    success = true;
                }
            }
            IsBusy = false;
            return success;
        }
        public bool IsBusy {
            get {
                return busy;
            }
            set {
                busy = value;
                onPropertyChanged(nameof(IsBusy));
                CreatNewRoomCommand.ChangeCanExecute();
                CancelCommand.ChangeCanExecute();

            }
        }
        public string ErrRoomName {
            get {
                return errRoomName;
            }
            set {
                errRoomName = value;
                onPropertyChanged(nameof(ErrRoomName));
            }
        }
        public string RoomName {
            get {
                return roomName;
            }
            set {
                roomName = value;
                onPropertyChanged(nameof(RoomName));
            }
        }
        public Command CreatNewRoomCommand { get; }
        public Command CancelCommand { get; }
    }
}
