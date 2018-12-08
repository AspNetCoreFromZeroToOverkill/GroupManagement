using CodingMilitia.PlayBall.GroupManagement.Data.Configurations;
using CodingMilitia.PlayBall.GroupManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodingMilitia.PlayBall.GroupManagement.Data
{
    public class GroupManagementDbContext : DbContext
    {
        public DbSet<GroupEntity> Groups { get; set; }

        public GroupManagementDbContext(DbContextOptions<GroupManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.ApplyConfiguration(new GroupEntityConfiguration());
        }
    }
}