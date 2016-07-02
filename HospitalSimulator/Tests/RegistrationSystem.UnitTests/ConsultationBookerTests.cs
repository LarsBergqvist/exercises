﻿using System;
using System.Linq;
using NUnit.Framework;
using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;
using RegistrationSystem.DI;

namespace RegistrationSystem.UnitTests
{
    [TestFixture]
    class ConsultationBookerTests
    {
        private IConsultationBooker _consultationBooker;
        private IResourceCalendar _resourceCalendar;
        private readonly Factory _factory = new Factory();

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _factory.BindDependencies();
            _consultationBooker = _factory.ConsultationBooker;
            var resourceRepository = _factory.ResourcesRepository;
            resourceRepository.Clear();
            var testDataGenerator = new ResourceCreator();
            testDataGenerator.SetupResourcesSet1(resourceRepository);
            _resourceCalendar = _factory.ResourceCalendar;
        }

        [SetUp]
        public void SetUp()
        {
            _factory.ConsultationsRepository.Clear();
        }

        [Test]
        public void Test_EarliestConsultationDateIsTomorrow()
        {
            var today = DateTime.Now.Date;
            _resourceCalendar.Generate(today, today.AddDays(365));

            var request = new ConsultationRequest()
            {
                Condition = ConditionType.Flu,
                PatientName = "Pelle",
                RegistrationDate = today
            };
            var result = _consultationBooker.RequestFirstAvailableConsultation(request);

            Assert.IsTrue(result.Succesful);
            Assert.IsTrue(result.BookedConsultation.ConsultationDate == today.AddDays(1));
            Assert.IsTrue(result.BookedConsultation.RegistrationDate == today);
            Assert.IsTrue(DoctorHasMatchingRole(result.BookedConsultation.Doctor, RoleType.GeneralPractioner));
        }


        [Test]
        public void Test_CorrectRoleAndMachineTypeBooked()
        {
            RequestTwoCancerConsultations();
        }

        [Test]
        public void Test_FullyBookedTomorrow()
        {
            RequestTwoCancerConsultations();

            var today = DateTime.Now.Date;
            //
            // As the test set only has two doctors and the earliest consultation date is tomorrow,
            // the third cancer consultation will be the day after tomorrow
            //
            var request3 = new ConsultationRequest()
            {
                Condition = ConditionType.CancerHeadNeck,
                PatientName = "Per",
                RegistrationDate = today
            };
            var result3 = _consultationBooker.RequestFirstAvailableConsultation(request3);

            Assert.IsTrue(result3.Succesful);
            Assert.IsTrue(result3.BookedConsultation.ConsultationDate == today.AddDays(2));
            Assert.IsTrue(DoctorHasMatchingRole(result3.BookedConsultation.Doctor, RoleType.Oncologist));
            Assert.IsTrue(result3.BookedConsultation.Room.TreatmentMachine.Capability == MachineCapabilityType.Advanced);
        }

        [Test]
        public void Test_GetAllRegisteredPatients()
        {
            RequestTwoCancerConsultations();
            var patientRecords = _consultationBooker.GetAllRegisteredPatients();
            Assert.IsTrue(patientRecords.Count == 2);
            Assert.IsTrue(patientRecords.First(record => record.Name == "Pelle").BookedConsultations.Count == 1);
            Assert.IsTrue(patientRecords.First(record => record.Name == "Anna").BookedConsultations.Count == 1);
        }

        [Test]
        public void Test_GetAllScheduledConsultations()
        {
            RequestTwoCancerConsultations();
            var today = DateTime.Now.Date;
            Assert.IsTrue(_consultationBooker.GetAllScheduledConsultations(today).Count == 2);
            // The consultations should be booked tomorrow
            Assert.IsTrue(_consultationBooker.GetAllScheduledConsultations(today.AddDays(1)).Count == 2);
            // No consultations the day after tomorrow
            Assert.IsTrue(_consultationBooker.GetAllScheduledConsultations(today.AddDays(2)).Count == 0);
        }

        private void RequestTwoCancerConsultations()
        {
            var today = DateTime.Now.Date;
            _resourceCalendar.Generate(today, today.AddDays(365));

            var request1 = new ConsultationRequest()
            {
                Condition = ConditionType.CancerHeadNeck,
                PatientName = "Pelle",
                RegistrationDate = today
            };
            var result1 = _consultationBooker.RequestFirstAvailableConsultation(request1);

            Assert.IsTrue(result1.Succesful);
            Assert.IsTrue(result1.BookedConsultation.ConsultationDate == today.AddDays(1));
            Assert.IsTrue(result1.BookedConsultation.RegistrationDate == today);
            Assert.IsTrue(DoctorHasMatchingRole(result1.BookedConsultation.Doctor, RoleType.Oncologist));
            Assert.IsTrue(result1.BookedConsultation.Room.TreatmentMachine.Capability == MachineCapabilityType.Advanced);

            var request2 = new ConsultationRequest()
            {
                Condition = ConditionType.CancerBreast,
                PatientName = "Anna",
                RegistrationDate = today
            };
            var result2 = _consultationBooker.RequestFirstAvailableConsultation(request2);

            Assert.IsTrue(result2.Succesful);
            Assert.IsTrue(result2.BookedConsultation.ConsultationDate == today.AddDays(1));
            Assert.IsTrue(result2.BookedConsultation.RegistrationDate == today);
            Assert.IsTrue(DoctorHasMatchingRole(result2.BookedConsultation.Doctor, RoleType.Oncologist));
            Assert.IsTrue((result1.BookedConsultation.Room.TreatmentMachine.Capability == MachineCapabilityType.Advanced) ||
                (result1.BookedConsultation.Room.TreatmentMachine.Capability == MachineCapabilityType.Simple));
        }

        private bool DoctorHasMatchingRole(Doctor doctor, RoleType roleToMatch)
        {
            return doctor.Roles.Any(role => role == roleToMatch);
        }
    }
}
