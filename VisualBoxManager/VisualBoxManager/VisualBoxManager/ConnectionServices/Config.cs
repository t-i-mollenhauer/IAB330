using System;
using System.Collections.Generic;
using System.Text;

namespace VisualBoxManager
{
    static class Config
    {
        public static readonly bool deployed = false;

        public static string getAddress()
        {
            if (deployed)
            {
                return  "pack-manager.herokuapp.com";
            }
            //return "172.19.29.7"; //uni address
            return "192.168.1.79";  //local dev address
        }


        public  static int port = 4567;
        public static Uri getUri() { return new Uri(getBaseUri());  }

        
        private static string getBaseUri() {
            if (deployed)
            {
                return "https://" + getAddress() + "/";

            }
            return "http://" + getAddress() + ":" + port.ToString() + "/";
        } 

        public static Uri getLoginUri() { return new Uri(getBaseUri() + "login/"); }
        public static Uri getLogoutUri() { return new Uri(getBaseUri() + "logout/"); }
        public static Uri getUserUri() { return new Uri(getBaseUri() + "user/"); }

        public static Uri getMovesUri() { return new Uri(getBaseUri() + "api/move/"); }

        public static Uri getMoveDetailUri  (String moveID) { return new Uri(getBaseUri() + "api/move/" + moveID); }

        public static Uri getBoxesForMoveURI(String moveID) { return new Uri(getMovesUri() + moveID + "/box"); }

        public static Uri getRoomsForMoveURI(String moveId) { return new Uri(getMovesUri() + moveId + "/room/");}
    }
}