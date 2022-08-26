using Api_Dog.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Api_Dog.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Dog> dogs { get; set; }
    }
}
