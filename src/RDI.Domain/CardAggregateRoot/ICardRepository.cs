using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using RDI.Domain.Kernel;

namespace RDI.Domain.CardAggregateRoot
{
    public interface ICardRepository : IRepository
    {
        IQueryable<Card> Get(Expression<Func<Card, bool>> predicate);

        Task AddAsync(Card card, CancellationToken cancellationToken);
    }
}