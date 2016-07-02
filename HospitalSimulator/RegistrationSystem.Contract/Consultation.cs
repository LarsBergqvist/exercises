using System;
using RegistrationSystem.Contract.Resources;

namespace RegistrationSystem.Contract
{
    public class Consultation
    {
        public DateTime ConsultationDate { get; set; }
        public Doctor Doctor { get; set; }
        public TreatmentRoom Room { get; set; }
        public String PatientName { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Id { get; set; }
        public ConditionType ConditionType { get; set; }
    }
}