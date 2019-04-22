using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    /// <summary>
    /// Dto pour citation par exemple dans une reponse pour avoir le poste et le sujet
    /// </summary>
    public class UsersForListForumPosteDto
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
        /// Date de création pour l'utilisateur
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// Nombre de message
        /// </summary>
        public int? MessageCount;
    }
}
