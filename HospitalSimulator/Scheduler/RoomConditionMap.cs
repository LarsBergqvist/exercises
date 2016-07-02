using Consultations.Contract;
using Resources.Contract;

namespace Scheduler
{
    public class RoomConditionMap
    {
        public static bool IsSuitableForConditionTreatment(TreatmentMachine machine, ConditionType condition)
        {
            if (condition == ConditionType.Flu)
            {
                return true;
            }

            if (machine == null)
                return false;

            if (condition == ConditionType.CancerBreast)
            {
                if ((machine.Capability == MachineCapabilityType.Simple) ||
                    (machine.Capability == MachineCapabilityType.Advanced))
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
                if ((machine.Capability == MachineCapabilityType.Advanced))
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

    }
}
