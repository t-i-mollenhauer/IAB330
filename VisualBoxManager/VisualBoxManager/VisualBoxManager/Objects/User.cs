using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VisualBoxManager.ViewModels;

namespace VisualBoxManager
{
    sealed class User
    {
        //** SINGLETON SETUP **//
        private static User _instance = new User();

        private User()
        {
            moves = new ObservableCollection<Move>();
        }

        internal static User Instance()
        {
            return _instance;
        }
        //** END SINGLETON SETUP **//

        private string Email;
        public ObservableCollection<Move> moves;

        
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string id { get; set; }
        public string email {
            get { return Email; }
            set {
                if (isValidEmail(value))
                {
                    Email = value;
                }
                else
                {
                    throw new ArgumentException("Invalid email address");
                }
            }
        }

        public string getEmail() { return email; }

        

        private bool isValidEmail(String email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public void addOrUpdateMove(Move move)
        {
            foreach (Move existingMove in moves)
            {
                
                if (existingMove.id == move.id)
                {
                    existingMove.name = move.name;
                    return;
                }
            }
            moves.Add(move);
           
        }

        public void SyncMoves(List<Move> moves)
        {
            //This algorithim could potentially be a little processor intensive. Might do to revise it.
            foreach (Move newMove in moves)
            {
                bool found = false;
                for(int i = 0; i < this.moves.Count; i++)
                {
                    if(this.moves[i].id == newMove.id)
                    {
                        this.moves[i].id = newMove.id;
                        found = true;
                    }
                }
                if (!found) this.moves.Add(newMove);
            }
        }

        public ObservableCollection<Move> getMoves()
        {
            return moves;
        }

        public void MoveDeleted(string moveId)
        {
            for (int i = 0; i < this.moves.Count; i++)
            {
                if (this.moves[i].id == moveId)
                {
                    this.moves.RemoveAt(i);
                    break;
                }
            }
        }
    }  
}
