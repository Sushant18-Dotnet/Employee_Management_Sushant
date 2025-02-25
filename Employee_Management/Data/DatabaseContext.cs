using EntityLayer;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Employee_Management.Data
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions options) :base(options)
        {
            
        }

        public DbSet<City> City { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public DbSet<State> States { get; set; }

       
    }

}
