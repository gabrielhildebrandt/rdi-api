using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RDI.Infra
{
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<Context>();
        }

        private readonly Context _context;

        public void Handle()
        {
            _context.Database.Migrate();
        }
    }
}