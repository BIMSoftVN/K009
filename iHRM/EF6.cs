using iHRM.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHRM
{
    public class EF6 : DbContext
    {
        public EF6(string connectionString) : base(connectionString)
        {
        }

        public DbSet<clUserEF6> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
