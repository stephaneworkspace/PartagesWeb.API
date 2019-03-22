//-----------------------------------------------------------------------
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
    /// Dto pour la création de poste dans le forum
    /// </summary>
    public class ForumPosteForSeedDto
    {
        /// <summary>
        /// Nom Forum Categorie
        /// </summary>
        public string NomForumCategorie { get; set; }
        /// <summary>
        /// Nom Forum Sujet unique (pour seed)
        /// </summary>
        public string NomForumSujet { get; set; }
        /// <summary>
        /// Nom Username
        /// </summary>
        public string NomUser { get; set; }
        /// <summary>
        /// Date du poste
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Contenu unique (pour seed)
        /// </summary>
        public string Contenu { get; set; }
    }
}
