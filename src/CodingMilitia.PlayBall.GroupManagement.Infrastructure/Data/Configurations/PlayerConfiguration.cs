using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
                .HasKey(e => e.Id);

            builder
                .Property(e => e.Id)
                .UseIdentityAlwaysColumn();

            builder
                .HasOne(e => e.Group);

            builder
                .HasOne(e => e.User);
        }
    }
}