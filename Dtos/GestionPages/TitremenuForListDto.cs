using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages
{
    /// <summary>
    /// Dto pour l'affichage des titre menu dans sections sans le champ SectionId int?
    /// </summary>
    public class TitreMenuForListDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
    }
}
