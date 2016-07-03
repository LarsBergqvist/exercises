using System;
using System.Collections.Generic;
using System.Linq;
using Consultations.Contract;
using RegistrationSystem.Contract;
using Scheduler.Contract;

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
                var earliestDate = consultationRequest.RegistrationDate.Date.AddDays(1);
                var consultation = _resourceCalendar.ScheduleFirstAvailable(consultationRequest.PatientName, consultationRequest.RegistrationDate, consultationRequest.Condition, earliestDate);
                if (consultation != null)
                {
                    return new ConsultationRequestResult
                    {
                        Successful = true,
                        BookedConsultation = consultation
                    };                    
                }
                else
                {
                    return new ConsultationRequestResult
                    {
                        Successful = false,
                        ErrorMessage = "Could not find resources in the calendar for the consultation request."
                    };                                        
                }
            }
            catch (ResourceCalendarException calendarException)
            {
                var result = new ConsultationRequestResult()
                {
                    Successful = false,
                    ErrorMessage = calendarException.Message
                };

                return result;
            }
        }

        public ICollection<PatientRecord> GetAllRegisteredPatients()
        {
            return _consultationsRepository.GetAllRegisteredPatients();
        }

        public ICollection<Consultation> GetAllScheduledConsultations(DateTime fromDate)
        {
            return _consultationsRepository.GetAllConsultations().Where(x => x.ConsultationDate >= fromDate).ToList();
        }

        public void RemoveScheduledConsultation(string id)
        {
            _consultationsRepository.RemoveConsultation(id);
            _resourceCalendar.RemoveConsultationFromCalendars(id);
        }

        public void Init(DateTime calenderStartDate, uint sizeInDays)
        {
            _resourceCalendar.Generate(calenderStartDate, sizeInDays);
        }
    }

}
