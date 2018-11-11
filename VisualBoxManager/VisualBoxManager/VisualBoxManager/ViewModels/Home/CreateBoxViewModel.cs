using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using VisualBoxManager.ConnectionServices;
using VisualBoxManager.Objects;
using VisualBoxManager.Objects.Validations;
using VisualBoxManager.views;
using static VisualBoxManager.Box;

namespace VisualBoxManager.ViewModels
{
    public class CreateBoxViewModel : RootViewModel
    {
        private string _moveId;
        private string _boxName;
        private BoxPriority _boxPriority = BoxPriority.Medium;
        private bool _busy;
        private string _errBoxName;
        //private List<Room> _rooms = new List<Room>();
        private Room _selectedRoom;
        private User user = User.Instance();
        private bool _editing = false;
        private Box _editingBox;

        public CreateBoxViewModel(IConnectionService connectionService, string moveId) : base(connectionService)
        {
            _moveId = moveId;
            CancelCommand = new Command(() => OnCancel(), () => !(IsBusy));
            CreateNewBoxCommand = new Command(async () => await OnSave(), () => !(IsBusy));
            CreateNewRoomCommand = new Command(() => CreateNewRoom());
            Refresh();
            //SelectedRoom = 1;
            SelectedRoom = new Room("Not Set", "-1");

        }

        public CreateBoxViewModel(IConnectionService connectionService, string moveId, Box box) : this(connectionService, moveId)
        {
            //This constructor is used when editing a box rather than simply creating a new one.
            _editing = true;
            _editingBox = box;

            var move = user.moves.SingleOrDefault(mv => mv.id == _moveId);
            SelectedRoom = move.rooms.SingleOrDefault(rm => rm.id == box.DestinationRoomID);
            _boxName = box.name;

        }

        private async void Refresh()
        {
            IsBusy = true;
            await RoomDAO.GetRoomsForMove(_moveId);
            onPropertyChanged(nameof(Rooms));
            IsBusy = false;
        }

        public string BoxName {
            get {
                return _boxName;
            }
            set {
                _boxName = value;
                onPropertyChanged(nameof(BoxName));
            } }
        public string ErrBoxName
        {
            get
            {
                return _errBoxName;
            }
            set
            {
                _errBoxName = value;
                onPropertyChanged(nameof(ErrBoxName));
            }
        }
        public int Priority {
            get
            {
                return (int)_boxPriority;
            }

            set
            {
                _boxPriority = (BoxPriority)value;
                onPropertyChanged(nameof(Priority));
            }
        }

        public Room SelectedRoom {
            get {
                return _selectedRoom;
            }
            set {
                _selectedRoom = value;
                onPropertyChanged(nameof(SelectedRoom));
            }
        }

        //public List<String> Rooms {
        //get {
        //        var rooms = _rooms.Select(x => x.Name).ToList();
        //        rooms.Insert(0, "No room");
        //        return rooms;
        //    }
        //}
        public ObservableCollection<Room> Rooms
        {
            get {
                var move = user.moves.SingleOrDefault(mv => mv.id == _moveId);
                return move.rooms;
            }
        }

        public string ErrRooms { get; set; }

        public bool IsBusy
        {
            get
            {
                return _busy;
            }
            set
            {
                _busy = value;
                onPropertyChanged(nameof(IsBusy));
                onPropertyChanged(nameof(IsNotBusy));
                CreateNewBoxCommand.ChangeCanExecute();
                CancelCommand.ChangeCanExecute();

            }
        }

        // Most minimalistic way to disable entrys while being busy
        public bool IsNotBusy => !_busy;

        private void OnCancel()
        {
            App.PopNavAsync();
        }

        private async Task<bool> OnSave()
        {
            if (_editing)
            {
                return await UpdateBox();
            }
            IsBusy = true;
            bool success = false;

            ValidationResult result = Validator.ValidateName(_boxName);
            ErrBoxName = result.Message;

            if (!result.Error)
            {
                //String roomId = null;

                //if (SelectedRoom != null)
                //     roomId = SelectedRoom.Id;

                Box newBox = new Box(_boxName, _boxPriority, SelectedRoom.id);
               // await _connectionService.CreateBox(_moveId, newBox);
                newBox.id = await BoxDAO.CreateBox(_moveId, newBox);
                if (string.IsNullOrEmpty(newBox.id))
                {
                    ErrBoxName = "Failed to add new box. Please try again";
                    IsBusy = false;
                }
                else
                { 
                    ErrBoxName = "Success!";
                    IsBusy = false;
                    App.PopNavAsync();
                    success = true;
                }
            }
            IsBusy = false;
            return success;
        }

        private async Task<bool> UpdateBox()
        {
            IsBusy = true;
            System.Diagnostics.Debug.WriteLine("8=============D    Preparing to send box update to PAMS");
            if(SelectedRoom != null)
            {
                _editingBox.DestinationRoomID = SelectedRoom.id;
            }
            _editingBox.name = _boxName;
            _editingBox.Priority = _boxPriority;
            bool success =  await BoxDAO.UpdateBox(_moveId, _editingBox);
            IsBusy = false;
            App.PopNavAsync();
            return success;
        }

        private void CreateNewRoom()
        {
            App.PushNavAsync(new CreateRoom(_connectionService, _moveId));
        }

        public Command CreateNewBoxCommand { get; }
        public Command CancelCommand { get; }
        public Command CreateNewRoomCommand { get; }

    }
}
