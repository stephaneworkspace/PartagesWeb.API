//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models
{
    /// <summary>
    /// Model Section
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nom de la section
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Icone fa de font awesome
        /// </summary>
        public string Icone { get; set; }
        /// <summary>
        /// Type de section
        /// </summary>
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment il n'y a pas de model relié à ça
        /// </remarks>
        public string Type { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
        /// <summary>
        /// Switch si c'est en ligne true, si c'est hors ligne false
        /// </summary>
        public bool SwHorsLigne { get; set; }
        /// <summary>
        /// TitreMenu relié à cette section
        /// </summary>
        /// <remarks>
        /// Trouver un moyen de trier cette collection
        /// </remarks>
        public ICollection<TitreMenu> TitreMenus { get; set; }
    }
}
