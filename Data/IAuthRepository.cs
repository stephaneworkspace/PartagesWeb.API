//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository pour l'authentification
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>  
        /// Cette méthode permet le login
        /// </summary>  
        /// <param name="nom"> Nom d'utilisateur</param>
        /// <param name="motDePasse"> Mot de passe</param>
        Task<User> Login(string nom, string motDePasse);
        /// <summary>  
        /// Cette méthode permet l'inscription
        /// </summary>  
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment [Authorize] est un utilisateur administrateur
        /// </remarks>
        /// <param name="user"> Model user</param>
        /// <param name="password"> Mot de passe</param>        
        Task<User> Register(User user, string password);
        /// <summary>  
        /// Cette méthode permet de savoir si l'utilisateur existe
        /// </summary>  
        /// <param name="nom"> Mot de passe</param>
        Task<bool> UserExists(string nom);
    }
}
