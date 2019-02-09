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
    /// Model User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Hash du mot de passe
        /// </summary>
        public byte[] PasswordHash { get; set; }
        /// <summary>
        /// Salt du mot de passe
        /// </summary>
        public byte[] PasswordSalt { get; set; }
        /// <summary>
        /// Date de création
        /// </summary>
        public DateTime Created { get; set; }
    }
}
