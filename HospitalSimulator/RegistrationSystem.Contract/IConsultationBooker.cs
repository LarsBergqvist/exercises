using System;
using System.Collections.Generic;

namespace RegistrationSystem.Contract
{
    public class ConsultationRequest
    {
        public string PatientName { get; set; }
        public ConditionType Condition { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class ConsultationRequestResult
    {
        public bool Succesful { get; set; }
        public string ErrorMessage { get; set; }
        public Consultation BookedConsultation { get; set; }
    }

    public interface IConsultationBooker
    {
        ConsultationRequestResult RequestFirstAvailableConsultation(ConsultationRequest consultationRequest);
        ICollection<PatientRecord> GetAllRegisteredPatients();
        ICollection<Consultation> GetAllScheduledConsultations(DateTime fromDate);
        void RemovedScheduledConsultation(string id);
    }
}