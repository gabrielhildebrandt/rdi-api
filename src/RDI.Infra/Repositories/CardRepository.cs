using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;

namespace RDI.Infra.Repositories
{
    public class CardRepository : ICardRepository
    {
        public CardRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private readonly Context _context;

        public IUnitOfWork UnitOfWork => _context;

        public IQueryable<Card> Get(Expression<Func<Card, bool>> predicate)
        {
            var query = _context
                .Set<Card>()
                .AsQueryable();

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public async Task AddAsync(Card card, CancellationToken cancellationToken)
        {
            await _context.AddAsync(card, cancellationToken);
        }
    }
}