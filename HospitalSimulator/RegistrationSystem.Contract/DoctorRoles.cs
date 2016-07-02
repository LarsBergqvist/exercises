using System.Collections.Generic;

namespace RegistrationSystem.Contract
{
    public enum RoleType
    {
        Oncologist,
        GeneralPractioner
    }

    public class DoctorRoles
    {
        private readonly Dictionary<ConditionType, RoleType> _conditionToRoleMap;

        public DoctorRoles()
        {
            _conditionToRoleMap = new Dictionary<ConditionType, RoleType>();
            _conditionToRoleMap.Add(ConditionType.Flu, RoleType.GeneralPractioner);
            _conditionToRoleMap.Add(ConditionType.CancerBreast, RoleType.Oncologist);
            _conditionToRoleMap.Add(ConditionType.CancerHeadNeck, RoleType.Oncologist);
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
