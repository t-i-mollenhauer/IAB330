using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager.Objects;

namespace VisualBoxManager
{
    public interface IConnectionService
    {
        Task<bool> AddUser(string firstName, string lastName, string email, string password);
        //bool CheckUsername(string username);
        Task<List<Box>> GetBoxes(string moveId);
        Task<List<Item>> GetItems(string moveId);
        Task<List<Item>> GetItems(string moveId, string boxId);
       // Task<ObservableCollection<Move>> GetMoves();
       // Task<String> CreateMove(Move move);
        Task<String> CreateBox(string moveId, Box box);
        string GetUsername();
        Task<bool> Login(string username, string password);
        Task<List<Room>> GetRooms(string moveId);
        Task<bool> Logout();
    }
}
