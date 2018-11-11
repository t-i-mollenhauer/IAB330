using System;
using System.Collections.Generic;
using System.Text;
using VisualBoxManager;
using VisualBoxManager.ViewModels;
using Xamarin.Forms;

namespace VisualBoxManager.ViewModels.Home
{
    public class ItemPageViewModel : RootViewModel
    {

        private List<Item> items;
        private bool busy;
        private Item itemSelected;
        private string _moveId;

        public ItemPageViewModel(IConnectionService connectionService, string moveId) : base(connectionService)
        {
            items = new List<Item>();
            _moveId = moveId;
            CreatNewItemCommand = new Command(() => OnCreateNew(), () => !IsBusy);
            // (User.Instance()).SetItemPage(this);
            Refresh();
        }

        public List<Item> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
                onPropertyChanged(nameof(Items));
            }
        }

        public Command CreatNewItemCommand { get; }

        private void OnCreateNew()
        {
            //App.PushNavAsync(new CreateItem(_connectionService, _move));
        }
        public async void Refresh()
        {
            IsBusy = true;
            Items.Clear();
            foreach (Item Item in (await _connectionService.GetItems(_moveId)))
            {
                Items.Add(Item);
            }
            Items = Items;
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
                CreatNewItemCommand.ChangeCanExecute();
            }
        }

    }
}
