using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Helpers;

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
    }
}
