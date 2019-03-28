using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Output
{
    /// <summary>
    /// Dto pour l'affichage du tableau
    /// </summary>
    public class TitreMenuForSelectInsideDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Nom de la section
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Relation au model Section clé principale
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Model Section Dto
        /// </summary>
        public virtual SectionForSelectInsideDto Section { get; set; }
    }
}
