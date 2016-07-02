using System.Linq;
using NUnit.Framework;
using RegistrationSystem.Contract;
using RegistrationSystem.DI;

namespace RegistrationSystem.UnitTests
{
    [TestFixture]
    public class ConsultationRepositoryTests
    {
        private IConsultationsRepository _consultationsRepository;
        private readonly Factory _factory = new Factory();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _factory.BindDependencies();
            _consultationsRepository = _factory.ConsultationsRepository;
        }

        [SetUp]
        public void SetUp()
        {
            _consultationsRepository.Clear();
        }

        [Test]
        public void Test_AddConsultation()
        {
            _consultationsRepository.AddConsultation(new Consultation() {PatientName = "knut1"});
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 1);
            _consultationsRepository.AddConsultation(new Consultation() { PatientName = "knut2" });
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 2);
        }

        [Test]
        public void Test_GetAllRegisteredPatients()
        {
            _consultationsRepository.AddConsultation(new Consultation() { PatientName = "knut" });
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 1);
            _consultationsRepository.AddConsultation(new Consultation() { PatientName = "knut" });
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 1);
            _consultationsRepository.AddConsultation(new Consultation() { PatientName = "olle" });
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 2);

            var patientRecords = _consultationsRepository.GetAllRegisteredPatients();
            Assert.IsTrue(patientRecords.First(record => record.Name == "knut").BookedConsultations.Count == 2);
            Assert.IsTrue(patientRecords.First(record => record.Name == "olle").BookedConsultations.Count == 1);
        }

        [Test]
        public void Test_Remove()
        {
            _consultationsRepository.RemoveConsultation("4");


            var cons1 = _consultationsRepository.AddConsultation(new Consultation() { PatientName = "knut" });
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 1);
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 1);
            var cons2 = _consultationsRepository.AddConsultation(new Consultation() { PatientName = "knut" });
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 2);
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 1);
            var cons3 = _consultationsRepository.AddConsultation(new Consultation() { PatientName = "olle" });
            Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 3);
            Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 2);

           _consultationsRepository.RemoveConsultation(cons1.Id);
           Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 2);
           Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 2);

            _consultationsRepository.RemoveConsultation(cons2.Id);
           Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 1);
           Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 1);

            _consultationsRepository.RemoveConsultation(cons3.Id);
           Assert.IsTrue(_consultationsRepository.GetAllConsultations().Count == 0);
           Assert.IsTrue(_consultationsRepository.GetAllRegisteredPatients().Count == 0);
        }
    }
}
