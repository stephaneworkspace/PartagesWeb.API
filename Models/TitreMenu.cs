using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models
{
    public class TitreMenu
    {
        public int Id { get; set; }
        // S'inspirer de vega et voir si c'est possible ? la possibilité que ça soit vide
        public Section Section { get; set; }
        public int SectionId { get; set; }
        public string Nom { get; set; }
        public int Position { get; set; }
    }
}
