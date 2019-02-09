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
    /// Dto pour le login depuis le frontend
    /// </summary>
    public class UserForLoginDto
    {
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Mot de passe
        /// </summary>
        public string Password { get; set; }
    }
}
