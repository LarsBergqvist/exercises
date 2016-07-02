using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace RegistrationSystem.UnitTests
{
    public class TestDataGenerator
    {
        public void SetupResourcesSet1(IResourcesRepository resourcesRepository)
        {
            var doctor1 = new Doctor("John");
            doctor1.Roles.Add(RoleType.Oncologist);
            resourcesRepository.AddDoctor(doctor1);

            var doctor2 = new Doctor("Anna");
            doctor2.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor2);

            var doctor3 = new Doctor("Peter");
            doctor3.Roles.Add(RoleType.Oncologist);
            doctor3.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor3);

            var elekta = new TreatmentMachine() { Name = "Elekta", Capability = MachineCapabilityType.Advanced };
            var varian = new TreatmentMachine() { Name = "Varian", Capability = MachineCapabilityType.Advanced };
            var mm50 = new TreatmentMachine() { Name = "MM50", Capability = MachineCapabilityType.Simple };

            var room1 = new TreatmentRoom("One") { TreatmentMachine = elekta };
            resourcesRepository.AddRoom(room1);
            var room2 = new TreatmentRoom("Two") { TreatmentMachine = varian };
            resourcesRepository.AddRoom(room2);
            var room3 = new TreatmentRoom("Three") { TreatmentMachine = mm50 };
            resourcesRepository.AddRoom(room3);
            var room4 = new TreatmentRoom("Four");
            resourcesRepository.AddRoom(room4);
            var room5 = new TreatmentRoom("Five");
            resourcesRepository.AddRoom(room5);
        }

        public void SetupResourcesSet2(IResourcesRepository resourcesRepository)
        {
            SetupResourcesSet1(resourcesRepository);

            // Add additional GeneralPractioners
            var doctor4 = new Doctor("Julia");
            doctor4.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor4);
            var doctor5 = new Doctor("Bert");
            doctor5.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor5);
            var doctor6 = new Doctor("Maria");
            doctor6.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor6);
        }

    }
}
