using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mercadia.Api.Models
{
    public class MercadiaDbContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MercadiaDbContext() : base("name=MercadiaDbContext")
        {
        }

        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.User> Users { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.Store> Stores { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.StoreSetting> StoreSettings { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.Product> Products { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<Mercadia.Infrastructure.Models.OrderItem> OrderItems { get; set; }
    }
}
