using Microsoft.EntityFrameworkCore;
using Transactions.Business.Model;
using Transactions.Data.Mapping;

namespace Transactions.Data.Context
{
    public class TransactionContext:DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options): base(options)
        {
            
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
