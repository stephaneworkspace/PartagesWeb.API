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
    /// Model Icone
    /// </summary>
    public class Icone
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Icone fa de font awesome
        /// </summary>
        /// <remarks>
        /// Unique
        /// </remarks>
        public string FaValeur { get; set; }
        /// <summary>
        /// Nom de l'icone pour le SelectBox FrontEnd
        /// </summary>
        /// <remarks>
        /// Unique
        /// </remarks>
        public string NomSelectBox { get; set; }
    }
}
