﻿//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Forum
{
    /// <summary>
    /// Dto pour la création de sujet dans le forum
    /// </summary>
    public class ForumSujetForSeedDto
    {
        /// <summary>
        /// Nom Forum Categori
        /// </summary>
        public string NomForumCategorie { get; set; }
        /// <summary>
        /// Nom du topic unique (pour seed)
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
