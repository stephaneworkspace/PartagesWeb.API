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
    /// Repository pour la gestion des pages
    /// </summary>
    public interface IGestionPagesRepository
    {
        /// <summary>  
        /// Cette méthode permet d'ajouter une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity"> Entité (par exemple Section)</param>
        void Add<T>(T entity) where T : class;
        /// <summary>
        /// Cette méthode permet de mettre à jour l'entité dans le DataContext
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity"></param>
        void Update<T>(T entity) where T : class;
        /// <summary>  
        /// Cette méthode permet d'effacer une entité dans le DataContext
        /// </summary>  
        /// <param name="entity"> Entité (par exemple Section)</param>
        void Delete<T>(T entity) where T : class;
        /// <summary>  
        /// Cette méthode permet de sauvegarder tout dans le DataContext
        /// </summary> 
        Task<bool> SaveAll();
        /// <summary>  
        /// Cette méthode permet d'obtenir une section bien précise
        /// </summary>  
        /// <param name="id"> Clé principale du model Section</param>
        Task<Section> GetSection(int id);
        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les sections
        /// </summary>
        Task<List<Section>> GetSections();
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà
        /// </summary>  
        /// <param name="nom"> Nom de section</param>
        Task<bool> SectionExists(string nom);
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà et ignorer l'enregistrement en cours
        /// </summary>  
        /// <param name="id">Clé de l'enregistrement à igonrer</param>
        /// <param name="nom"> Nom de section</param>
        Task<bool> SectionExistsUpdate(int id, string nom);
        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="swHorsLigne"> Switch si on est en ligne true ou hors ligne false</param>
        Task<int> LastPositionSection(bool swHorsLigne);
        /// <summary>
        /// Cette méthode refait la liste des positions pour les sections
        /// </summary>
        Task<bool> SortPositionSections();
        /// <summary>
        /// Monter une section
        /// </summary>
        /// <param name="id"> Clé principale du model Section à monter</param>
        Task<bool> UpSection(int id);
        /// <summary>
        /// Descendre une section
        /// </summary>
        /// <param name="id"> Clé principale du model Section à descendre</param>
        Task<bool> DownSection(int id);
    }
}
