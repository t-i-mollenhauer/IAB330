using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager.views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateItem : ContentPage
	{
		public CreateItem (IConnectionService connectionService, string moveId, string boxId)
		{
			InitializeComponent ();
            //BindingContext = new CreateItemViewModel(connectionService, moveId, boxId);
		}
	}
}