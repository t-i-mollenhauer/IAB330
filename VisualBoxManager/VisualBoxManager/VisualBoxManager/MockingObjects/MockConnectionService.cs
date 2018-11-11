using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VisualBoxManager.Objects;
using static VisualBoxManager.Box;

namespace VisualBoxManager
{
    class MockConnectionService : IConnectionService
    {
        private string _uname;
        private User user;
        private int idCounter;
        public MockConnectionService() {
            user = User.Instance();

            List<Move> list = new List<Move>
             {
                 new Move("Grandmothers move","0"),
                 new Move("Move 2016","1"),
                 new Move("Helping friends","2"),
             };
            idCounter = 4;
            user.SyncMoves(list);
            user.addOrUpdateMove( new Move("Helping friends2", "3"));
        }
        public Task<bool> AddUser(string firstName, string lastName, string email, string password)
        {
          
            return Task.FromResult<bool>(true);
        }

        public bool CheckUsername(string username)
        {
            if (username == null)
                return false;
            return true;
        }

        public async Task<List<Box>> GetBoxes(string moveId)
        {
                List<Box> list = new List<Box>
            {
                new Box("Kitchen 1", BoxPriority.Medium, null),
                new Box("Kitchen 2", BoxPriority.Medium, null),
                new Box("LivingRoom 1", BoxPriority.Medium, null),
                new Box("Garry's room 1", BoxPriority.Medium, null),
                new Box("Garry's room 2", BoxPriority.Medium, null)
            };

            return list;
        }

        public async Task<List<Item>> GetItems(string moveId)
        {
            List<Item> list = new List<Item>
            {
                new Item("Forks"),
                new Item("Chairs"),
                new Item("Gameboy"),
                new Item("Computer"),
                new Item("Mac"),
                new Item("Keyboard"),
                new Item("Gopro"),
                new Item("Cloth"),
                new Item("Curtains"),
                new Item("Bottle"),
            };

            return list;
        }

        public async Task<ObservableCollection<Move>> GetMoves()
        {
  
            return user.getMoves();
        }

        public string GetUsername() => _uname;

        public async Task<bool> Login(string username, string password)
        {
            _uname = username;

            await Task.FromResult(true);

            return true;
        }

        public async Task<String> CreateMove(Move move)
        {
            idCounter += 1;
                
            return await Task.FromResult(idCounter.ToString());
        }

        public async Task<List<Item>> GetItems(string moveId, string boxId)
        {
            return await GetItems(moveId);
        }

        public Task<string> CreateBox(string moveId, Box box)
        {
            box.id = "id32";
            return Task.FromResult(box.id);
        }

        public async Task<List<Room>> GetRooms(string moveId)
        {
            await Task.Delay(2000);

            List<Room> list = new List<Room >
            {
                new Room("Forks"   , "123"),
                new Room("Chairs"  , "456"),
                new Room("Gameboy" , "789"),
                new Room("Computer", "012")
            };

            return list;
        }

        public Task<bool> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
