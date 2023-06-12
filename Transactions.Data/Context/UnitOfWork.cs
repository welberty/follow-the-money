using Foundation.Business.Data;
using Microsoft.EntityFrameworkCore;

namespace Transactions.Data.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext dbContext;

        public UnitOfWork(TransactionContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Commit(CancellationToken cancellationToken)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
