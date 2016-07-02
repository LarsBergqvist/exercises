namespace Resources.Contract
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

    public class TreatmentRoom
    {
        public TreatmentRoom(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }
        public TreatmentMachine TreatmentMachine { get; set; }

        public bool HasTreatmentMachine() { return (TreatmentMachine != null);}

        public string Id { get; set; }
    }
}