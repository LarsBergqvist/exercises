using System;
using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

namespace HospitalSimulator
{
    public class Simulator
    {
        private IConsultationBooker _consultationBooker;
        private IResourcesRepository _resourcesRepository;
        private ResultHelper _resultHelper;

        public void StartMenu()
        {          
            MenuLoop();
        }

        public void Init(DateTime calenderStartDate, int calenderSize)
        {
            var factory = new RegistrationSystem.DI.Factory();

            _resultHelper = new ResultHelper();

            factory.BindDependencies();

            _resourcesRepository = factory.ResourcesRepository;

            ResourceCreator.AddSimulatedResources(_resourcesRepository);

            var resourceCalendar = factory.ResourceCalendar;
            resourceCalendar.Generate(calenderStartDate, calenderStartDate.AddDays(calenderSize));

            _consultationBooker = factory.ConsultationBooker;
        }

        private void MenuLoop()
        {
            _resultHelper.PrintMenu();

            var menuStr = Console.ReadLine();
            while (menuStr != "6")
            {
                if (menuStr == "1")
                {
                    _resultHelper.PrintAllBookedConsultations(_consultationBooker);
                }
                else if (menuStr == "2")
                {
                    _resultHelper.PrintAllRegisteredPatients(_consultationBooker);
                }
                else if (menuStr == "3")
                {
                    _resultHelper.PrintAvailableResources(_resourcesRepository);
                }
                else if (menuStr == "4")
                {
                    SelectFileAndProcessRequestFile();                    
                }
                else if (menuStr == "5")
                {
                    RemoveBookedConsultation();
                }

                _resultHelper.PrintMenu();
                menuStr = Console.ReadLine();
            }
        }

        private void RemoveBookedConsultation()
        {
            Console.Write("Enter id of booked consultation to remove: ");
            var id = Console.ReadLine();
            _consultationBooker.RemovedScheduledConsultation(id);
        }

        private void SelectFileAndProcessRequestFile()
        {
            Console.Write("Enter csv-file to load: ");
            var file = Console.ReadLine();
            var today = DateTime.Now.Date;
            LoadAndProcessRequestFile(file,today);
        }

        public void LoadAndProcessRequestFile(string file, DateTime today)
        {
            RequestsFileReader.ProcessRequestsFile(file, today,_consultationBooker,_resultHelper);            
        }

    }
}
