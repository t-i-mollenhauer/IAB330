using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Collections;
using System.Linq;
using System.Security.Authentication;
using Newtonsoft.Json.Linq;
using VisualBoxManager.Objects;
using System.Collections.ObjectModel;

namespace VisualBoxManager
{
    class ConnectionService : IConnectionService
    {
        private readonly User user;
        private bool authorised;

        private HttpClient client;
        private static CookieContainer cookieContainer = new CookieContainer();

        //** SINGLETON SETUP **//
        private static ConnectionService _instance = new ConnectionService();

        private ConnectionService()
        {
            user = User.Instance();
            this.authorised = false;

            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            handler.CookieContainer = cookieContainer;

            this.client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip");
            client.DefaultRequestHeaders.Add("accept", "application/json");
        }

        internal static ConnectionService Instance()
        {
            return _instance;
        }

        //** END SINGLETON SETUP **//

        public HttpClient GetClient()
        {
            return this.client;
        }

        public bool IsAuthorised()
        {
            return this.authorised;
        }

        public async Task<bool> Login(string username, string password)
        {
            var authData = string.Format("{0}:{1}", username, password);
            var request = new HttpRequestMessage()
            {
                RequestUri = Config.getLoginUri(),
                Method = HttpMethod.Post
            };

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(authData))
            );

            authorised =  await ProcessLoginResponse(await client.SendAsync(request));
            return authorised;
        }

        private async Task<bool> ProcessLoginResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                //process result
                Dictionary<String, String> responseResult;
                try
                {
                    responseResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(await response.Content.ReadAsStringAsync());
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("8=============D    Error deserializing object: " + e.GetType().ToString() + "   Message: " + e.Message);
                    return false;
                }

                String authorised;
                responseResult.TryGetValue("authenticated", out authorised);
                if (authorised != null && authorised.Equals("true"))
                {
                    IEnumerable<string> cookieHeader;
                    response.Headers.TryGetValues("Set-Cookie", out cookieHeader);

                    String sessionID = (cookieHeader.First()).Split(';')[0];
                    sessionID = sessionID.Substring(11);
                    System.Diagnostics.Debug.WriteLine("8=============D    Seting SessionID too: " + sessionID);
                    cookieContainer.Add(Config.getUri(), new Cookie("JSESSIONID", sessionID));
                    this.authorised = true;
                }
                else
                {
                    this.authorised = false;
                    var cookies = cookieContainer.GetCookies(Config.getUri());
                    foreach (Cookie cookie in cookies)
                    {
                        cookie.Expired = true;
                    }
                }
                
                return true;

            }
            return false;
        }



        public async Task<HttpResponseMessage> MakeRequest(HttpRequestMessage message)
        {
            if (authorised)
            {
                try
                {
                    return await client.SendAsync(message);
                }catch(Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("8===========D    Exception encountered sending message to server. Exception:  " + e.ToString());
                    return null;
                }
            }
            else
            {
                throw new AuthenticationException("User is not authenticated");
            }
        }

        public async Task<bool> Logout()
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = Config.getLogoutUri(),
                Method = HttpMethod.Post
            };

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                this.authorised = false;
                var cookies = cookieContainer.GetCookies(Config.getUri());
                foreach (Cookie cookie in cookies)
                {
                    cookie.Expired = true;
                }
            }
            return this.authorised;
        }

        //**OLD FUNCTIONS TO BE MOVED**\\

        public async Task<bool> AddUser(string firstName, string lastName, string email, string password)
        {
            user.email     = email.Trim();
            user.firstName = firstName.Trim();
            user.lastName  = lastName.Trim();

            //Build Json content for post

            JObject jObj = JObject.FromObject(user);
            jObj.Add("password", password);
            String json = JsonConvert.SerializeObject(jObj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            System.Diagnostics.Debug.WriteLine("8================D  Creating new user, sending json to server: " + json);

            //Build HttpClient and send request
            try
            {
                HttpClient client = HttpClientFactory.getCLient();
                var response = await client.PostAsync(Config.getUserUri(), new StringContent(json));


                //Check for valid response
                if (response.IsSuccessStatusCode)
                {
                    String responseString = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("8================D  Received 2xx code when adding user. Response: " + responseString);
                    Dictionary<String, String> responseResult;
                    String userId;
                    try
                    {
                        responseResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(responseString);
                        responseResult.TryGetValue("id", out userId);
                        user.id = userId;
                        System.Diagnostics.Debug.WriteLine("8=============D   User ID has been set to: " + user.id);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine("8=============D    Error deserializing object: " + e.GetType().ToString() + "   Message: " + e.Message);
                    }

                    bool loggedIn = await Login(user.email, password);

                    return (loggedIn) ? true : false;

                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("8================D  Non 200 response when adding user: " + response.StatusCode + " content:" + await response.Content.ReadAsStringAsync());
                    return false;
                }

            }catch (Exception e){
                System.Diagnostics.Debug.WriteLine("8================D  Exception when sending request: " + e.GetType().ToString() + " --> MESSAGE: " + e.Message);
            }
            return false;
            //add id to user object

            
        }


        public string GetUsername()
        {
            throw new NotImplementedException();
        }



        public async Task<List<Box>> GetBoxes(string moveId)
        {
            return await Task.FromResult(new List<Box>());
        }

        public async Task<List<Item>> GetItems(string moveId)
        {
            return await Task.FromResult(new List<Item>());
        }

        public async Task<List<Item>> GetItems(string moveId, string boxId)
        {
            return await Task.FromResult(new List<Item>());
        }

        // must assign an ID do box.id
        public async Task<string> CreateBox(string moveId, Box box)
        {
            return await Task.FromResult("Create Box");
        }

        public Task<List<Room>> GetRooms(string moveId)
        {
            throw new NotImplementedException();
        }
    }
}
