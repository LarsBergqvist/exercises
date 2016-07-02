using System.Collections.Generic;
using System.Linq;
using Resources.Contract;

namespace RegistrationSystem.Fakes
{
    public class ResourcesRepository : IResourcesRepository
    {
        public ResourcesRepository()
        {
            _doctors = new List<Doctor>();
            _rooms = new List<TreatmentRoom>();
        }

        private readonly IList<Doctor> _doctors;
        private readonly IList<TreatmentRoom> _rooms;
        private int _doctorNextId = 1;
        private int _roomNextId = 1;

        public void AddDoctor(Doctor doctor)
        {
            if ( _doctors.Any(d => d.Name == doctor.Name) )
                throw new ResourcesRepositoryException(string.Format("A doctor with name {0} already exists in the repository",doctor.Name));
            doctor.Id = (_doctorNextId++).ToString();
            _doctors.Add(doctor);
        }

        public void AddRoom(TreatmentRoom room)
        {
            if (_rooms.Any(d => d.Name == room.Name))
                throw new ResourcesRepositoryException(string.Format("A room with name {0} already exists in the repository", room.Name));

            room.Id = (_roomNextId++).ToString();
            _rooms.Add(room);
        }

        public IList<Doctor> GetAllDoctors()
        {
            return _doctors;
        }

        public IList<TreatmentRoom> GetAllRooms()
        {
            return _rooms;
        }

        public Doctor GetDoctorByName(string name)
        {
            var doctor = _doctors.FirstOrDefault(d => d.Name == name);
            return doctor;
        }

        public TreatmentRoom GetRoomByName(string name)
        {
            var room = _rooms.FirstOrDefault(r => r.Name == name);
            return room;
        }

        public void Clear()
        {
            _doctors.Clear();
            _rooms.Clear();
        }
    }
}