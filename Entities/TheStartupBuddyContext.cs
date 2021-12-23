using System;
using Microsoft.EntityFrameworkCore;

namespace TheStartupBuddy.Entities
{
    public class TheStartupBuddyContext : DbContext
    {
        public TheStartupBuddyContext(DbContextOptions<TheStartupBuddyContext> options) : base(options)
        {
        }

        public DbSet<AdminUserEntity> AdminUsers { get; set; }
    }
}
