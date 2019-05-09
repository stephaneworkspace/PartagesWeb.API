using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Models;
using PartagesWeb.API.Models.Messagerie;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository de la messagerie
    /// </summary>
    public class MessagerieRepository : IMessagerieRepository
    {
        private readonly DataContext _context;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="context">DataContext</param>
        public MessagerieRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>  
        /// Cette méthode permet d'ajouter une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity">Entitée au choix</param>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        /// <summary>
        /// Cette méthode permet de mettre à jour l'entité dans le DataContext
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity">Entitée au choix</param>
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>/// 
        /// <param name="entity">Entitée au choix</param>
        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        /// <summary>  
        /// Cette méthode permet de sauvegarder tout dans le DataContext
        /// </summary> 
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /**
         * Messagerie
         */

        /// <summary>  
        /// Cette méthode permet de lire un message
        /// </summary>
        /// <param name="id">MessagerieId</param>
        /// <returns></returns> 
        public async Task<Messagerie> GetMessagerie(int id)
        {
            var item = await _context.Messageries.Where(x => x.Id == id).FirstOrDefaultAsync();
            return item;
        }

        /// <summary>  
        /// Cette méthode permet de switché la variable de lecture d'un message
        /// </summary>
        /// <param name="id">MessagerieId</param>
        /// <returns></returns> 
        public async Task<bool> SwLu(int id)
        {
            var item = await _context.Messageries.Where(x => x.Id == id).FirstOrDefaultAsync();
            item.SwLu = true;
            Update(item);
            return await SaveAll();
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir tous les messages
        /// </summary>
        /// <param name="messagerieParams">Pagination</param>
        /// <param name="userId">Utilisateur [Authorize]</param>
        /// <returns></returns> 
        public async Task<PagedList<Messagerie>> GetMessageries(MessagerieParams messagerieParams, int userId)
        {
            var items = _context.Messageries
                .OrderBy(u => u.Date).Where(x => x.UserId == userId).AsQueryable();
            return await PagedList<Messagerie>.CreateAsync(items, messagerieParams.PageNumber, messagerieParams.PageSize);
        }

        /// <summary>
        /// Compter les messages non lu
        /// </summary>
        /// <param name="userId">Utilisateur [Authorize]</param>/// 
        /// <returns></returns>
        /// <remarks>
        /// 9 mai: Amélioration dans le futur redux pour lire les messages hors connexion ???
        /// </remarks>
        public async Task<int> GetCountMessagesNonLu(int userId)
        {
            var count = _context.Messageries.Where(x => x.UserId == userId).Where(y => y.SwLu == false).Count();
            await Task.FromResult(count);
            return count;
        }

        ///<summary>
        ///Lit l'UserId SendByUserId?
        ///</summary>
        /// <param name="sendByUserId">SendByUserId int?</param>
        /// <returns></returns>
        public async Task<User> GetSendByUser(int sendByUserId)
        {
            var user = await _context.Users.Where(x => x.Id == sendByUserId).FirstOrDefaultAsync();
            return user;
        }
    }
}
