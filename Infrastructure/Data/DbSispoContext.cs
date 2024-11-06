using Core.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DbSispoContext : DbContext
    {
        public DbSispoContext()
        {
        }

        public DbSispoContext(DbContextOptions<DbSispoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EstadoCuentasXPorPagar> EstadoCuentasXPorPagar { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurations();
        }
    }
}
