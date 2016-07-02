using System;
using Consultations.Contract;

namespace Scheduler.Contract
{
    public class ResourceCalendarException : Exception
    {
        public ResourceCalendarException(string message)
            : base(message)
        {
        }
    }

    public interface IResourceCalendar
    {
        /// <summary>
        /// Generates or regenerates a new a new calendar with a specified size
        /// </summary>
        /// <param name="fromDate">The first date in the calendar</param>
        /// <param name="sizeInDays">The size of the calendar in days</param>
        /// <returns></returns>
        void Generate(DateTime fromDate, int sizeInDays);

        /// <summary>
        /// Schedules the first available consultation date with resources based on the specified condition
        /// Throws a ResourceCalendarException if the calendar is not initiated or if the earliest date requested
        /// is beyond the limits of the generated calendar
        /// </summary>
        /// <param name="patientName">The name of the patient</param>
        /// <param name="registrationDate">The date when the patient was registred</param>
        /// <param name="condition">The ConditionType of the patient</param>
        /// <param name="earliestDate">The earliest date allowed for consultation</param>
        /// <returns>A consultation record if successful. null if no date with fitting resources could be found within the calendar.</returns>
        Consultation ScheduleFirstAvailable(string patientName, DateTime registrationDate, ConditionType condition, DateTime earliestDate);

        /// <summary>
        /// Gets the total number of schedulations of doctors in the calendar
        /// </summary>
        /// <returns>The number of doctor schedulations</returns>
        int NumDoctorSchedulations();

        /// <summary>
        /// Gets the total number of schedulations of rooms in the calendar
        /// </summary>
        /// <returns>The number of room schedulations</returns>
        int NumRoomSchedulations();

        /// <summary>
        /// Removes a registred consultation from the calendar
        /// </summary>
        /// <returns></returns>
        void RemoveConsultationFromCalendars(string id);

        void Clear();
    }
}