using MediatR;

namespace RDI.Domain.Kernel
{
    public interface IMediatorInput<out TMediatorResult> : IRequest<TMediatorResult> where TMediatorResult : IMediatorResult
    {
    }
}