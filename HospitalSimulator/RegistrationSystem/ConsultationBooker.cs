using System;
using System.Collections.Generic;
using System.Linq;
using RegistrationSystem.Contract;

namespace RegistrationSystem
{
    public class ConsultationBooker : IConsultationBooker
    {
        private readonly IResourceCalendar _resourceCalendar;
        private readonly IConsultationsRepository _consultationsRepository;

        public ConsultationBooker(IResourceCalendar resourceCalendar,IConsultationsRepository consultationsRepository)
        {
            _resourceCalendar = resourceCalendar;
            _consultationsRepository = consultationsRepository;
        }

        public ConsultationRequestResult RequestFirstAvailableConsultation(ConsultationRequest consultationRequest)
        {
            try
            {
                DateTime earliestDate = DateTime.Now.Date.AddDays(1);
                var consultation = _resourceCalendar.ScheduleFirstAvailable(consultationRequest.PatientName, consultationRequest.RegistrationDate, consultationRequest.Condition, earliestDate);
                var result = new ConsultationRequestResult
                {
                    Succesful = (consultation != null),
                    BookedConsultation = consultation
                };
                return result;
            }
            catch (ResourceCalendarException calendarException)
            {
                var result = new ConsultationRequestResult()
                {
                    Succesful = false,
                    ErrorMessage = calendarException.Message
                };

                return result;
            }

            return null;
        }

        public ICollection<PatientRecord> GetAllRegisteredPatients()
        {
            return _consultationsRepository.GetAllRegisteredPatients();
        }

        public ICollection<Consultation> GetAllScheduledConsultations(DateTime fromDate)
        {
            return _consultationsRepository.GetAllConsultations().Where(x => x.ConsultationDate >= fromDate).ToList();
        }

        public void RemovedScheduledConsultation(string id)
        {
            _consultationsRepository.RemoveConsultation(id);
        }
    }

}
