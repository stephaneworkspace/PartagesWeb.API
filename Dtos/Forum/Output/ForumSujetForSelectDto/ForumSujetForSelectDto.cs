using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour sélection (exemple : reponse à un sujet)
    /// </summary>
    public class ForumSujetForSelectDto
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
        public virtual ForumCategorieForSelectForumSujetDto ForumCategorie { get; set; }
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
        /// Nombre de view
        /// </summary>
    }
}