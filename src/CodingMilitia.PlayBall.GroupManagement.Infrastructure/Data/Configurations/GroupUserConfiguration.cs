using CodingMilitia.PlayBall.GroupManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodingMilitia.PlayBall.GroupManagement.Infrastructure.Data.Configurations
{
    public class GroupUserConfiguration : IEntityTypeConfiguration<GroupUser>
    {
        public void Configure(EntityTypeBuilder<GroupUser> builder)
        {
            builder.HasKey(e =>
                new
                {
                    e.GroupId,
                    e.UserId
                });
        }
    }
}