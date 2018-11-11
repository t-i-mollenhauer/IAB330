using System;
using System.Collections.Generic;
using System.Text;

namespace VisualBoxManager
{
    sealed class Auth
    {
        private bool authState;
        private string sessionID;

        private static Auth _instance = new Auth();

        private Auth()
        {
            authState = false;
            sessionID = "";
        }

        static internal Auth Instance()
        {
            return _instance;
        }

        public bool authenticated()
        {
            return authState;
        }

        public bool setAuthenticated(string sessionID)
        {
            if (sessionID.StartsWith("JSESSIONID"))
            {
                sessionID = sessionID.Split(';')[0];
                sessionID = sessionID.Substring(11);
            }
            authState = true;
            this.sessionID = sessionID;
            return authState;
        } 

        public bool clearAuth()
        {
            authState = false;
            this.sessionID = "";
            return authState;
        }

        public string getSessionId()
        {
            return sessionID;
        }
    }
}
