using System.Collections.Generic;
using Consultations.Contract;
using Resources.Contract;

namespace Scheduler
{
    public class DoctorRoleConditionMap
    {
        private readonly Dictionary<ConditionType, RoleType> _conditionToRoleMap;

        public DoctorRoleConditionMap()
        {
            _conditionToRoleMap = new Dictionary<ConditionType, RoleType>
            {
                {ConditionType.Flu, RoleType.GeneralPractioner},
                {ConditionType.CancerBreast, RoleType.Oncologist},
                {ConditionType.CancerHeadNeck, RoleType.Oncologist}
            };
        }

        public bool CanRoleHandeCondition(RoleType roleType, ConditionType condition)
        {
            if (_conditionToRoleMap.ContainsKey(condition))
            {
                if (_conditionToRoleMap[condition] == roleType)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
