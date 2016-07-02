using System;
using RegistrationSystem.Contract;
using Resources.Contract;

namespace Simulator.Console
{
    public class Simulator
    {
        private IConsultationBooker _consultationBooker;
        private IResourcesRepository _resourcesRepository;
        private PrintHelper _resultHelper;

        public void StartMenu()
        {          
            MenuLoop();
        }

        public void Init(DateTime calenderStartDate, int calenderSize)
        {
            var factory = new RegistrationSystem.DI.Factory();
            factory.BindDependencies();

            _resultHelper = new PrintHelper();

            _resourcesRepository = factory.ResourcesRepository;

            ResourcesFileReader.AddDoctorsFromFile("doctors.json",_resourcesRepository);
            ResourcesFileReader.AddRoomsFromFile("rooms.json",_resourcesRepository);

            _consultationBooker = factory.ConsultationBooker;
            _consultationBooker.Init(calenderStartDate, calenderSize);
        }

        private void MenuLoop()
        {
            _resultHelper.PrintMenu();

            var menuStr = System.Console.ReadLine();
            while (menuStr != "6")
            {
                switch (menuStr)
                {
                    case "1":
                        _resultHelper.PrintAllBookedConsultations(_consultationBooker);
                        break;
                    case "2":
                        _resultHelper.PrintAllRegisteredPatients(_consultationBooker);
                        break;
                    case "3":
                        _resultHelper.PrintAvailableResources(_resourcesRepository);
                        break;
                    case "4":
                        SelectAndProcessRequestFile();
                        break;
                    case "5":
                        RemoveScheduledConsultation();
                        break;
                }

                _resultHelper.PrintMenu();
                menuStr = System.Console.ReadLine();
            }
        }

        private void RemoveScheduledConsultation()
        {
            System.Console.Write("Enter id of scheduled consultation to remove: ");
            var id = System.Console.ReadLine();
            _consultationBooker.RemoveScheduledConsultation(id);
        }

        private void SelectAndProcessRequestFile()
        {
            System.Console.Write("Enter csv-file to load: ");
            var file = System.Console.ReadLine();
            var today = DateTime.Now.Date;
            LoadAndProcessRequestFile(file,today);
        }

        public void LoadAndProcessRequestFile(string file, DateTime today)
        {
            RequestsFileReader.ProcessRequestsFile(file, today,_consultationBooker,_resultHelper);            
        }

    }
}
