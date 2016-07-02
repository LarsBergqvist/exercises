namespace RegistrationSystem.Contract.Resources
{
    public enum MachineCapabilityType
    {
        Simple,
        Advanced
    }

    public class TreatmentMachine
    {
        public string Name { get; set; }
        public MachineCapabilityType Capability { get; set; }
    }

    public class TreatmentRoom : IHospitalResource
    {
        public TreatmentRoom(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public TreatmentMachine TreatmentMachine { get; set; }

        public bool HasTreatmentMachine() { return (TreatmentMachine != null);}

        public bool IsSuitableForConditionTreatment(ConditionType condition)
        {
            if (condition == ConditionType.Flu)
            {
                return true;
            }

            if (TreatmentMachine == null)
                return false;

            if (condition == ConditionType.CancerBreast)
            {
                if ((TreatmentMachine.Capability == MachineCapabilityType.Simple) ||
                    (TreatmentMachine.Capability == MachineCapabilityType.Advanced))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (condition == ConditionType.CancerHeadNeck)
            {
                if ((TreatmentMachine.Capability == MachineCapabilityType.Advanced))
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            else
            {
                return false;
            }
        }

        public string Id { get; set; }
    }
}