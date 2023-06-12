using AutoMapper;
using Consolidate.Business.Contracts;
using Consolidate.Business.Model;
using Consolidate.Data.Mapping;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Consolidate.Data.Repository
{
    public class ConsolidateTransactionRepository : IConsolidateTransactionRepository
    {
        //private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public ConsolidateTransactionRepository(IDatabase database)
        {
            //_redis = ConnectionMultiplexer.Connect(redisSettings.Value.ConnectionString);
            _database = database;
        }

        public Task Create(ConsolidateTransaction consolidateTransaction, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ConsolidateTransaction?> Get(DateOnly date, CancellationToken cancellationToken)
        {
            var redisValue = await _database.StringGetAsync(date.ToString());
            
            if(redisValue.IsNull) 
            {
                return null;
            }

            var consolidate = JsonConvert.DeserializeObject<ConsolidateTransaction>(redisValue.ToString());
            return consolidate;

        }

        public async Task Save(ConsolidateTransaction consolidateTransaction, CancellationToken cancellationToken)
        {
            await _database.StringSetAsync(consolidateTransaction.Date.ToString(), JsonConvert.SerializeObject(consolidateTransaction));            
        }
    }

    public class ConsolidateTransactionMongoRepository : IConsolidateTransactionRepository
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<ConsolidateTransactionMap> _collection;
        private readonly IMapper _mapper;

        public ConsolidateTransactionMongoRepository(IMongoDatabase mongoDatabase, IMapper mapper)
        {
            _mapper = mapper;
            _mongoDatabase = mongoDatabase;
            _collection = mongoDatabase.GetCollection<ConsolidateTransactionMap>("consolidateTransactions");
        }

        public async Task Create(ConsolidateTransaction consolidateTransaction, CancellationToken cancellationToken)
        {
            var consolidateMap = _mapper.Map<ConsolidateTransactionMap>(consolidateTransaction);
            await _collection.InsertOneAsync(consolidateMap);
            
        }

        public async Task<ConsolidateTransaction?> Get(DateOnly date, CancellationToken cancellationToken)
        {
            var consolidateMap = await _collection.Find(c => c.Date == date).FirstOrDefaultAsync();
            var consolidate = _mapper.Map<ConsolidateTransaction>(consolidateMap);            

            return consolidate;
        }

        public async Task Save(ConsolidateTransaction consolidateTransaction, CancellationToken cancellationToken)
        {
            var consolidateMap = _mapper.Map<ConsolidateTransactionMap>(consolidateTransaction);
            var options = new ReplaceOptions { IsUpsert = true };

            await _collection.ReplaceOneAsync(x => x.Id == consolidateMap.Id, consolidateMap, options, cancellationToken);
        }
    }

    
}
