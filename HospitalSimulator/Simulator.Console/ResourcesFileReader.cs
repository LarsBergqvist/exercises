using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace Simulator.Console
{
    public class ResourcesFileReader
    {
        public static void AddDoctorsFromFile(string jsonFile, IResourcesRepository resourcesRepository)
        {
            try
            {
                List<Doctor> doctors;
                using (StreamReader file = File.OpenText(jsonFile))
                {
                    var serializer = new JsonSerializer();
                    doctors = (List<Doctor>)serializer.Deserialize(file, typeof(List<Doctor>));
                }

                foreach (var doctor in doctors)
                {
                    resourcesRepository.AddDoctor(doctor);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(string.Format("Could not read doctor resources from file '{0}'. {1}",jsonFile,e.Message));
            }
        }

        public static void AddRoomsFromFile(string jsonFile, IResourcesRepository resourcesRepository)
        {
            try
            {
                List<TreatmentRoom> rooms;
                using (StreamReader file = File.OpenText(jsonFile))
                {
                    var serializer = new JsonSerializer();
                    rooms = (List<TreatmentRoom>)serializer.Deserialize(file, typeof(List<TreatmentRoom>));
                }

                foreach (var room in rooms)
                {
                    resourcesRepository.AddRoom(room);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(string.Format("Could not read room resources from file '{0}'. {1}", jsonFile, e.Message));
            }
        }

    }
}
