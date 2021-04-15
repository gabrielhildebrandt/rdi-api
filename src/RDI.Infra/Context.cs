using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RDI.Domain.CardAggregateRoot;
using RDI.Domain.Kernel;
using RDI.Infra.Maps;

namespace RDI.Infra
{
    public class Context : DbContext, IUnitOfWork
    {
        public Context(IEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        private readonly IEnvironment _environment;

        public DbSet<Card> Cards { get; set; }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken)
        {
            return await SaveChangesAsync(cancellationToken) > 0;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CardMap());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _environment["ConnectionStrings:DefaultConnection"];
            optionsBuilder.UseSqlServer(connectionString, ob => ob.EnableRetryOnFailure());
        }
    }
}