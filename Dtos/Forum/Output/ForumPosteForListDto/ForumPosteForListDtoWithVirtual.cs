using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour les postes du forum
    /// </summary>
    /// <remarks>
    /// Avec le champ SwCurrentUser en plus
    /// </remarks>
    public class ForumPosteForListDtoWithVirtual
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé principal du topic
        /// </summary>
        public int ForumSujetId { get; set; }
        /// <summary>
        /// Relation avec model ForumSujet
        /// </summary>
        public virtual ForumSujetForListForumPosteDto ForumSujet { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        public virtual UsersForListForumPosteDto User { get; set; }
        /// <summary>
        /// Date du poste
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Contenu { get; set; }
        /// <summary>
        /// Utilisateur en cours identifié
        /// </summary>
        /// <remarks>C'est un champ DTO virtuel</remarks>
        public bool? SwCurrentUser { get; set; }
    }
}