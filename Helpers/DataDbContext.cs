using Microsoft.EntityFrameworkCore;
using core7_oracle_angular14.Entities;
using Oracle.ManagedDataAccess.Client;

namespace core7_oracle_angular14.Helpers
{

   public class DataDbContext : DbContext
    {

        protected readonly IConfiguration Configuration;

        public DataDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // options.UseSqlServer(Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }

}