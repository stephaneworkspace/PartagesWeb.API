using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Output
{
    /// <summary>
    /// Dto pour l'affichage de l'article à éditer
    /// </summary>
    public class ArticleForReadDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SousTitreMenuId si sélectioné (sinon 0)
        /// </summary>
        public int? SousTitreMenuId { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Contenu de l'article
        /// </summary>
        public string Contenu { get; set; }
    }
}
