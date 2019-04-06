using PartagesWeb.API.Helpers;
using PartagesWeb.API.Helpers.Forum;
using PartagesWeb.API.Models.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository pour le forum
    /// </summary>
    public interface IForumRepository
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
         * ForumCategorie
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les catégories
        /// </summary>
        /// <param name="forumCategorieParams">Param pagination</param>
        /// <param name="id">Id du sujet ForumPoste</param>
        /// <returns></returns> 
        /// <remark>Obsolète ici à enlever au futur</remark>
        Task<PagedList<ForumCategorie>> GetForumCategories(ForumCategorieParams forumCategorieParams);
        /// <summary>
        /// Compter les sujets d'une catégorie du forum
        /// </summary>
        /// <param name="id">Clé principale Id ForumCategorie</param>
        /// <returns></returns>
        Task<int> GetCountSujet(int id);

        /// <summary>
        /// Compter les postes d'une catégorie du forum
        /// </summary>
        /// <param name="id">Clé principale Id ForumCategorie</param>
        /// <returns></returns>
        Task<int> GetCountPosteForumCategorie(int id);

        /// <summary>
        /// Obtenir le dernier poste d'une catégorie
        /// </summary>
        /// <param name="id">Id de la catégorie</param>
        /// <returns></returns>
        Task<ForumPoste> GetDernierForumPosteDeUneCategorie(int id);

        /// <summary>
        /// Compter le nombre de poste d'un sujet pour le dernier poste
        /// </summary>
        /// <param name="id">Id ForumSujet</param>
        /// <returns></returns>
        Task<int> GetCountDernierPoste(int id);

        Task<ForumPoste> GetForumPosteTest(int id);
        /*
        /// <summary>
        /// Obtenir le dernier poste d'une catégorie du forum
        /// </summary>
        /// <param name="id">Clé principale Id ForumCategorie</param>
        /// <returns></returns>
        Task<ForumPoste> GetDernierPosteForumCategorie(int id);*/

        /**
         * ForumPoste
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir tous les postes
        /// </summary>
        /// <param name="forumPosteParams">Pagination</param>
        /// <param name="id">Id du sujet ForumPoste</param>
        /// <returns></returns> 
        Task<PagedList<ForumPoste>> GetForumPostes(ForumPosteParams forumPosteParams, int id);
    }
}
