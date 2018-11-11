using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualBoxManager;
using VisualBoxManager.Objects.Validations;
using Xamarin.Forms;
using VisualBoxManager.ViewModels;
using static VisualBoxManager.Box;

namespace NUnit.Tests1
{


    [TestFixture]
    public partial class TestClass
    {


        private Auth auth;
        private User user;
        private string EmailAdd;
        private Box box;
        private Item item;
        private Move move;
        private Room room;
        private IConnectionService _connectionService;



        [SetUp]
        public void Setup()
        {
            auth = Auth.Instance();
            user = User.Instance();
            


            EmailAdd = "test@test.com";
            _connectionService = ConnectionService.Instance();

        }



        /// <summary>
        /// ///////////////////////////////////////
        /// Auth Tests
        /// </summary>
        [Test]
        public void Test_authenticated()
        {
            bool test = auth.setAuthenticated("TestHelloWorld");
            Assert.IsTrue(auth.authenticated());
        }

        [Test]
        public void Test_getSessionId_Normal()
        {
            bool test = auth.setAuthenticated("TestHelloWorld");
            Assert.AreSame(auth.getSessionId(), "TestHelloWorld");
        }

        [Test]
        public void Test_getSessionId_Null()
        {
            bool test = auth.setAuthenticated(null);
            Assert.That(auth.setAuthenticated(null), Throws.TypeOf<NullReferenceException>());
        }


        [Test]
        public void Test_getSessionId_Empty()
        {
            bool test = auth.setAuthenticated("");
            Assert.AreSame(auth.getSessionId(), "");
        }


        [Test]
        public void Test_clearAuth()
        {
            auth.clearAuth();
            Assert.IsFalse(auth.authenticated());

        }


        [Test]
        public void Test_email()
        {

            string hello = user.getEmail();
            System.Diagnostics.Debug.WriteLine("8DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD=");
            System.Diagnostics.Debug.WriteLine(hello);

            //Assert.AreSame(, "");
        }


        /// <summary>
        /// ////////////////////////////////////////////////
        /// User Tests
        /// </summary>
        /// 

        [Test]
        public void test_addOrUpdateMove()
        {
            Move move = new Move();


            user.getMoves();


            user.addOrUpdateMove(move);

        }


        [Test]
        public async Task test_addUserAsync()

        {

            IConnectionService connect = new MockConnectionService();

            string FirstName = "James";
            string LastName = "Matthews";
            string Email = "test@test.com";
            string Password = "12345";


            await connect.AddUser(FirstName, LastName, Email, Password);


            string test = user.getEmail();
            Assert.AreSame(test, "test@test.com");
        }


        [Test]
        public void testGetMoves()
        {
            Move move = new Move();


        }




        /// <summary>
        /// Validatior tests
        /// </summary>
        [Test]
        public void Test_emailisVaild()
        {
            string email = "test@test.com";
            bool test = (Validator.ValidateEmail(email).Error);
            Assert.IsFalse(test);
        }

        [Test]
        public void Test_EmailIsInvaild()
        {
            string email = "tesstom";

            bool test = Validator.ValidateEmail(email).Error;

            Assert.IsTrue(test);

        }

        [Test]
        public void Test_EmailIssInvaild()
        {
            string email = "tesstom@    ";

            bool test = Validator.ValidateEmail(email).Error;

            Assert.IsTrue(test);

        }





        [Test]
        public void Test_InvalidName()
        {
            string name = "1";
            bool test = Validator.ValidateName(name).Error;
            Assert.IsTrue(test);

        }
        [Test]
        public void Test_VailidName()
        {
            string name = "Jdddoe";
            bool test = Validator.ValidateName(name).Error;
            Assert.IsFalse(test);

        }


        [Test]
        public void Test_VailidPassword()
        {
            string FirstEntery = "12345678";
            string SecondEntery = "12345678";
            bool test = Validator.ValidatePass(FirstEntery, SecondEntery).Error;
            Assert.IsFalse(test);
        }

        [Test]
        public void Test_InvalidSecondPassword()
        {
            string FirstEntery = "12345678";
            string SecondEntery = "2";
            bool test = Validator.ValidatePass(FirstEntery, SecondEntery).Error;
            Assert.IsTrue(test);
        }

        [Test]
        public void Test_InvalidFirstPassword()
        {
            string FirstEntery = "1";
            string SecondEntery = "12345678";
            bool test = Validator.ValidatePass(FirstEntery, SecondEntery).Error;
            Assert.IsTrue(test);
        }

        [Test]
        public void Test_PasswordDoesNotMeetRequirements()
        {
            string FirstEntery = "2222";
            string SecondEntery = "2222";
            bool test = Validator.ValidatePass(FirstEntery, SecondEntery).Error;
            Assert.IsTrue(test);
        }

        /// <summary>
        /// Box tests
        /// </summary>

        [Test]
        public void Test_BoxName_high()
        {
            box = new Box("test", BoxPriority.High, null);
            string test = "test";

            Assert.AreEqual(box.name, test);
        }

        [Test]
        public void Test_BoxName_medium()
        {
            box = new Box("test", BoxPriority.Medium, null);
            string test = "test";

            Assert.AreEqual(box.name, test);
        }

        [Test]
        public void Test_BoxName_low()
        {
            box = new Box("test", BoxPriority.Low, null);
            string test = "test";

            Assert.AreEqual(box.name, test);
        }

        private void AddUser(string v1, string v2, string v3, string v4)
        {
            throw new NotImplementedException();
        }

        // Move tests

        [Test]
        public void test_OnSave() {
            CreateMoveViewModel testviewmove = new CreateMoveViewModel(_connectionService);
            testviewmove.CreatNewMoveCommand.Execute("");
            Assert.AreEqual(Validator.ValidateName("").Message, testviewmove.ErrMoveName);
        }

        //[Test]
        //public void test_OnSave()
        //{
        //    CreateMoveViewModel testviewmove = new CreateMoveViewModel(_connectionService);
        //    testviewmove.CreatNewMoveCommand.Execute("");
        //    Assert.AreEqual(Validator.ValidateName("").Message, testviewmove.ErrMoveName);
        //}








        /// this test is bugged, the code is correct but there is an issue with the code elsewhere in the application which causes this test to fail.
        /// The connection service does successfully call the server and the server does respond.
        [Test]
        public async Task test_login()
        {
            ConnectionService testConnect = ConnectionService.Instance();
            bool authorised = await testConnect.Login("test@test.com", "12345678");
            Assert.IsTrue(authorised);

        }

        [Test]
        public async Task test_Invalid_login()
        {
            ConnectionService testConnect = ConnectionService.Instance();
            bool authorised = await testConnect.Login("tt", "21331");
            Assert.IsFalse(authorised);

        }


        [Test]
        public async Task test_add_move()
        {

            //ConnectionService connect = new ConnectionService;

            await _connectionService.AddUser("test", "test", "test@test1.com", "12345678");
            // string name = "test";


            //bool test = await MoveController.AddMove(name, );


        }

        [Test]
        public void Test_DestinationRoomID_Set()
        {
            box.DestinationRoomID = "Room1";
            Assert.AreEqual(box.DestinationRoomID, "Room1");
        }

        [Test]
        public void Test_BoxPriority_Set()
        {
            box.Priority = Box.BoxPriority.High;
            Assert.AreEqual(box.Priority, Box.BoxPriority.High);
        }

        [Test]
        public void Test_item_Set()
        {
            item.name = "Chair"; 
            Assert.AreEqual(item.name, "Chair");
        }
        [Test]
        public void Test_move_Set()
        {
            
            move.name = "James";
            Assert.AreEqual(move.name, "James");
        }
        [Test]
        public void Test_id_Set()
        {
            move.id = "25";
            Assert.AreEqual(move.id, "25");
        }
        [Test]
        public void Test_room_Set()
        {
            room.name = "new";
            
            Assert.AreEqual(room.name, "new");
        }


    }
}
