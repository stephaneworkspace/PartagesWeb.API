using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    /// <summary>
    /// Model ForumCategorie
    /// </summary>
    public class ForumCategorie
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// ForumSujet relié à cette catégorie
        /// </summary>
        /// <remarks>
        /// Trouver un moyen de trier cette collection
        /// </remarks>
        public virtual ICollection<ForumSujet> ForumSujets { get; set; }
    }
}
