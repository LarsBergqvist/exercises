using System;
using NUnit.Framework;
using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;
using RegistrationSystem.DI;

namespace RegistrationSystem.UnitTests
{
    [TestFixture]
    public class ResourceCalendarTests
    {
        private IResourcesRepository _resourceRepository;
        private TestDataGenerator _testDataGenerator;
        private IResourceCalendar _resourceCalendar;
        private IConsultationsRepository _consultationsRepository;
        private DateTime _today;
        private readonly Factory _factory = new Factory();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _factory.BindDependencies();
            _testDataGenerator = new TestDataGenerator();
            _resourceRepository = _factory.ResourcesRepository;
            _resourceCalendar = _factory.ResourceCalendar;
            _consultationsRepository = _factory.ConsultationsRepository;
        }

        [SetUp]
        public void SetUp()
        {
            _resourceRepository.Clear();
            _resourceCalendar.Clear();
            _consultationsRepository.Clear();
            _today = DateTime.Now.Date;
        }

        [Test]
        public void Test_BookFirstAvailableDate_busy_doctors_and_rooms()
        {
            _testDataGenerator.SetupResourcesSet1(_resourceRepository);

            var startDate = DateTime.Now.Date;
            _resourceCalendar.Generate(startDate, startDate.AddDays(365));

            var tomorrow = DateTime.Now.Date.AddDays(1);

            var consultation1 = _resourceCalendar.ScheduleFirstAvailable("Knut1", _today, ConditionType.CancerHeadNeck, tomorrow);
            Assert.IsNotNull(consultation1);
            Assert.IsTrue(consultation1.ConsultationDate == tomorrow);
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 1);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 1);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 1);

            var consultation2 = _resourceCalendar.ScheduleFirstAvailable("Knut2", _today, ConditionType.CancerHeadNeck, tomorrow);
            Assert.IsNotNull(consultation2);
            Assert.IsTrue(consultation2.ConsultationDate == tomorrow);
            Assert.IsFalse(consultation1.Doctor.Name == consultation2.Doctor.Name);
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 2);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 2);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 2);

            // There are only 2 oncologists and 2 advanced cancer treatment rooms in the test set
            // Thus, we can only book two advanced cancer treatments per day
            var consultation3 = _resourceCalendar.ScheduleFirstAvailable("Bella", _today, ConditionType.CancerHeadNeck, tomorrow);
            Assert.IsNotNull(consultation3);
            Assert.IsTrue(consultation3.ConsultationDate == tomorrow.AddDays(1));
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 3);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 3);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 3);
        }

        [Test]
        public void Test_BookFirstAvailableDate_busy_rooms()
        {
            //
            // Use a resource set where there are 3 extra general practioner doctors (5 in total)
            // and 5 rooms
            //
            _testDataGenerator.SetupResourcesSet2(_resourceRepository);

            var startDate = DateTime.Now.Date;
            _resourceCalendar.Generate(startDate, startDate.AddDays(365));

            var tomorrow = DateTime.Now.Date.AddDays(1);

            //
            // Book all rooms with flu patients for 5 days
            //
            for (int bookingNum = 0; bookingNum < 5*5; bookingNum++)
            {
                var fluConsultation = _resourceCalendar.ScheduleFirstAvailable(string.Format("Knut{0}",bookingNum), _today, ConditionType.Flu, tomorrow);
                Assert.IsNotNull(fluConsultation);
            }

            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 25);

            var consultation1 = _resourceCalendar.ScheduleFirstAvailable("Knut", _today, ConditionType.CancerHeadNeck, tomorrow);
            // The first available date should be after 5 days from tomorrow as all rooms but not all doctors are booked until then
            Assert.IsNotNull(consultation1);
            Assert.IsTrue(consultation1.ConsultationDate == tomorrow.AddDays(5));
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 26);

        }

        [Test]
        public void Test_BookFirstAvailableDate_calender_too_small()
        {
            _testDataGenerator.SetupResourcesSet1(_resourceRepository);

            var startDate = DateTime.Now.Date;
            const int calenderSize = 365;
            _resourceCalendar.Generate(startDate, startDate.AddDays(calenderSize));

            var tomorrow = DateTime.Now.Date.AddDays(1);

            for (int i = 1; i <= 1000; i++)
            {
                var consultation = _resourceCalendar.ScheduleFirstAvailable("Knut", _today, ConditionType.CancerHeadNeck, tomorrow);
                if (i <= (calenderSize-1) * 2)
                {
                    Assert.IsNotNull(consultation, string.Format("Booking attempt no: {0}", i));
                }
                else
                {
                    Assert.IsNull(consultation);
                }
            }
        }

        [Test]
        public void Test_BookEarliestDateOutsideCalendar()
        {
            _testDataGenerator.SetupResourcesSet1(_resourceRepository);
            var startDate = DateTime.Now.Date;
            _resourceCalendar.Generate(startDate, startDate.AddDays(7));

            Assert.Throws<ResourceCalendarException>(
                () => _resourceCalendar.ScheduleFirstAvailable("Knut", _today, ConditionType.CancerHeadNeck, DateTime.Now.Date.AddDays(8))
                );
        }

        [Test]
        public void Test_CalendarCanBeRegeneratedFromConsultationsRepository()
        {
            _testDataGenerator.SetupResourcesSet1(_resourceRepository);

            var startDate = DateTime.Now.Date;

            var tomorrow = DateTime.Now.Date.AddDays(1);

            //
            // Start with an empty calendar and schedule two consultations
            //
            _resourceCalendar.Generate(startDate, startDate.AddDays(365));
            var consultation1 = _resourceCalendar.ScheduleFirstAvailable("Knut1", _today, ConditionType.CancerHeadNeck,tomorrow);
            var consultation2 = _resourceCalendar.ScheduleFirstAvailable("Knut2", _today, ConditionType.CancerHeadNeck,tomorrow);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 2);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 2);

            //
            // Clear the calendar and regenerate it
            // The persisted consultations and the resources should recreate the calendar
            //
            _resourceCalendar.Clear();
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 0);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 0);
            _resourceCalendar.Generate(startDate, startDate.AddDays(365));
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfDoctors() == 2);
            Assert.IsTrue(_resourceCalendar.NumberOfSchedulationsOfRooms() == 2);

        }

    }
}
