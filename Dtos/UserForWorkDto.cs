//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos
{
    /// <summary>
    /// Dto pour retourner l'utilisateur avec le token
    /// </summary>
    public class UserForWorkDto
    {
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public string Username { get; set; }
    }
}
