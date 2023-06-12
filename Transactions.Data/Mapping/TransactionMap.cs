using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson.Serialization.Attributes;
using Transactions.Business.Model;

namespace Transactions.Data.Mapping
{
    public class TransactionMap : IEntityTypeConfiguration<Transaction>
    {
        
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("TransactionsTable");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Date);
            builder.Property(x=> x.Id);
        }
    }


    public class TransactionMongoMap
    {
        [BsonId()]
        public Guid Id;
        [BsonElement("Description")]
        public string Description;
        [BsonElement("Value")]
        public double Value;
        [BsonElement("Type")]
        public TransactionType Type;
        [BsonElement("Date")]
        public DateTime Date;

    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionMongoMap>();
            CreateMap<TransactionMongoMap, Transaction>();
        }
    }
}
