﻿//-----------------------------------------------------------------------
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
    /// Model TitreMenu
    /// </summary>
    public class TitreMenu
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Clé facultative du model Section parent
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Relation avec model Section
        /// </summary>
        public virtual Section Section { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// SousTitreMenu relié à cette section
        /// </summary>
        /// <remarks>
        /// Trouver un moyen de trier cette collection
        /// </remarks>
        public virtual ICollection<SousTitreMenu> SousTitreMenus { get; set; }
    }
}
