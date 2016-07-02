using System.Collections.Generic;

namespace Resources.Contract
{
    public class Doctor
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