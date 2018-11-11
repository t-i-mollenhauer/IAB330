using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace VisualBoxManager
{
    public partial class App : Application
    {
        private IConnectionService _connectionService;
        private static NavigationPage _navPage;
        private Auth auth;
                
        public App()
        {
            InitializeComponent();

            _connectionService = ConnectionService.Instance();//new MockConnectionService();
            var start = new Start(_connectionService);
            _navPage = new NavigationPage(start);
            _navPage.Popped += start.OnPopped;
            MainPage = _navPage;
            System.Diagnostics.Debug.WriteLine("App initialisation complete");
        }
        //Not a good practice?
        private static NavigationPage getNavigation() {
            return _navPage;
        }
        public static async void PopNavAsync()
        {
           await _navPage?.PopAsync();
        }
        public static async void PushNavAsync(Page page)
        {
            await _navPage?.PushAsync(page);
        }


        protected override void OnResume()
        {
            //TODO: Restore session and auth information from permanent memory to the Auth singelton.
            //TODO: should we fire a request to the server to see if the session has expired?
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            //TODO: In here we will need to store the session ID in permanent memory after 
            //retrieving it from the Auth singelton.

            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            auth = Auth.Instance();
        }
    }
}
