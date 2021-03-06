﻿using System.Collections.Generic;

namespace Consultations.Contract
{
    public interface IConsultationsRepository
    {
        Consultation AddConsultation(Consultation consultation);
        ICollection<Consultation> GetAllConsultations();
        ICollection<PatientRecord> GetAllRegisteredPatients();
        void RemoveConsultation(string id);
        void Clear();
    }
}
