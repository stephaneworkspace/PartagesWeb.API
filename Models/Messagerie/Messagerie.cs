using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Messagerie
{
    /// <summary>
    /// Model ForumPrivateMessage
    /// </summary>
    public class Messagerie
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur qui reçoit le message
        /// </summary>
        /// <remarks>
        /// Par defaut il y a un delete cascade
        /// </remarks>
        [Required]
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// Clé de l'utilisateur qui envoie le message
        /// </summary>
        public int? SendByUserId {get; set; }
        /// <summary>
        /// Date du poste
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Contenu { get; set; }
        /// <summary>
        /// Switch si message lu
        /// </summary>
        public Boolean SwLu { get; set; }
    }
}
