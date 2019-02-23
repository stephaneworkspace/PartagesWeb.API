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
    public class TitreMenuConfiguration : IEntityTypeConfiguration<TitreMenu>
    {
        /// <summary>
        /// Information pour EntityFramwork
        /// </summary>
        /// <param name="builder">Builder</param>
        public void Configure(EntityTypeBuilder<TitreMenu> builder)
        {
            builder.HasKey(s => s.SectionId);
            builder.Property(s => s.SectionId).IsRequired(false);
        }
    }
}
