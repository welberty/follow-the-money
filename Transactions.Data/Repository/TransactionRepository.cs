using Transactions.Business.Contracts;
using Transactions.Business.Model;
using Transactions.Data.Context;

namespace Transactions.Data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionContext transactionContext;

        public TransactionRepository(TransactionContext transactionContext)
        {
            this.transactionContext = transactionContext;
        }
        public async Task Save(Transaction transaction, CancellationToken cancellationToken)
        {
            await transactionContext.AddAsync(transaction, cancellationToken);
        }
    }
}
