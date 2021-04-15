using MediatR;

namespace RDI.Domain.Kernel
{
    public class Mediator : MediatR.Mediator, IMediator
    {
        public Mediator(ServiceFactory serviceFactory) : base(serviceFactory)
        {
        }
    }
}