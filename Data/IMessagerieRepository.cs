using PartagesWeb.API.Dtos.Forum.Input;
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

        /// <sumary>
        /// Cette méthode permet d'envoyer un message privé
        /// </sumary>
        /// <param name="dto">Dto pour l'envoi du message</param>
        // Task<bool> SendMessage(MessagerieForNewMessageDto dto);

        /// <summary>  
        /// Cette méthode permet d'obtenir une catégorie
        /// </summary>
        /// <param name="id">MessagerieId</param>
        /// <returns></returns> 
        /// <remarks>Il y a une écriture a faire c'est switcher le message</remarks>
        // Task<Messagerie> GetMessage(int id);

        /// <summary>  
        /// Cette méthode permet d'obtenir tous les messages
        /// </summary>
        /// <param name="messagerieParams">Pagination</param>
        /// <returns></returns> 
        /// <remarks>L'utilisateur est identifié par le token</remarks>
        // Task<PagedList<Messagerie>> GetMessagess(MessagerieParams messagerieParams);

        /// <summary>
        /// Compter les messages non lu
        /// </summary>
        /// <returns></returns>
        /// <remarks>L'utilisateur est identifié par le token
        /// Fin avril 2019: Amélioration dans le futur redux pour lire les messages hors connexion ???
        /// </remarks>
        // Task<int> GetCountMessages(int id);
    }
}
