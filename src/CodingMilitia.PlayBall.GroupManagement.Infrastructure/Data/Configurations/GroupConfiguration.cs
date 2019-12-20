using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Configurations
{
    internal class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.Id)
                .UseIdentityAlwaysColumn();

            builder
                .HasOne(e => e.Creator);

            builder
                .HasMany(e => e.GroupUsers)
                .WithOne(e => e.Group)
                .HasForeignKey("GroupId");

            builder
                .Property(e => e.RowVersion)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }
    }
}