using System;
using System.Collections.Generic;
using System.Text;
using VisualBoxManager.Objects;

namespace VisualBoxManager
{
    public class Box
    {
        public string name { get; set; }
        public string id { get; set; }
        private bool packed;
        private bool sent;
        private bool fragile;
        //private Room destinationRoom;
        private String destinationRoomID;
        private List<Item> contents;
        private BoxPriority priority;


        public Box(string name, BoxPriority priority, String roomId)
        {
            this.name = name;
            this.priority = priority;
            destinationRoomID = roomId;
        }

        public String DestinationRoomID {
            get {
                return destinationRoomID;
            }
            set {
                destinationRoomID = value;
            }
        }

        public BoxPriority Priority {
            get {
                return priority;
            }
            set {
                priority = value;
            }
        }

        public enum BoxPriority
        {
            Low = 0,
            Medium = 1,
            High = 2
        }
    }
}
