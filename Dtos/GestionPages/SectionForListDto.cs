using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages
{
    /// <summary>
    /// Dto pour l'affichage des sections sans le champ Type et avec la Id en int?
    /// </summary>
    public class SectionForListDto
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
        /// Icone fa de font awesome
        /// </summary>
        public string Icone { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Switch si c'est en ligne true, si c'est hors ligne false
        /// </summary>
        public bool SwHorsLigne { get; set; }
        /// <summary>
        /// TitreMenu relié à cette section
        /// </summary>
        /// <remarks>
        /// Trouver un moyen de trier cette collection
        /// </remarks>
        public List<TitreMenuForListDto> TitreMenus { get; set; }
    }
}
