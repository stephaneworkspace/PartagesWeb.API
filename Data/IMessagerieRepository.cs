using PartagesWeb.API.Dtos.Forum.Input;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Models;
using PartagesWeb.API.Models.Messagerie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository pour la messagerie
    /// </summary>
    public interface IMessagerieRepository
    {
        /// <summary>  
        /// Cette méthode permet d'ajouter une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity">Entitée au choix</param>
        void Add<T>(T entity) where T : class;
        /// <summary>
        /// Cette méthode permet de mettre à jour l'entité dans le DataContext
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity">Entitée au choix</param>
        void Update<T>(T entity) where T : class;
        /// <summary>  
        /// Cette méthode permet d'effacer une entité dans le DataContext
        /// </summary>  
        /// <param name="entity">Entitée au choix</param>
        void Delete<T>(T entity) where T : class;
        /// <summary>  
        /// Cette méthode permet de sauvegarder tout dans le DataContext
        /// </summary> 
        Task<bool> SaveAll();

        /**
         * Messagerie
         **/

        /// <summary>  
        /// Cette méthode permet de switché la variable de lecture d'un message
        /// </summary>
        /// <param name="id">MessagerieId</param>
        /// <returns></returns> 
        Task<bool> SwLu(int id);
        /// <summary>  
        /// Cette méthode permet de lire un message
        /// </summary>
        /// <param name="id">MessagerieId</param>
        /// <returns></returns> 
        Task<Messagerie> GetMessagerie(int id);
        /// <summary>  
        /// Cette méthode permet d'obtenir tous les messages
        /// </summary>
        /// <param name="messagerieParams">Pagination</param>
        /// <param name="userId">Utilisateur [Authorize]</param>
        /// <returns></returns> 
        /// <remarks>L'utilisateur est identifié par le token</remarks>
        Task<PagedList<Messagerie>> GetMessageries(MessagerieParams messagerieParams, int userId);
        /// <summary>
        /// Compter les messages non lu
        /// </summary>
        /// <param name="userId">Utilisateur [Authorize]</param>
        /// <returns></returns>
        /// <remarks>
        /// 9 mai: Amélioration dans le futur redux pour lire les messages hors connexion ???
        /// </remarks>
        Task<int> GetCountMessagesNonLu(int userId);
        ///<summary>
        ///Lit l'UserId SendByUserId?
        ///</summary>
        /// <param name="sendByUserId">SendByUserId int?</param>
        /// <returns></returns>
        Task<User> GetSendByUser(int sendByUserId);
    }
}
