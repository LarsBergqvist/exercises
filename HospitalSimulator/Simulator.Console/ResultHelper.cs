using System;
using System.Linq;
using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace HospitalSimulator
{

    public class ResultHelper
    {
        private string _dateFormat = "yyyy-MM-dd";

        public void PrintRequest(ConsultationRequest request)
        {
            Console.WriteLine("Issuing a consultation request for {0} with condition {1}", request.PatientName, request.Condition);
        }

        public void PrintRequestResult(ConsultationRequestResult result)
        {
            PrintDivider();
            if (result.Succesful == false)
            {
                Console.WriteLine("Could not book consultation. Error message: {0}", result.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Successfully booked a consultation:");
                Console.WriteLine("Patient name: {0}, (condition={1})", result.BookedConsultation.PatientName, result.BookedConsultation.ConditionType);
                PrintConsultation(result.BookedConsultation);
            }
        }

        public void PrintDoctor(Doctor doctor)
        {
            Console.Write("Doctor: {0} (", doctor.Name);
            int numRolesPrinted = 0;
            foreach (RoleType role in doctor.Roles)
            {
                if (numRolesPrinted > 0)
                    Console.Write(",");
                Console.Write(role);
                numRolesPrinted++;
            }
            Console.WriteLine(")");            
        }

        public void PrintRoom(TreatmentRoom room)
        {
            if (room.TreatmentMachine != null)
            {
                Console.WriteLine("Room: {0} (machine {1}, {2})", room.Name, room.TreatmentMachine.Name, room.TreatmentMachine.Capability);
            }
            else
            {
                Console.WriteLine("Room: {0}", room.Name);
            }            
        }
        public void PrintConsultation(Consultation consultation)
        {
            Console.WriteLine("Consultation date: {0}", consultation.ConsultationDate.Date.ToString(_dateFormat));
            PrintDoctor(consultation.Doctor);
            PrintRoom(consultation.Room);
            Console.WriteLine("Id: {0}",consultation.Id);
            Console.WriteLine("Registration date: {0}", consultation.RegistrationDate.Date.ToString(_dateFormat));
            Console.WriteLine();
        }

        public void PrintDivider()
        {
            Console.WriteLine(("--------------------------------------------------------------------------------"));
        }

        public void PrintMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("Menu:");
            Console.WriteLine("1) List all scheduled consultations");
            Console.WriteLine("2) List all patients");
            Console.WriteLine("3) List available resources");
            Console.WriteLine("4) Load new requests from a csv file");
            Console.WriteLine("5) Remove a booked consultation ");
            Console.WriteLine("6) Quit");
        }

        public void PrintAllRegisteredPatients(IConsultationBooker consultationBooker)
        {
            var patientRecords = consultationBooker.GetAllRegisteredPatients();
            foreach (var patientRecord in patientRecords)
            {
                Console.WriteLine("Patient {0} has {1} booked consultation(s):", patientRecord.Name, patientRecord.BookedConsultations.Count);
                foreach (var consultation in patientRecord.BookedConsultations)
                {
                    PrintConsultation(consultation);
                }
                PrintDivider();
            }
        }

        public void PrintAllBookedConsultations(IConsultationBooker consultationBooker)
        {
            var consultations = consultationBooker.GetAllScheduledConsultations(DateTime.Now.Date);
            foreach (var consultation in consultations)
            {
                PrintDivider();
                Console.WriteLine("Patient name: {0}, (condition={1})", consultation.PatientName, consultation.ConditionType);
                PrintConsultation(consultation);
            }
        }


        public void PrintAvailableResources(IResourcesRepository resourcesRepository)
        {
            PrintDivider();

            Console.WriteLine("The available resources are:");
            var doctors = resourcesRepository.GetAllDoctors().OrderBy(x => x.Name);
            foreach (var doctor in doctors)
            {
                PrintDoctor(doctor);
            }
            var rooms = resourcesRepository.GetAllRooms().OrderBy(x => x.Name);
            foreach (var room in rooms)
            {
                PrintRoom(room);
            }
        }
    }
}
