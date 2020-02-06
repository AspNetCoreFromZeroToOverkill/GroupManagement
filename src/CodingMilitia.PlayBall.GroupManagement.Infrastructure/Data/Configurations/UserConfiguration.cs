using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder
                .HasMany(e => e.UserGroups)
                .WithOne()
                .HasForeignKey(e => e.UserId);
        }
    }
}