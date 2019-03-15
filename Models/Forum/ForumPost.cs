﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    public class ForumPost
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé principal du topic
        /// </summary>
        public int ForumTopicId { get; set; }
        /// <summary>
        /// Relation avec model toic
        /// </summary>
        public virtual ForumCategorie ForumTopic { get; set; }
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
