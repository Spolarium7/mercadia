using Mercadia.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadia.Infrastructure.Db
{
    public class MercadiaDbContext : DbContext
    {
        public MercadiaDbContext()
            : base("DefaultConnection")
        { }

        public DbSet<User> Users { get; set; }
    }
}
