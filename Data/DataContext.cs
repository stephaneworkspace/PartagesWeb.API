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
        /// Cette méthode permet la connexion au model SousTitreMenu
        /// </summary>
        public DbSet<SousTitreMenu> SousTitreMenus { get; set; }
        /// <summary>
        /// Cette méthode permet la connexion au model Article
        /// </summary>
        public DbSet<Article> Articles { get; set; }
        /// <summary>
        /// Configuration des models
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TitreMenuConfiguration());
            modelBuilder.ApplyConfiguration(new SousTitreMenuConfiguration());

            /*

            modelBuilder.Entity<Section>()
                .HasMany<TitreMenu>(c => c.TitreMenus).WithOne(x => x.Section)
                 .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<TitreMenu>().HasKey(x => x.SectionId); plante

            modelBuilder.Entity<TitreMenu>()
                .HasMany<SousTitreMenu>(c => c.SousTitreMenus).WithOne(x => x.TitreMenu)
                 .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<SousTitreMenu>().HasKey(x => x.TitreMenuId); plante

            modelBuilder.Entity<SousTitreMenu>()
                .HasMany<Article>(c => c.Articles).WithOne(x => x.SousTitreMenu)
                 .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<Article>().HasKey(x => x.SousTitreMenuId); plante
            */
            /*
            modelBuilder.Entity<SousTitreMenu>(e =>
            {
                e.HasOne(p => p.TitreMenu)
                    .WithOne()
                    .HasForeignKey<SousTitreMenu>(p => p.TitreMenuId)
                    .IsRequired(false);
            });*/

            /*
            modelBuilder.Entity<SousTitreMenu>()
                .HasMany(s => s.Articles)
                .WithOne(c => c.SousTitreMenu)
                .OnDelete(DeleteBehavior.Restrict);
            */


        }
    }
}