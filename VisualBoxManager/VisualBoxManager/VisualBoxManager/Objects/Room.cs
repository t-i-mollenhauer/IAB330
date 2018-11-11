using System;
using System.Collections.Generic;
using System.Text;

namespace VisualBoxManager
{
    public class Room
    {
        private string _name;
        private string _id;

        public Room(string name, string id)
        {
            _name = name;
            _id = id;
        }

        public Room(string name)
        {
            _name = name;
        }

        public Room(){}

        public string name {
            get { return _name; }
            set { _name = value; }
        }
        public string id{
            get { return _id; }
            set { _id = value; }
        }
    }
}
