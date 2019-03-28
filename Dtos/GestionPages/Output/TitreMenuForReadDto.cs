using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Output
{
    /// <summary>
    /// Dto pour l'affichage du titre de menu à éditer
    /// </summary>
    public class TitreMenuForReadDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SectionId si sélectioné (sinon 0)
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
    }
}
