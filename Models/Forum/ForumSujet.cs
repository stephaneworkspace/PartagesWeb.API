using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    /// <summary>
    /// Topic du forum
    /// </summary>
    public class ForumSujet
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
        /// Relation avec model User
        /// </summary>
        public virtual ForumCategorie ForumCategorie { get; set; }
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
    }
}
