using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VisualBoxManager
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateMove : ContentPage
    {
        private IConnectionService _connectionService;
        private User user;

        public CreateMove(IConnectionService connectionService)
        {
            InitializeComponent();
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
            user = User.Instance();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void OnSave(object sender, EventArgs e)
        {
            IsBusy = true;
            btnCancel.IsEnabled = false;
            btnSave.IsEnabled = true;
            if (!Validator.validateName(entryMoveName, labelErrMoveName))
            {
                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = true;
                IsBusy = false;
                return;
            }

            Move newMove = await MoveController.AddMove(entryMoveName.Text, _connectionService);
            if (string.IsNullOrEmpty(newMove.id))
            {
                labelErrMoveName.Text = "Failed to add new move. Please try again";
                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = true;
                IsBusy = false;
                return;
            }
            user.addOrUpdateMove(newMove);
            labelErrMoveName.Text = "Success!";
            IsBusy = false;
            await Navigation.PopAsync();
        }
        protected override async void OnAppearing()
        {
            btnCancel.IsEnabled = true;
            btnSave.IsEnabled = true;
            IsBusy = false;
        }
    }
}