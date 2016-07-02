using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Consultations.Contract;

namespace RegistrationSystem.Fakes
{
    public class ConsultationsRepository : IConsultationsRepository
    {
        private readonly ICollection<Consultation> _consultations;
        private readonly Dictionary<string, PatientRecord> _patientRecordLookup;
        private int _nextId = 1;
 
        public ConsultationsRepository()
        {
            _consultations = new Collection<Consultation>();
            _patientRecordLookup = new Dictionary<string, PatientRecord>();
        }

        public Consultation AddConsultation(Consultation consultation)
        {
            consultation.Id = (_nextId++).ToString();
            _consultations.Add(consultation);
            var patientName = consultation.PatientName;
            var patientRecord = new PatientRecord(patientName);
            if (!_patientRecordLookup.ContainsKey(patientName))
            {
                _patientRecordLookup.Add(patientName, patientRecord);
            }
            _patientRecordLookup[patientName].BookedConsultations.Add(consultation);

            return consultation;
        }

        public ICollection<Consultation> GetAllConsultations()
        {
            return _consultations;
        }

        public ICollection<PatientRecord> GetAllRegisteredPatients()
        {
            return _patientRecordLookup.Select(patient => patient.Value).ToList();
        }

        public void RemoveConsultation(string id)
        {
            var consultation = _consultations.FirstOrDefault(x => x.Id == id);
            if (consultation != null)
            {
                var patientName = consultation.PatientName;
                _consultations.Remove(consultation);
                var consOnPatientRecord = _patientRecordLookup[patientName].BookedConsultations.FirstOrDefault(x => x.Id == id);
                _patientRecordLookup[patientName].BookedConsultations.Remove(consOnPatientRecord);
                if (_patientRecordLookup[patientName].BookedConsultations.Count == 0)
                {
                    _patientRecordLookup.Remove(patientName);
                }
            }

        }

        public void Clear()
        {
            _consultations.Clear();
            _patientRecordLookup.Clear();
        }
    }
}