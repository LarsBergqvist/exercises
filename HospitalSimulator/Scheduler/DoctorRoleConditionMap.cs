using System.Collections.Generic;
using Consultations.Contract;
using Resources.Contract;

namespace Scheduler
{
    public class DoctorRoleConditionMap
    {
        private static readonly Dictionary<ConditionType, RoleType> ConditionToRoleMap = new Dictionary<ConditionType, RoleType>
            {
                {ConditionType.Flu, RoleType.GeneralPractioner},
                {ConditionType.CancerBreast, RoleType.Oncologist},
                {ConditionType.CancerHeadNeck, RoleType.Oncologist}
            };

        public static bool CanRoleHandeCondition(RoleType roleType, ConditionType condition)
        {
            if (ConditionToRoleMap.ContainsKey(condition))
            {
                if (ConditionToRoleMap[condition] == roleType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
