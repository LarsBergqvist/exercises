﻿using System.Collections.Generic;

namespace RegistrationSystem.Contract
{
    public class DoctorRoles
    {
        private readonly Dictionary<ConditionType, RoleType> _conditionToRoleMap;

        public DoctorRoles()
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
