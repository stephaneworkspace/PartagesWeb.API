﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour l'affichage du tableau
    /// </summary>
    public class ForumPosteForListForumCategorieDtoController
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
        /// <remarks>
        /// // A FAIRE public virtual ForumSujet ForumSujet { get; set; }
        /// </remarks>
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Relation avec model User
        /// </summary>
        /// <remarks>
        ///public virtual User User { get; set; }
        /// </remarks>
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