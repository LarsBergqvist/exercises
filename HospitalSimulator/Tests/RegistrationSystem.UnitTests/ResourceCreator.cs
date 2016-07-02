using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace RegistrationSystem.UnitTests
{
    public class ResourceCreator
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

            SetupRoomsSet1(resourcesRepository);
        }

        private void SetupRoomsSet1(IResourcesRepository resourcesRepository)
        {
            var elekta = new TreatmentMachine() { Name = "Elekta", Capability = MachineCapabilityType.Advanced };
            var varian = new TreatmentMachine() { Name = "Varian", Capability = MachineCapabilityType.Advanced };
            var mm50 = new TreatmentMachine() { Name = "MM50", Capability = MachineCapabilityType.Simple };

            resourcesRepository.AddRoom(new TreatmentRoom("One") { TreatmentMachine = elekta });
            resourcesRepository.AddRoom(new TreatmentRoom("Two") { TreatmentMachine = varian });
            resourcesRepository.AddRoom(new TreatmentRoom("Three") { TreatmentMachine = mm50 });
            resourcesRepository.AddRoom(new TreatmentRoom("Four"));
            resourcesRepository.AddRoom(new TreatmentRoom("Five"));            
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

        public void SetupResourcesSet3(IResourcesRepository resourcesRepository)
        {
            // No oncologists
            var doctor1 = new Doctor("Julia");
            doctor1.Roles.Add(RoleType.GeneralPractioner);
            resourcesRepository.AddDoctor(doctor1);
        }


    }
}
