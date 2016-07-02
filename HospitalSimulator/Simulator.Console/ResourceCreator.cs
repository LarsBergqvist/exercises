using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace HospitalSimulator
{
    public class ResourceCreator
    {
        public static void AddSimulatedResources(IResourcesRepository resourcesRepository)
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

            var elekta = new TreatmentMachine() {Name = "Elekta", Capability = MachineCapabilityType.Advanced};
            var varian = new TreatmentMachine() {Name = "Varian", Capability = MachineCapabilityType.Advanced};
            var mm50 = new TreatmentMachine() {Name = "MM50", Capability = MachineCapabilityType.Simple};

            resourcesRepository.AddRoom(new TreatmentRoom("One") {TreatmentMachine = elekta});
            resourcesRepository.AddRoom(new TreatmentRoom("Two") {TreatmentMachine = varian});
            resourcesRepository.AddRoom(new TreatmentRoom("Three") {TreatmentMachine = mm50});
            resourcesRepository.AddRoom(new TreatmentRoom("Four"));
            resourcesRepository.AddRoom(new TreatmentRoom("Five"));

        }
    }
}
