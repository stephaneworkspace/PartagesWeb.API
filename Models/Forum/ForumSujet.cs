using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    /// <summary>
    /// Model ForumSujet
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
        [Required]
        public int ForumCategorieId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        [ForeignKey("ForumCategorieId")]
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
