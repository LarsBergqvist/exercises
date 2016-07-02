﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RegistrationSystem.Contract.Resources
{
    public class Doctor : IHospitalResource
    {
        public Doctor(string name)
        {
            Name = name;
            Roles = new List<RoleType>();
        }

        public string Name { get; private set; }

        public IList<RoleType> Roles { get; set; }

        public string Id { get; set; }
    }
}