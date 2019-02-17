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
        /// <remarks>
        /// 8 Février : 
        /// https://stackoverflow.com/questions/5668801/entity-framework-code-first-null-foreign-key
        /// </remarks>
        public int? SectionId { get; set; }
        /// <summary>
        /// Virtual Clé facultative du model Section parent
        /// </summary>
        /// <remarks>
        /// 8 Février : 
        /// à tester
        /// </remarks>
        [ForeignKey("SectionId")]
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
        /// Switch si c'est en ligne true, si c'est hors ligne false
        /// </summary>
        public bool SwHorsLigne { get; set; }
    }
}
