using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RegistrationSystem.Contract
{
    public class PatientRecord
    {
        public PatientRecord(string name)
        {
            Name = name;
            BookedConsultations = new Collection<Consultation>();
        }
        public string Name { get; set; }
        public ICollection<Consultation> BookedConsultations { get; set; }
    }
}
