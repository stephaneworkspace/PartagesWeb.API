using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour l'affichage du tableau
    /// </summary>
    public class ForumCategorieForListDto
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
        /// Nombre de sujet(s)
        /// </summary>
        public int CountSujet { get; set; }
        /// <summary>
        /// Nombre de poste(s)
        /// </summary>
        public int CountPoste { get; set; }
        /// <summary>
        /// Dernier Poste
        /// </summary>
        public ForumPosteForListForumCategorieDtoController DernierPoste { get; set; }
    }
}
