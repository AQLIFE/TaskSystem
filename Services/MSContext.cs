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

            modelBuilder.Entity<InventoryInfo>().HasIndex(e => e.ProductName).IsUnique();
            
            modelBuilder.Entity<Customer>().HasIndex(e => e.CustomerName).IsUnique();

            modelBuilder.Entity<Category>(e=>{e.HasIndex(e => e.SortSerial).IsUnique();e.HasIndex(e => e.CategoryName).IsUnique();});


            // modelBuilder.Entity<TaskAffair>().Property(e=>e.)
            modelBuilder.Entity<TaskStatusTrack>().HasIndex(e => new {e.TaskId,e.EmployeeId,e.TaskStatus}).IsUnique();
            
        }

    }
}
