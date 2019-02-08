using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models
{
    public class TitreMenu
    {
        public int Id { get; set; }
        // https://stackoverflow.com/questions/5668801/entity-framework-code-first-null-foreign-key
        public int? SectionId { get; set; }
        [ForeignKey("SectionId")]
        public virtual Section Section { get; set; }
        
        public string Nom { get; set; }
        public int Position { get; set; }
    }
}
