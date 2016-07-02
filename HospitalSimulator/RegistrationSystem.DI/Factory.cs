using Consultations.Contract;
using Ninject;
using RegistrationSystem.Contract;
using RegistrationSystem.Fakes;
using Resources.Contract;
using Scheduler;
using Scheduler.Contract;

namespace RegistrationSystem.DI
{
    public class Factory : IFactory
    {
        readonly IKernel _kernel = new StandardKernel();

        public void BindDependencies()
        {
            _kernel.Bind<IResourceCalendar>().To<ResourceCalendar>().InSingletonScope();
            _kernel.Bind<IConsultationsRepository>().To<ConsultationsRepository>().InSingletonScope();
            _kernel.Bind<IResourcesRepository>().To<ResourcesRepository>().InSingletonScope();
            _kernel.Bind<IConsultationBooker>().To<ConsultationBooker>().InSingletonScope();
        }

        public IResourceCalendar ResourceCalendar
        {
            get
            {
                return _kernel.Get<IResourceCalendar>();            
            }
        }

        public IConsultationsRepository ConsultationsRepository
        {
            get
            {
                return _kernel.Get<IConsultationsRepository>();                
            }
        }

        public IResourcesRepository ResourcesRepository
        {
            get
            {
                return _kernel.Get<IResourcesRepository>();                
            }
        }

        public IConsultationBooker ConsultationBooker
        {
            get
            {
                return _kernel.Get<IConsultationBooker>();                
            }
        }

    }
}
