using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using VisualBoxManager.views.Home;
using Xamarin.Forms;
using System.ComponentModel;
using VisualBoxManager.ConnectionServices;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Windows.Input;

namespace VisualBoxManager.ViewModels
{
    public class MoveViewModel : RootViewModel
    {
        private User user;
        private bool busy;
        private Move moveSelected;

        public MoveViewModel(IConnectionService connectionService) : base(connectionService)
        {
            user = User.Instance();
            CreatNewMoveCommand = new Command(() => OnCreateNew(), () => !IsBusy);

            //Subscribe to collection changed event to allow updating of the moves list when data is loaded from the server.
            user.moves.CollectionChanged += new NotifyCollectionChangedEventHandler(this.MovesCollectionChanged);
            
            Refresh();

        }
        public ObservableCollection<Move> Moves {
            get {
                return user.getMoves();
            }
        }


        public ICommand RefreshCommand {
            get {
                return new Command(Refresh);
            }
        }


        public async void Refresh()
        {
            IsBusy = true;
            await MoveDAO.GetMoves();
            IsBusy = false;
        }

        //TODO: make MVVM https://huntjason.wordpress.com/2016/04/19/xamarin-forms-navigation-part-2-mvvm-and-the-tabbedpage/      
        public Move SelectedMove {
            get {
                return moveSelected;
            }
            set {
                moveSelected = value;
                if (value != null)
                {
                    var tab = new TabbedPage() { Title = moveSelected.name };
                    tab.Children.Add(new BoxPage(_connectionService, moveSelected) { Title = "Boxes" });
                    tab.Children.Add(new ItemPage(_connectionService, moveSelected) { Title = "Items" });
                    tab.Children.Add(new Scanner(_connectionService, moveSelected) { Title = "Scanner" });
                    App.PushNavAsync(tab);
                }
            }
        }

        private void OnCreateNew()
        {
            App.PushNavAsync(new CreateMove(_connectionService));
        }
        public Command CreatNewMoveCommand { get; }

        public async void OnDelete(string id)
        {
            IsBusy = true;
            await MoveDAO.DeleteMove(id);
            IsBusy = false;
        }

        public Command DeleteCommand { get;  }

        public bool IsBusy {
            get {
                return busy;
            }
            set {
                busy = value;
                onPropertyChanged(nameof(IsBusy));
                CreatNewMoveCommand.ChangeCanExecute();
            }
        }

        private void MovesCollectionChanged(object aSender, NotifyCollectionChangedEventArgs aArgs)
        {
            onPropertyChanged(nameof(Moves));
        }

    }
}
