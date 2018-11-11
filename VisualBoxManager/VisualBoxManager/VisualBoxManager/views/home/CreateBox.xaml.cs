using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VisualBoxManager.ViewModels.Home;

namespace VisualBoxManager.views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateBox : ContentPage
	{
		public CreateBox (IConnectionService connectionservice, string moveId)
        {
            //Initilize as delete
            InitializeComponent();
            BindingContext = new CreateBoxViewModel(connectionservice, moveId);
		}

        public CreateBox(IConnectionService connectionservice, string moveId, Box box)
        {
            //Initilize as an edit
            InitializeComponent();
            BindingContext = new CreateBoxViewModel(connectionservice, moveId, box);
        }
    }
}