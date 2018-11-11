using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VisualBoxManager.ConnectionServices
{
    class RoomDAO
    {
        private static User user = User.Instance();
        private static ConnectionService connection;

        static RoomDAO()
        {
            connection = ConnectionService.Instance();
        }

        /// <summary>
        /// Gets list of rooms for provided Move Id and syncs with move object in the model.
        /// </summary>
        /// <param name="moveID">ID of the move</param>
        /// <returns>Boolean success/fail</returns>
        public static async Task<bool> GetRoomsForMove(String moveID)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, Config.getRoomsForMoveURI(moveID));
            var response = await connection.MakeRequest(message);

            List<Room> responseResult = new List<Room>();

            if (response.IsSuccessStatusCode)
            {
                //process result
                try
                {
                    String responseData = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("8=============D    Retrieved boxes: " + responseData);
                    responseResult = JsonConvert.DeserializeObject<List<Room>>(responseData);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("8=============D    Error deserializing object: " +
                                                       e.GetType().ToString() + "   Message: " + e.Message);
                }
            }
            else
            {
                //TODO: opps not 2xx status code. What now?
                //Build custom exception handler
                //Redirect user to login if auth problem
                //Rollback and retry if 5xx problem
                //Alert user
            }

            if (0 < (responseResult.Count))
            {
                var move = user.moves.SingleOrDefault(mv => mv.id == moveID);
                move.SyncRooms(responseResult);
            }

            return true;
        }

        public static async Task<bool> AddRoomToMove(String moveId, Room newRoom)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Config.getRoomsForMoveURI(moveId) );
            message.Content = new StringContent(
                JsonConvert.SerializeObject(newRoom, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

            var response = await connection.MakeRequest(message);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }

}

