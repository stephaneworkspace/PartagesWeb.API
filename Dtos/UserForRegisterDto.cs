//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos
{
    /// <summary>
    /// Dto pour la création d'un utilisateur depuis le frontend
    /// </summary>
    public class UserForRegisterDto
    {
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// Mot de passe
        /// </summary>
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "You must specify a password between 4 and 8 characters")]
        public string Password { get; set; }
    }
}
