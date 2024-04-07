using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.Services
{

    public class ManagementSystemContext : DbContext
    {
        public DbSet<EmployeeAccount> employees { get; set; }
        public DbSet<EmployeeInfo> realInfos { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<InventoryInfo> inventoryInfos { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<TaskAffair> tasks { get; set; }
        public DbSet<TaskStatusTrack> tracks { get; set; }
        public DbSet<InOutStock> inOutStocks { get; set; }

        public ManagementSystemContext(DbContextOptions<ManagementSystemContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeAccount>().HasIndex(e => e.EmployeeAlias).IsUnique();

            modelBuilder.Entity<InventoryInfo>(e =>
            {
                e.HasIndex(e => e.ProductName).IsUnique();
                e.HasOne(i => i.Category).WithMany().HasForeignKey(e => e.ProductType).IsRequired(false);
            });

            modelBuilder.Entity<InOutStock>(e =>
            {
                e.HasOne(i => i.Task).WithMany().HasForeignKey(e => e.TaskId);
                e.HasOne(i => i.InventoryInfo).WithMany().HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasIndex(e => e.CustomerName).IsUnique();

                e.HasOne(i => i.Category).WithMany().HasForeignKey(e => e.CustomerType);
            });

            modelBuilder.Entity<Category>(e => { e.HasIndex(e => e.SortSerial).IsUnique(); e.HasIndex(e => e.CategoryName).IsUnique(); });


            modelBuilder.Entity<TaskAffair>(e =>
            {
                e.HasOne(i => i.EmployeeAccount).WithMany().HasForeignKey(e => e.EmployeeId);
                e.HasOne(i => i.Customer).WithMany().HasForeignKey(e => e.CustomerId);
            });
            modelBuilder.Entity<TaskStatusTrack>(e =>
            {
                e.HasOne(i => i.EmployeeAccount).WithMany().HasForeignKey(e => e.EmployeeId);
                e.HasOne(i => i.Task).WithMany().HasForeignKey(e => e.TaskId);
                e.HasIndex(e => new { e.TaskId, e.EmployeeId, e.TaskStatus }).IsUnique();
            });

        }

    }
}
