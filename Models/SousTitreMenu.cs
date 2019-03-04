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
    /// Model SousTitreMenu
    /// </summary>
    public class SousTitreMenu
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé facultative du model TitreMenu parent
        /// </summary>
        public int? TitreMenuId { get; set; }
        /// <summary>
        /// Relation avec model TitreMenu
        /// </summary>
        public virtual TitreMenu TitreMenu { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Article relié à cette section
        /// </summary>
        /// <remarks>
        /// Trouver un moyen de trier cette collection
        /// </remarks>
        public ICollection<Article> Articles { get; set; }
    }
}
