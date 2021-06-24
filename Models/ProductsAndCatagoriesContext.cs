using Microsoft.EntityFrameworkCore;

namespace ProductsAndCatagories.Models
{
    public class ProductsAndCatagoriesContext : DbContext
    {
        public ProductsAndCatagoriesContext(DbContextOptions options) : base(options) { }

        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        public DbSet<Product> Products { get; set; }
        public DbSet<Catagory> Catagories { get; set; }

        public DbSet<Association> Associations { get; set; }
    }
}