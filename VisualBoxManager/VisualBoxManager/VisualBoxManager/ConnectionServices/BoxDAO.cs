using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.Objects;

namespace VisualBoxManager.ConnectionServices
{
    class BoxDAO
    {
        private static User user = User.Instance();
        private static ConnectionService connection;

        static BoxDAO()
        {
            connection = ConnectionService.Instance();
        }

        /// <summary>
        /// Retrieves a list of boxes from PAMS for the specified move.
        /// Syncs retrieved boxes with client model.
        /// </summary>
        /// <param name="moveID">ID for the move</param>
        /// <returns>True on success, false on fail.</returns>
        public static async Task<bool> GetBoxes(String moveID)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, Config.getBoxesForMoveURI(moveID));
            var response = await connection.MakeRequest(message);

            List<Box> responseResult = new List<Box>();

            if (response.IsSuccessStatusCode)
            {
                //process result
                try
                {
                    String responseData = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("8=============D    Retrieved boxes: " + responseData);
                    //JObject jObj = JObject.Parse(responseData);

                    //JToken priorityJson;
                    //if (jObj.TryGetValue("priority", out priorityJson))
                    //{
                    //    //Process priority
                    //    //TODO: Create new priority ready to add to the box.
                    //    jObj.Remove("priority");
                    //}
                    //responseResult = JsonConvert.DeserializeObject<List<Box>>(jObj.ToString());

                    responseResult = JsonConvert.DeserializeObject<List<Box>>(responseData);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("8=============D    Error deserializing object: " + e.GetType().ToString() + "   Message: " + e.Message);
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
                move.SyncBoxes(responseResult);
            }

            return true;
        }

        /// <summary>
        /// Save a box for a given move to PAMS.
        /// </summary>
        /// <param name="moveId">ID of move to store new box in</param>
        /// <param name="newBox">Box to save to server</param>
        /// <returns>ID of new box.</returns>
        public static async Task<String> CreateBox(String moveId, Box newBox)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Config.getBoxesForMoveURI(moveId) + "/");
            message.Content = new StringContent(
                JsonConvert.SerializeObject(newBox, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

            var response = await connection.MakeRequest(message);
            if (response.IsSuccessStatusCode)
            {
                String responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("8================D  Created box, response from server: " + responseString);
                try
                {
                    String boxId;
                    var responseResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(responseString);
                    responseResult.TryGetValue("id", out boxId);
                    return boxId;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("8=============D    Error deserializing object: " +
                                                       e.GetType().ToString() + "   Message: " + e.Message);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Sends a box update request to PAMS.
        /// </summary>
        /// <param name="moveId">ID of the move containing the box</param>
        /// <param name="box">An existing box object with edits made</param>
        /// <returns></returns>
        public static async Task<bool> UpdateBox(String moveId, Box box)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Put, Config.getBoxesForMoveURI(moveId) + "/" + box.id + "/");
            message.Content = new StringContent(
                JsonConvert.SerializeObject(box, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

            var response = await connection.MakeRequest(message);
            if (response != null && response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="moveId"></param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteBox(String moveId, String boxId)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, Config.getBoxesForMoveURI(moveId) + "/" + boxId + "/");
            var response = await connection.MakeRequest(message);
            if (response.IsSuccessStatusCode)
            {
                var move = user.moves.SingleOrDefault(mv => mv.id == moveId);
                move.DeleteBox(boxId);
            }
            return response.IsSuccessStatusCode;
        }
    }
}