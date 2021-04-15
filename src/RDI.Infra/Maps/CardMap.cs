using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RDI.Domain.CardAggregateRoot;

namespace RDI.Infra.Maps
{
    public sealed class CardMap : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder
                .ToTable(nameof(Card), "dbo")
                .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier")
                .ValueGeneratedNever();

            builder
                .Property(x => x.CreationDate)
                .HasColumnName("CreationDate")
                .IsRequired();

            builder.Property(x => x.CustomerId)
                .HasColumnName("CustomerId")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(x => x.Number)
                .HasColumnName("Number")
                .HasColumnType("bigint")
                .IsRequired();
            
            builder.Property(x => x.Token)
                .HasColumnName("Token")
                .HasColumnType("uniqueidentifier")
                .IsRequired();
        }
    }
}