//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Data.Configuration;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// DataContext, connexion des Models
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>  
        /// Cette méthode permet la connexion DbContext
        /// </summary>  
        /// <param name="options"> DbContextOptions</param>
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}
        /// <summary>  
        /// Cette méthode permet la connexion au model User
        /// </summary> 
        public DbSet<User> Users { get; set; }
        /// <summary>  
        /// Cette méthode permet la connexion au model Icone
        /// </summary> 
        public DbSet<Icone> Icones { get; set; }
        /// <summary>  
        /// Cette méthode permet la connexion au model Section
        /// </summary> 
        public DbSet<Section> Sections { get; set; }
        /// <summary>  
        /// Cette méthode permet la connexion au model TitreMenu
        /// </summary> 
        public DbSet<TitreMenu> TitreMenus { get; set; }
        /// <summary>
        /// Configuration des models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TitreMenuConfiguration());
        }
    }
}