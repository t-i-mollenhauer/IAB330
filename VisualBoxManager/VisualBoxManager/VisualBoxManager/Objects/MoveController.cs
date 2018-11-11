using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VisualBoxManager.ConnectionServices;

namespace VisualBoxManager
{
    static class MoveController
    {
        public static async Task<bool> AddMove(String name, User user)
        {
            Move newMove = new Move(name);
            newMove.id = await MoveDAO.CreateMove(newMove);
            if (string.IsNullOrEmpty(newMove.id))
            {
                return false;
            }

            user.addOrUpdateMove(newMove);
            return true;
        }

        public static async Task<bool> GetMoveDetails(String moveId, User user)
        {

            return false;
        }

        public static async Task<bool> DeleteMove(String moveId, User user)
        {

            return false;
        }

        public static async Task<bool> EditMove(String moveId, User user)
        {

            return false;
        }
    }
}
