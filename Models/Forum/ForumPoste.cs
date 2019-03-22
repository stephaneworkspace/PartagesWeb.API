using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    /// <summary>
    /// Model ForumPoste
    /// </summary>
    public class ForumPoste
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
        /// Relation avec model ForumCategorie
        /// </summary>
        public virtual ForumCategorie ForumCategorie { get; set; }
        /// <summary>
        /// Clé principal du topic
        /// </summary>
        public int ForumSujetId { get; set; }
        /// <summary>
        /// Relation avec model ForumSujet
        /// </summary>
        public virtual ForumSujet ForumSujet { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        public virtual User User { get; set; }
        /// <sumary>
        /// Position
        /// </sumary>
        public int Position { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Contenu { get; set; }
    }
}
