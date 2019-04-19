using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Helpers.Forum;
using PartagesWeb.API.Models.Forum;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository du forum
    /// </summary>
    public class ForumRepository : IForumRepository
    {
        private readonly DataContext _context;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="context">DataContext</param>
        public ForumRepository(DataContext context)
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
         * ForumCategorie
         **/

        /// <summary>
        /// Liste avec pagination des catégories du forum
        /// </summary>
        /// <param name="forumCategorieParams"></param>
        /// <returns></returns>
        public async Task<PagedList<ForumCategorie>> GetForumCategories(ForumCategorieParams forumCategorieParams)
        {
            var items = _context.ForumCategories
                .OrderBy(u => u.Nom).AsQueryable();
            return await PagedList<ForumCategorie>.CreateAsync(items, forumCategorieParams.PageNumber, forumCategorieParams.PageSize);
        }

        /// <summary>
        /// Compter les sujets d'une catégorie du forum
        /// </summary>
        /// <param name="id">Clé principal Id ForumCategorie</param>
        /// <returns></returns>
        public async Task<int> GetCountSujet(int id)
        {
            var count = _context.ForumSujets.Where(x => x.ForumCategorieId == id).Count();
            await Task.FromResult(count);
            return count;
        }

        /// <summary>
        /// Compter les postes d'une catégorie du forum
        /// </summary>
        /// <param name="id">Clé principal Id ForumCategorie</param>
        /// <returns></returns>
        public async Task<int> GetCountPosteForumCategorie(int id)
        {
            var items = _context.ForumSujets.Where(x => x.ForumCategorieId == id);
            var count = await Task.FromResult(0);
            foreach (var unite in items)
            {
                var countTempo = _context.ForumPostes.Where(x => x.ForumSujetId == unite.Id).Count();
                await Task.FromResult(countTempo);
                count += countTempo;
            }
            return count;
        }

        /// <summary>
        /// Obtenir le dernier poste d'une catégorie
        /// </summary>
        /// <param name="id">Id de la catégorie</param>
        /// <returns></returns>
        public async Task<ForumPoste> GetDernierForumPosteDeUneCategorie(int id)
        {
            var item = await _context.ForumPostes.Where(x => x.ForumSujet.ForumCategorieId == id).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            return item;
        }

        /**
         * ForumSujet
         */

        /// <summary>  
        /// Cette méthode permet d'obtenir tous les sujets
        /// </summary>
        /// <param name="forumSujetParams">Pagination</param>
        /// <param name="id">Id du sujet ForumPoste</param>
        /// <returns></returns> 
        public async Task<PagedList<ForumSujet>> GetForumSujets(ForumSujetParams forumSujetParams, int id)
        {
            var items = _context.ForumSujets
                .OrderBy(u => u.Date).Where(x => x.ForumCategorieId == id).AsQueryable();
            return await PagedList<ForumSujet>.CreateAsync(items, forumSujetParams.PageNumber, forumSujetParams.PageSize);
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir un sujet
        /// </summary>
        /// <param name="id">Id du sujet</param>
        /// <returns></returns> 
        public async Task<ForumSujet> GetForumSujet(int id)
        {
            var item = await _context.ForumSujets.Where(x => x.Id == id).FirstOrDefaultAsync();
            return item;
        }

        /// <summary>
        /// Compter le nombre de poste d'un sujet pour le dernier poste
        /// </summary>
        /// <param name="id">Id ForumSujet</param>
        /// <returns></returns>
        public async Task<int> GetCountDernierPoste(int id)
        {
            var items = _context.ForumPostes.Where(x => x.ForumSujetId == id).Count();
            var count = await Task.FromResult(items);
            return count;
        }

        /// <summary>
        /// Compter les postes d'unsujet du forum
        /// </summary>
        /// <param name="id">Clé principal Id ForumSujet</param>
        /// <returns></returns>
        /// <remarks>
        /// Doublon ??? GetCountDernierPoste/GetCountPosteForumSujet</remarks>
        public async Task<int> GetCountPosteForumSujet(int id)
        {
            var items = _context.ForumPostes.Where(x => x.ForumSujetId == id).Count();
            var count = await Task.FromResult(items);
            return count;
        }

        /// <summary>
        /// Obtenir le dernier poste d'un sujet
        /// </summary>
        /// <param name="id">Id du sujet</param>
        /// <returns></returns>
        public async Task<ForumPoste> GetDernierForumPosteDeUnSujet(int id)
        {
            var item = await _context.ForumPostes.Where(x => x.ForumSujet.Id == id).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
            return item;
        }

        /// <summary>
        /// Incrementer compteur view
        /// </summary>
        /// <param name="id">Clé principale ForumSujet Id</param>
        public async Task<bool> IncView(int id)
        {
            var item = await _context.ForumSujets.Where(x => x.Id == id).FirstOrDefaultAsync();
            item.View++;
            Update(item);
            return await SaveAll();
        }

        /**
         * ForumPoste
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir tous les postes
        /// </summary>
        /// <param name="forumPosteParams">Pagination</param>
        /// <param name="id">Id du sujet ForumPoste</param>
        /// <returns></returns> 
        public async Task<PagedList<ForumPoste>> GetForumPostes(ForumPosteParams forumPosteParams, int id)
        {
            var items = _context.ForumPostes
                .OrderBy(u => u.Date).Where(x => x.ForumSujetId == id).AsQueryable();
            return await PagedList<ForumPoste>.CreateAsync(items, forumPosteParams.PageNumber, forumPosteParams.PageSize);
        }

        /// <summary>
        /// Obtenir le nombre de message d'un utilisateur
        /// </summary>
        /// <param name="id">Clé principale User</param>
        /// <returns></returns>
        public async Task<int> GetCountUser(int id)
        {
            var items = _context.ForumPostes.Where(x => x.UserId == id).Count();
            var count = await Task.FromResult(items);
            return count;
        }
    }
}
