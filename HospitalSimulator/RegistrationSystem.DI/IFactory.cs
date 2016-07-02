using Consultations.Contract;
using RegistrationSystem.Contract;
using Resources.Contract;
using Scheduler.Contract;

namespace RegistrationSystem.DI
{
    public interface IFactory
    {
        void BindDependencies();
        IResourceCalendar ResourceCalendar { get; }
        IConsultationsRepository ConsultationsRepository { get; }
        IResourcesRepository ResourcesRepository { get; }
        IConsultationBooker ConsultationBooker { get; }
    }
}