using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Output
{
    /// <summary>
    /// Dto pour l'affichage du tableau
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
        /// <summary>
        /// SousTitreMenu relié à cette section
        /// </summary>
        public List<SousTitreMenuForListDto> SousTitreMenus { get; set; }
    }
}
