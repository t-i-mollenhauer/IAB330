using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VisualBoxManager.ViewModels
{
    public abstract class RootViewModel : INotifyPropertyChanged
    {
        protected IConnectionService _connectionService;
        public RootViewModel(IConnectionService connectionService)
        {
            //TODO: If we uncomment the following line we'll be able to remove the parameter in the constructor and just get an instance of the Connection service from here.
            //_connectionService = ConnectionService.Instance();
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void onPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
    }
}
