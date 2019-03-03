//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models
{
    /// <summary>
    /// Model Article
    /// </summary>
    public class Article
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé facultative du model SousTitreMenu parent
        /// </summary>
        public int? SousTitreMenuId { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Contenu de l'article
        /// </summary>
        public string Contenu { get; set; }
    }
}
