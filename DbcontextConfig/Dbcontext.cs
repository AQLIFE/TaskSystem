using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models;

namespace TaskManangerSystem.DbContextConfg
{

    public class ManagementSystemContext : DbContext
    {
        public DbSet<EmployeeSystemAccount> EmployeeSystemAccounts { get; set; }

        public DbSet<AliasMd5> AliasMds { get; set; }
        public DbSet<EmployeeRealInfo> EmployeeRealInfos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryInfo> InventoryInfos { get; set; }
        public DbSet<TaskCustomer> TaskCustomers { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskStatusTrack> TaskStatusTracks { get; set; }
        public DbSet<InOutStock> InOutStocks { get; set; }









        public ManagementSystemContext(DbContextOptions<ManagementSystemContext> options) : base(options) { }








        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AliasMd5>().ToView("alias_md5");
            modelBuilder.Entity<AliasMd5>().HasNoKey();
        }

    }
}
