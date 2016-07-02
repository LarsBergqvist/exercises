using System;

namespace RegistrationSystem.Contract
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
        void Generate(DateTime fromDate, DateTime toDate);
        Consultation ScheduleFirstAvailable(string patientName, DateTime registrationDate, ConditionType condition, DateTime earliestDate);
        int NumberOfSchedulationsOfDoctors();
        int NumberOfSchedulationsOfRooms();
        void Clear();
    }
}