using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour l'affichage à l'intégrieur de ForumCategorie
    /// </summary>
    public class ForumSujetForListDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé principal de la catégorie
        /// </summary>
        public int ForumCategorieId { get; set; }
        /// <summary>
        /// Relation
        /// </summary>
        public virtual ForumCategorieForListForumSujetDto ForumCategorie { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Date du topic
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Nombre de view
        /// </summary>
        public int View { get; set; }
        /// <summary>
        /// Nombre de sujet(s)
        /// </summary>
        // public int CountSujet { get; set; }
        /// <summary>
        /// Nombre de poste(s)
        /// </summary>
        // public int CountPoste { get; set; }
        /// <summary>
        /// Dernier Poste
        /// </summary>
        // public ForumPosteForListForumCategorieDto DernierPoste { get; set; }
        /// <summary>
        /// Numéro total de poste dans le dernier poste
        /// </summary>
        // public int CountDernierPoste { get; set; }
        /// <summary>
        /// Page du dernier poste
        /// </summary>
        // public int PageDernierPoste { get; set; }
    }
}