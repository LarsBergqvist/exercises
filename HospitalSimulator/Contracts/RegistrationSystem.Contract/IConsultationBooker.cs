﻿using System;
using System.Collections.Generic;
using Consultations.Contract;

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
        public bool Successful { get; set; }
        public string ErrorMessage { get; set; }
        public Consultation BookedConsultation { get; set; }
    }

    public interface IConsultationBooker
    {
        /// <summary>
        /// Schedules the first available consultation date with resources based on the specified consultation request
        /// The first possible date is the day after the registration date
        /// </summary>
        /// <param name="consultationRequest">PatientName, condition and registration date</param>
        /// <returns>A ConsultationRequestResult with a consultation record if successful. An error message if not successful.</returns>
        ConsultationRequestResult RequestFirstAvailableConsultation(ConsultationRequest consultationRequest);

        /// <summary>
        /// Gets a collection of all registred patients and their consultation records in the system
        /// </summary>
        /// <returns>A PatientRecord for each registred patient in the system</returns>
        ICollection<PatientRecord> GetAllRegisteredPatients();

        /// <summary>
        /// Gets a collection of all consultation records in the system from a specified date and onwards
        /// </summary>
        /// <param name="fromDate">The earliest consultation date to for consultation records to return</param>
        /// <returns>A collection of consultation records</returns>
        ICollection<Consultation> GetAllScheduledConsultations(DateTime fromDate);

        /// <summary>
        /// Removes a previously registred consultation from the system
        /// </summary>
        /// <param name="id">The id of the consultation record in the system</param>
        /// <returns></returns>
        void RemoveScheduledConsultation(string id);

        /// <summary>
        /// Initializes the system with a date range that can be used for requests
        /// </summary>
        /// <param name="calenderStartDate">The first date for the calendar in the system</param>
        /// <param name="sizeInDays">The number of days from start date that can be handled</param>
        /// <returns></returns>
        void Init(DateTime calenderStartDate, uint sizeInDays);
    }
}