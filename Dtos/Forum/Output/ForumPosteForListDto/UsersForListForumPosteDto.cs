using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto
    /// </summary>
    public class UsersForSelectForumPosteDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id;
        /// <summary>
        /// Nom d'utilisateur
        /// </summary>
        public string Username;
        /// <summary>
        /// Date de création de l'utilisateur
        /// </summary>
        public DateTime Created { get; set; }
    }
}
