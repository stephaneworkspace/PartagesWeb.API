using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}

        public DbSet<Categorie> Categories { get; set; }
    }
}