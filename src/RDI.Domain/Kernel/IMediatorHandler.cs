using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace RDI.Domain.Kernel
{
    public interface IMediatorHandler<in TMediatorInput, TMediatorResult> : IRequestHandler<TMediatorInput, TMediatorResult>
        where TMediatorInput : IRequest<TMediatorResult>, IMediatorInput<TMediatorResult>
        where TMediatorResult : IMediatorResult
    {
        new Task<TMediatorResult> Handle(TMediatorInput request, CancellationToken cancellationToken);
    }
}