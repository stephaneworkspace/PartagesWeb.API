using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
