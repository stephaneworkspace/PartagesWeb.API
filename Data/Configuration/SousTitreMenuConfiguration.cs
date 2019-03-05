using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data.Configuration
{
    /// <summary>
    /// Configuration de TitreMenu
    /// </summary>
    public class SousTitreMenuConfiguration : IEntityTypeConfiguration<SousTitreMenu>
    {
        /// <summary>
        /// Information pour EntityFramwork
        /// </summary>
        /// <param name="builder">Builder</param>
        public void Configure(EntityTypeBuilder<SousTitreMenu> builder)
        {
            // J'ai pas forcement besoin de tout ça mainteant, je garde le code ça peux me servir pour le delete cascade ou dans le même genre

            // builder.HasKey(s => s.SectionId);
            // builder.HasOne<Section>(s => s.SectionId)
            //    .WithOne().IsRequired(false);
            // builder.Property(s => s.SectionId).HasOne(s => s.SectionId).WithMany("TitreMenus").IsRequired(false);
            // builder.Property(s => s.SectionId).IsRequired(false);
            // builder.HasOne(s => s.Section)        
        }
    }
}
