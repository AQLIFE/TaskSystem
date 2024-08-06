using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models;

namespace TaskManangerSystem.Tool
{

    public class ManagementSystemContext(DbContextOptions<ManagementSystemContext> options) : DbContext(options)
    {
        public DbSet<EmployeeAccount> employees { get; set; }
        public DbSet<EmployeeInfo> realInfos { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<InventoryInfo> inventoryInfos { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<TaskAffair> tasks { get; set; }
        public DbSet<TaskStatusTrack> tracks { get; set; }
        public DbSet<InOutStock> inOutStocks { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeAccount>().HasIndex(e => e.EmployeeAlias).IsUnique();

            modelBuilder.Entity<InventoryInfo>(e =>
            {
                e.HasIndex(e => e.ProductName).IsUnique();
                e.HasOne(i => i.Categories).WithMany().HasForeignKey(e => e.ProductType).IsRequired(false);
            });

            modelBuilder.Entity<InOutStock>(e =>
            {
                e.HasOne(i => i.Task).WithMany().HasForeignKey(e => e.TaskId);
                e.HasOne(i => i.InventoryInfo).WithMany().HasForeignKey(e => e.ProductId);
            });

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasIndex(e => e.CustomerName).IsUnique();

                e.HasOne(i => i.Categories).WithMany().HasForeignKey(e => e.CustomerType);
            });

            modelBuilder.Entity<Category>(e => { e.HasIndex(e => e.SortSerial).IsUnique(); e.HasIndex(e => e.CategoryName).IsUnique(); });


            modelBuilder.Entity<TaskAffair>(e =>
            {
                e.HasOne(i => i.EmployeeAccounts).WithMany().HasForeignKey(e => e.EmployeeId);
                e.HasOne(i => i.Customers).WithMany().HasForeignKey(e => e.CustomerId);
                e.HasOne(i => i.Categorys).WithMany().HasForeignKey(e => e.TaskType);
                e.HasIndex(i => i.Serial).IsUnique();
            });
            modelBuilder.Entity<TaskStatusTrack>(e =>
            {
                e.HasOne(i => i.EmployeeAccounts).WithMany().HasForeignKey(e => e.EmployeeId);
                e.HasOne(i => i.Tasks).WithMany().HasForeignKey(e => e.TaskId);
                e.HasIndex(e => new { e.TaskId, e.EmployeeId, e.TaskStatus }).IsUnique();
            });

        }

    }
}
