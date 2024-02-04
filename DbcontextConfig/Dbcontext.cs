using Microsoft.EntityFrameworkCore;
using TaskManangerSystem.Models.DataBean;

namespace TaskManangerSystem.DbContextConfg
{

    public class ManagementSystemContext : DbContext
    {
        public DbSet<EmployeeAccount> employees { get; set; }

        public DbSet<EncryptAccount> encrypts{set;get;}
        public DbSet<EmployeeRealInfo> realInfos { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<InventoryInfo> inventoryInfos { get; set; }
        public DbSet<TaskCustomer> customers { get; set; }
        public DbSet<TaskAffair> tasks { get; set; }
        public DbSet<TaskStatusTrack> tracks { get; set; }
        public DbSet<InOutStock> inOutStocks {get;set;}

        public ManagementSystemContext(DbContextOptions<ManagementSystemContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EncryptAccount>().HasNoKey().ToView("alias_md5");
        }

    }
}
