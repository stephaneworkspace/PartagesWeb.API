//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>St�phane</author>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Data.Configuration;
using PartagesWeb.API.Models;
using PartagesWeb.API.Models.Forum;
using PartagesWeb.API.Models.Messagerie;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// DataContext, connexion des Models
    /// </summary>
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>  
        /// Cette m�thode permet la connexion DbContext
        /// </summary>  
        /// <param name="options"> DbContextOptions</param>
        public DataContext(DbContextOptions<DataContext> options) : base (options) {}
        /// <summary>  
        /// Cette m�thode permet la connexion au model User
        /// </summary> 
        public DbSet<User> Users { get; set; }
        /// <summary>  
        /// Cette m�thode permet la connexion au model Icone
        /// </summary> 
        public DbSet<Icone> Icones { get; set; }
        /// <summary>  
        /// Cette m�thode permet la connexion au model Section
        /// </summary> 
        public DbSet<Section> Sections { get; set; }
        /// <summary>  
        /// Cette m�thode permet la connexion au model TitreMenu
        /// </summary> 
        public DbSet<TitreMenu> TitreMenus { get; set; }
        /// <summary>
        /// Cette m�thode permet la connexion au model SousTitreMenu
        /// </summary>
        public DbSet<SousTitreMenu> SousTitreMenus { get; set; }
        /// <sumary> Cette m�thode permet la connexion au model ForumCategorie
        /// </sumary>
        public DbSet<ForumCategorie> ForumCategories { get; set; }
        /// <sumary> Cette m�thode permet la connexion au model ForumPoste
        /// </sumary>
        public DbSet<ForumPoste> ForumPostes { get; set; }
        /// <sumary> Cette m�thode permet la connexion au model ForumSujet
        /// </sumary>
        public DbSet<ForumSujet> ForumSujets { get; set; }
        /// <sumary> Cette m�thode permet la connexion au model ForumPrivateMessage
        /// </sumary>
        public DbSet<Messagerie> Messageries { get; set; }
        /// <summary>
        /// Cette m�thode permet la connexion au model Article
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

            // A l'arrache (a faire plus en d�tail au futur)
            /*
                     modelBuilder.Entity<ForumSujet>()
                         .HasOne(s => s.ForumCategorie)
                         .WithOne()
                         .HasForeignKey<ForumCategorie>(x => x.Id)
                         .OnDelete(DeleteBehavior.Restrict);


                     modelBuilder.Entity<ForumPoste>()
                         .HasOne(s => s.ForumCategorie)
                         .WithOne()
                         .HasForeignKey<ForumCategorie>(x => x.Id)
                         .OnDelete(DeleteBehavior.Restrict);

                     modelBuilder.Entity<ForumPoste>()
                         .HasOne(s => s.ForumSujet)
                         .WithOne()
                         .HasForeignKey<ForumSujet>(x => x.Id)
                         .OnDelete(DeleteBehavior.Restrict);

                     // mettre un sw effac� � l'utilisateur et garder l'e-mail au cas ou il reviendrait
                     modelBuilder.Entity<ForumPoste>()
                         .HasOne(s => s.User)
                         .WithOne()
                         .HasForeignKey<User>(x => x.Id)
                         .OnDelete(DeleteBehavior.Restrict);
                              */
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