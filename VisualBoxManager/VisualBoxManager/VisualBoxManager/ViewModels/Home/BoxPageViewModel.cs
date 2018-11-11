using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VisualBoxManager.ConnectionServices;
using VisualBoxManager.views;
using Xamarin.Forms;

namespace VisualBoxManager.ViewModels.Home
{
    public class BoxPageViewModel : RootViewModel
    {
        //private List<Box> boxes;
        private bool busy;
        private Box itemSelected;
        private string _moveId;
        private Move move;
        private User user;

        public BoxPageViewModel(IConnectionService connectionService, string moveId) : base(connectionService)
        {
            //Boxes = new List<Box>();
            user = User.Instance();
            _moveId = moveId;
            move = user.moves.SingleOrDefault(mv => mv.id == _moveId);

            CreatNewBoxCommand = new Command(() => OnCreateNew(), () => !IsBusy);
            move.boxes.CollectionChanged += new NotifyCollectionChangedEventHandler(this.BoxesCollectionChanged);

            Refresh();
        }

        public ObservableCollection<Box> Boxes {
            get
            {
                return move.boxes;
            }
        }

        internal async void DeleteBox(string id)
        {
            await BoxDAO.DeleteBox(_moveId, id);
            Refresh();
        }

        internal  void EditBox(Box box)
        {
            App.PushNavAsync(new CreateBox(_connectionService, _moveId, box));
        }

        private void OnCreateNew()
        {
            App.PushNavAsync(new CreateBox(_connectionService, _moveId));            
        }
        public async void Refresh()
        {
            IsBusy = true;
            await BoxDAO.GetBoxes(_moveId); 
            IsBusy = false;
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
                CreatNewBoxCommand.ChangeCanExecute();
            }
        }

        public Command CreatNewBoxCommand { get; }
        //public Command RefreshCommand { get; }
        public ICommand RefreshCommand {
            get {
                return new Command(Refresh);
            }
        }


        private void BoxesCollectionChanged(object aSender, NotifyCollectionChangedEventArgs aArgs)
        {
            onPropertyChanged(nameof(Boxes));
        }

    }
}
