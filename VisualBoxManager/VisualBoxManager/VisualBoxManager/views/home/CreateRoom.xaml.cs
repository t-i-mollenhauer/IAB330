using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager.views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateRoom : ContentPage
	{
		public CreateRoom (IConnectionService connectionService, string moveId)
		{
			InitializeComponent ();
            BindingContext = new CreateRoomViewModel(connectionService, moveId);
        }
	}
}