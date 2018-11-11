using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace VisualBoxManager.ConnectionServices
{
    static class MoveDAO
    {
        private static User user = User.Instance();
        private static ConnectionService connection;

        static MoveDAO()
        {
            connection = ConnectionService.Instance();
        }

        /// <summary>
        /// Requests a list of all moves for the user from PAMS (Pack manager service).
        /// Passes returned data to the user singleton and triggers a sync of the moves array.
        /// </summary>
        /// <returns>True if successful, false on failure</returns>
        public static async Task<bool> GetMoves()
        {

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, Config.getMovesUri());
            var response = await connection.MakeRequest(message);
            List<Move> responseResult = new List<Move>();

            if (response.IsSuccessStatusCode)
            {
                //process result
                try
                {
                    String responseData = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("8=============D    Retrieved moves: " + responseData);
                    responseResult = JsonConvert.DeserializeObject<List<Move>>(responseData);
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
                user.SyncMoves(responseResult);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Takes a valid move object and converts it to json before posting to PAMS (Pack manager service)
        /// </summary>
        /// <param name="move"></param>
        /// <returns>String id of new move if successfull. Null if failure.</returns>
        public static async Task<String> CreateMove(Move move)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Config.getMovesUri());
            message.Content = new StringContent(
                JsonConvert.SerializeObject(move, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

            var response = await connection.MakeRequest(message);
            if (response.IsSuccessStatusCode)
            {
                String responseString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("8================D  Created move, response from server: " + responseString);
                try
                {
                    String moveId;
                    var responseResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(responseString);
                    responseResult.TryGetValue("id", out moveId);
                    return moveId;
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

        public static async Task<bool> DeleteMove(String moveId)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, Config.getMoveDetailUri(moveId));
            var response = await connection.MakeRequest(message);
            //TODO: for some reason the server is returning a 404 despite the response being set to 200.
            if (response.IsSuccessStatusCode || true)
            {
                user.MoveDeleted(moveId);
                return true;
            }
            return false;
        }
    }
}
