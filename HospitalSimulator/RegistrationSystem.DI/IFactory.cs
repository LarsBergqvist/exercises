using RegistrationSystem.Contract;
using RegistrationSystem.Contract.Resources;

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