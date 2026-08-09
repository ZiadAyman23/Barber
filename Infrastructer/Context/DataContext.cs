using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructer.Context
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }
        public DbSet<Barber> Barbers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ItemOrder> ItemsOrders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Deductions> Deductions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Salaries> Salaries { get; set; }
    }
}
