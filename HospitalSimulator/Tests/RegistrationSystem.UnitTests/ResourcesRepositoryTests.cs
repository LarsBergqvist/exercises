using NUnit.Framework;
using RegistrationSystem.DI;
using Resources.Contract;

namespace RegistrationSystem.UnitTests
{
    [TestFixture]
    class ResourcesRepositoryTests
    {
        private IResourcesRepository _resourceRepository;
        private readonly Factory _factory = new Factory();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _factory.BindDependencies();
            _resourceRepository = _factory.ResourcesRepository;
        }

        [SetUp]
        public void SetUp()
        {
            _resourceRepository.Clear();
        }

        [Test]
        public void Test_AddDoctors()
        {
            _resourceRepository.AddDoctor(new Doctor("Kjell1"));
            _resourceRepository.AddDoctor(new Doctor("Kjell2"));
            Assert.IsTrue(_resourceRepository.GetAllDoctors().Count == 2);
        }

        [Test]
        public void Test_AddRooms()
        {
            _resourceRepository.AddRoom(new TreatmentRoom("Room1"));
            _resourceRepository.AddRoom(new TreatmentRoom("Room2"));
            Assert.IsTrue(_resourceRepository.GetAllRooms().Count == 2);
            _resourceRepository.Clear();
            Assert.IsTrue(_resourceRepository.GetAllDoctors().Count == 0);
            Assert.IsTrue(_resourceRepository.GetAllRooms().Count == 0);
        }

        [Test]
        public void Test_Clear()
        {
            _resourceRepository.AddDoctor(new Doctor("Kjell1"));
            _resourceRepository.AddRoom(new TreatmentRoom("Room1"));
            Assert.IsTrue(_resourceRepository.GetAllDoctors().Count == 1);
            Assert.IsTrue(_resourceRepository.GetAllRooms().Count == 1);
            _resourceRepository.Clear();
            Assert.IsTrue(_resourceRepository.GetAllDoctors().Count == 0);
            Assert.IsTrue(_resourceRepository.GetAllRooms().Count == 0);           
        }

        [Test]
        public void Test_GetResourceByName()
        {
            Assert.IsNull(_resourceRepository.GetDoctorByName("Kjell1"));
            _resourceRepository.AddDoctor(new Doctor("Kjell1"));
            Assert.IsNotNull(_resourceRepository.GetDoctorByName("Kjell1"));
            Assert.IsNull(_resourceRepository.GetRoomByName("kjell1"));

            Assert.IsNull(_resourceRepository.GetRoomByName("Room1"));
            _resourceRepository.AddRoom(new TreatmentRoom("Room1"));
            Assert.IsNotNull(_resourceRepository.GetRoomByName("Room1"));
            Assert.IsNull(_resourceRepository.GetRoomByName("room1"));
        }

        [Test]
        public void Test_AddDoctorsWithSameName()
        {
            _resourceRepository.AddDoctor(new Doctor("Kjell"));

            Assert.Throws<ResourcesRepositoryException>(() => _resourceRepository.AddDoctor(new Doctor("Kjell")));
        }

        [Test]
        public void Test_RoomWithSameName()
        {
            _resourceRepository.AddRoom(new TreatmentRoom("Room"));

            Assert.Throws<ResourcesRepositoryException>(() => _resourceRepository.AddRoom(new TreatmentRoom("Room")));
        }

    }
}
