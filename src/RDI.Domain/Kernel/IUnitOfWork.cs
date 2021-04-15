using System;
using System.Threading;
using System.Threading.Tasks;

namespace RDI.Domain.Kernel
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken);
    }
}