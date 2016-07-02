using System;
using System.Collections.Generic;
using System.Linq;
using Consultations.Contract;
using Resources.Contract;
using Scheduler.Contract;

namespace Scheduler
{
    public class CalenderDay
    {
        public Consultation BookedConsultation { get; set; }
    }

    public class ResourceCalendar : IResourceCalendar
    {
        private readonly IResourcesRepository _resourcesRepository;
        private readonly IConsultationsRepository _consultationsRepository;
        private Dictionary<string, CalenderDay[]> _doctorsCalendars;
        private Dictionary<string, CalenderDay[]> _roomsCalendars;
        private Dictionary<DateTime, int> _dateToIndexMap;
        private bool _isInitated = false;

        public DateTime CalendarStartDate { get; private set; }
        public int CalendarSize { get; private set; }

        public ResourceCalendar(IResourcesRepository resourcesRepository, IConsultationsRepository consultationsRepository)
        {
            _resourcesRepository = resourcesRepository;
            _consultationsRepository = consultationsRepository;
        }

        public void Generate(DateTime fromDate, int sizeInDays)
        {
            CalendarStartDate = fromDate.Date;
            CalendarSize = sizeInDays;

            _dateToIndexMap = CreateDateToIndexMap(CalendarStartDate, CalendarSize);

            _doctorsCalendars = CreateEmptyDoctorsCalendars(CalendarSize, _resourcesRepository);

            _roomsCalendars = CreateEmptyRoomsCalendars(CalendarSize, _resourcesRepository);

            FillCalendarsWithExisitingConsultationBookings(_dateToIndexMap,_doctorsCalendars,_roomsCalendars,_consultationsRepository);

            _isInitated = true;
        }

        public Consultation ScheduleFirstAvailable(string patientName, DateTime registrationDate, ConditionType condition, DateTime earliestDate)
        {
            if (!_isInitated)
                throw new ResourceCalendarException("The ResourceCalendar is not initated.");

            if (!_dateToIndexMap.ContainsKey(earliestDate.Date))
            {
                throw new ResourceCalendarException(string.Format("The requested consultation date {0} is beyond the limits of this calendar.",earliestDate.Date));
            }

            int firstDayIndex = _dateToIndexMap[earliestDate.Date];
            int numDaysToCheck = CalendarSize - firstDayIndex;

            for (int day = firstDayIndex; day <= numDaysToCheck; day++)
            {
                foreach (var doctorNameCalendar in _doctorsCalendars)
                {
                    //
                    // First check if there is suitable doctor available for this day
                    //
                    var doctorName = doctorNameCalendar.Key;
                    var docCal = doctorNameCalendar.Value;

                    if (!IsDoctorSuitableAndFree(doctorName, day, docCal, condition, _resourcesRepository))
                        continue;

                    //
                    // Then check if there is a suitable room available for this day
                    //
                    foreach (var roomNameCalendar in _roomsCalendars)
                    {
                        var roomName = roomNameCalendar.Key;
                        var roomCal = roomNameCalendar.Value;
                        if (IsRoomSuitableAndFree(roomName, day, roomCal, condition, _resourcesRepository))
                        {
                            //
                            // Suitable room and doctor available
                            // Make a new booking
                            //
                            var newConsultation = new Consultation
                            {
                                ConsultationDate = CalendarStartDate.Date.AddDays(day),
                                Doctor = _resourcesRepository.GetDoctorByName(doctorName),
                                PatientName = patientName,
                                Room = _resourcesRepository.GetRoomByName(roomNameCalendar.Key),
                                RegistrationDate = registrationDate,
                                ConditionType = condition
                            };

                            docCal[day] = new CalenderDay() { BookedConsultation = newConsultation };
                            roomCal[day] = new CalenderDay() { BookedConsultation = newConsultation };
                            _consultationsRepository.AddConsultation(newConsultation);

                            return newConsultation;
                        }
                    }
                }

            }

            return null;
        }

        public int NumDoctorSchedulations()
        {
            if (!_isInitated)
                throw new ResourceCalendarException("The ResourceCalendar is not initated.");

            return _doctorsCalendars.SelectMany(doctorNameCalendar => doctorNameCalendar.Value).Count(calDay => calDay != null);
        }

        public int NumRoomSchedulations()
        {
            if (!_isInitated)
                throw new ResourceCalendarException("The ResourceCalendar is not initated.");

            return _roomsCalendars.SelectMany(roomNameCalendar => roomNameCalendar.Value).Count(calDay => calDay != null);
        }

        public void RemoveConsultationFromCalendars(string id)
        {
            foreach (var doctorNameCalendar in _doctorsCalendars)
            {
                var doctorCalendar = doctorNameCalendar.Value;
                for (var i = 0; i < doctorCalendar.Length; i++)
                {
                    if (doctorCalendar[i] == null) continue;
                    if (doctorCalendar[i].BookedConsultation.Id == id)
                    {
                        doctorCalendar[i] = null;
                    }
                }
            }

            foreach (var roomNameCalendar in _roomsCalendars)
            {
                var roomCalendar = roomNameCalendar.Value;
                for (var i = 0; i < roomCalendar.Length; i++)
                {
                    if (roomCalendar[i] == null) continue;
                    if (roomCalendar[i].BookedConsultation.Id == id)
                    {
                        roomCalendar[i] = null;
                    }
                }
            }
        }

        public void Clear()
        {
            if (_doctorsCalendars != null)
                _doctorsCalendars.Clear();

            if (_roomsCalendars != null)
                _roomsCalendars.Clear();

            if (_dateToIndexMap != null)
                _dateToIndexMap.Clear();
        }

        private bool IsDoctorSuitableAndFree(string doctorName, int day, CalenderDay[] docCal, ConditionType condition, IResourcesRepository resourcesRepository)
        {
            var doctor = resourcesRepository.GetDoctorByName(doctorName);
            if (doctor == null) return false;

            bool okDoctorForCondition = false;
            foreach (var role in doctor.Roles)
            {
                if (DoctorRoleConditionMap.CanRoleHandeCondition(role, condition))
                    okDoctorForCondition = true;
            }

            if (!okDoctorForCondition) return false;

            if (docCal[day] != null)
            {
                // already booked for this day
                return false;
            }

            return true;
        }

        private bool IsRoomSuitableAndFree(string roomName, int dayNo, CalenderDay[] roomCal, ConditionType condition, IResourcesRepository resourcesRepository)
        {
            var room = resourcesRepository.GetRoomByName(roomName);
            if (room == null) return false;

            if (!RoomConditionMap.IsSuitableForConditionTreatment(room.TreatmentMachine,condition)) return false;

            if (roomCal == null) return false;

            if (roomCal[dayNo] != null)
            {
                // already booked for this day
                return false;
            }

            return true;
        }

        private Dictionary<DateTime, int> CreateDateToIndexMap(DateTime fromDate, int numberOfDays)
        {
            var dateToIndexMap = new Dictionary<DateTime, int>();

            for (int day = 0; day < numberOfDays; day++)
            {
                dateToIndexMap.Add(fromDate.Date.AddDays(day), day);
            }

            return dateToIndexMap;
        }

        Dictionary<string, CalenderDay[]> CreateEmptyDoctorsCalendars(int numberOfDays, IResourcesRepository resourcesRepository)
        {
            var doctorsCalendars = new Dictionary<string, CalenderDay[]>();
            foreach (var doctor in resourcesRepository.GetAllDoctors())
            {
                var doctorCalendar = new CalenderDay[numberOfDays];
                doctorsCalendars.Add(doctor.Name, doctorCalendar);
            }

            return doctorsCalendars;
        }

        Dictionary<string, CalenderDay[]> CreateEmptyRoomsCalendars(int numberOfDays, IResourcesRepository resourcesRepository)
        {
            var roomsCalendars = new Dictionary<string, CalenderDay[]>();
            var rooms = resourcesRepository.GetAllRooms().OrderBy(x => x.HasTreatmentMachine());
            foreach (var room in rooms)
            {
                var roomCalendar = new CalenderDay[numberOfDays];
                roomsCalendars.Add(room.Name, roomCalendar);
            }

            return roomsCalendars;
        }

        private void FillCalendarsWithExisitingConsultationBookings(Dictionary<DateTime, int> dateToIndexMap, Dictionary<string, CalenderDay[]> doctorsCalendars, Dictionary<string, CalenderDay[]> roomsCalendars, IConsultationsRepository consultationsRepository)
        {
            foreach (var consultation in consultationsRepository.GetAllConsultations())
            {
                string doctorName = consultation.Doctor.Name;
                string roomName = consultation.Room.Name;
                DateTime date = consultation.ConsultationDate.Date;
                if (dateToIndexMap.ContainsKey(date))
                {
                    int dayIndex = _dateToIndexMap[date];
                    if (doctorsCalendars.ContainsKey(doctorName))
                    {
                        doctorsCalendars[doctorName][dayIndex] = new CalenderDay() { BookedConsultation = consultation };
                    }
                    if (roomsCalendars.ContainsKey(roomName))
                    {
                        roomsCalendars[roomName][dayIndex] = new CalenderDay() { BookedConsultation = consultation };
                    }
                }
            }
        }

    }
}
