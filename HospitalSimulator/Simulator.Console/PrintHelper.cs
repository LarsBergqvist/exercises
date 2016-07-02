using System;
using Consultations.Contract;
using RegistrationSystem.Contract;
using Resources.Contract;

namespace Simulator.Console
{
    public class PrintHelper
    {
        private const string _dateFormat = "yyyy-MM-dd";

        public void PrintRequest(ConsultationRequest request)
        {
            System.Console.WriteLine("Issuing a consultation request for {0} with condition {1}", request.PatientName, request.Condition);
        }

        public void PrintRequestResult(ConsultationRequestResult result)
        {
            PrintDivider();
            if (result.Successful == false)
            {
                System.Console.WriteLine("Could not book consultation. Error message: {0}", result.ErrorMessage);
            }
            else
            {
                System.Console.WriteLine("Successfully booked a consultation:");
                System.Console.WriteLine("Patient name: {0}", result.BookedConsultation.PatientName);
                PrintConsultation(result.BookedConsultation);
            }
        }

        public void PrintDoctor(Doctor doctor)
        {
            System.Console.Write("Doctor: {0} (", doctor.Name);
            int numRolesPrinted = 0;
            foreach (RoleType role in doctor.Roles)
            {
                if (numRolesPrinted > 0)
                    System.Console.Write(",");
                System.Console.Write(role);
                numRolesPrinted++;
            }
            System.Console.WriteLine(")");            
        }

        public void PrintRoom(TreatmentRoom room)
        {
            if (room.TreatmentMachine != null)
            {
                System.Console.WriteLine("Room: {0} (machine {1}, {2})", room.Name, room.TreatmentMachine.Name, room.TreatmentMachine.Capability);
            }
            else
            {
                System.Console.WriteLine("Room: {0}", room.Name);
            }            
        }
        public void PrintConsultation(Consultation consultation)
        {
            System.Console.WriteLine("Consultation date: {0}", consultation.ConsultationDate.Date.ToString(_dateFormat));
            PrintDoctor(consultation.Doctor);
            PrintRoom(consultation.Room);
            System.Console.WriteLine("Id: {0}",consultation.Id);
            System.Console.WriteLine("Registration date: {0}", consultation.RegistrationDate.Date.ToString(_dateFormat));
            System.Console.WriteLine("Condition: {0}",consultation.ConditionType);
            System.Console.WriteLine();
        }

        public void PrintDivider()
        {
            System.Console.WriteLine(("--------------------------------------------------------------------------------"));
        }

        public void PrintMenu()
        {
            System.Console.WriteLine("");
            System.Console.WriteLine("Menu:");
            System.Console.WriteLine("1) List all scheduled consultations");
            System.Console.WriteLine("2) List all patients");
            System.Console.WriteLine("3) List available resources");
            System.Console.WriteLine("4) Load new requests from a csv file");
            System.Console.WriteLine("5) Remove a scheduled consultation ");
            System.Console.WriteLine("6) Quit");
        }

        public void PrintAllRegisteredPatients(IConsultationBooker consultationBooker)
        {
            var patientRecords = consultationBooker.GetAllRegisteredPatients();
            foreach (var patientRecord in patientRecords)
            {
                System.Console.WriteLine("Patient {0} has {1} scheduled consultation(s):", patientRecord.Name, patientRecord.BookedConsultations.Count);
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
                System.Console.WriteLine("Patient name: {0}", consultation.PatientName);
                PrintConsultation(consultation);
            }
        }


        public void PrintAvailableResources(IResourcesRepository resourcesRepository)
        {
            PrintDivider();

            System.Console.WriteLine("The available resources are:");
            var doctors = resourcesRepository.GetAllDoctors();
            foreach (var doctor in doctors)
            {
                PrintDoctor(doctor);
            }
            var rooms = resourcesRepository.GetAllRooms();
            foreach (var room in rooms)
            {
                PrintRoom(room);
            }
        }
    }
}
