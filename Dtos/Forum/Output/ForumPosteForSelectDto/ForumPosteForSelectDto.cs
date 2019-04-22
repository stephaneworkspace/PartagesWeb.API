using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour citation par exemple dans une reponse pour avoir le poste et le sujet
    /// </summary>
    public class ForumPosteForSelectDto
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
        public virtual ForumSujetForSelectForumPosteDto ForumSujet { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        public virtual UsersForSelectForumPosteDto User { get; set; }
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