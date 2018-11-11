using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;

namespace VisualBoxManager
{
    public class Move
    {
        public ObservableCollection<Box> boxes = new ObservableCollection<Box>();
        public ObservableCollection<Room> rooms = new ObservableCollection<Room>();
        public Move() { }

        public Move(string name)
        {
            this.name = name;
        }

        public Move(string name, string id)
        {
            this.name = name;
            this.id = id;
        }

        public string name { get; set; }
        public string id { get; set; }

        public void SyncBoxes(List<Box> boxes)
        {
            foreach (Box newBox in boxes)
            {
                bool found = false;
                for (int i = 0; i < this.boxes.Count; i++)
                {
                    if (this.boxes[i].id == newBox.id)
                    {
                        this.boxes[i] = newBox;
                        found = true;
                    }
                }
                if (!found) this.boxes.Add(newBox);
            }
        }

        public void SyncRooms(List<Room> rooms)
        {
            foreach (Room newRoom in rooms)
            {
                bool found = false;
                for (int i = 0; i < this.rooms.Count; i++)
                {
                    if (this.rooms[i].id == newRoom.id)
                    {
                        this.rooms[i] = newRoom;
                        found = true;
                    }
                }
                if (!found) this.rooms.Add(newRoom);
            }
        }

        public void DeleteBox(String boxId)
        {
            for(int i = 0; i < this.boxes.Count; i++)
            {
                if(this.boxes[i].id == boxId)
                {
                    this.boxes.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
