using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        /// Clé principal du topic
        /// </summary>
        [Required]
        public int ForumSujetId { get; set; }
        /// <summary>
        /// Relation avec model ForumSujet
        /// </summary>
        public virtual ForumSujet ForumSujet { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// Date du poste
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Contenu { get; set; }
    }
}
