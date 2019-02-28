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

        /**
         * Sections
         **/

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
        /// <param name="nom">Nom de section</param>
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
        /// <param name="swHorsLigne">Switch si on est en ligne true ou hors ligne false</param>
        Task<int> LastPositionSection(bool swHorsLigne);
        /// <summary>
        /// Cette méthode refait la liste des positions pour les sections
        /// </summary>
        Task<bool> SortPositionSections();
        /// <summary>
        /// Cette méthode permet d'effacer une entitée Section ainsi que rendre offline TitreMenu[]
        /// </summary>
        /// <param name="section">Section à effacer</param>
        /// <returns></returns>
        Task <bool> DeleteSection(Section section);
        /// <summary>
        /// Monter une section
        /// </summary>
        /// <param name="id">Clé principale du model Section à monter</param>
        Task<bool> UpSection(int id);
        /// <summary>
        /// Descendre une section
        /// </summary>
        /// <param name="id">Clé principale du model Section à descendre</param>
        Task<bool> DownSection(int id);

        /**
         * TitreMenus
         */

        /// <summary> Cette méthode permet d'obtenir un titre de menu bien précis</summary>
        /// <param name="id"> Clé principale du model TitreMenu</param> 
        Task<TitreMenu> GetTitreMenu(int id);
        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les titre de menus hors ligne int? SectionId
        /// </summary>
        Task<List<TitreMenu>> GetTitreMenuHorsLigne();
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de titre de menu existe déjà
        /// </summary>  
        /// <param name="nom">Nom du titre de menu</param>
        /// <param name="sectionId">SectionId int? ou le nom doit être unique</param>
        Task<bool> TitreMenuExists(string nom, int? sectionId);
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de titre de menu existe déjà dans le cas d'une mise à jour des données
        /// </summary>  
        /// <param name="id">Id de la clé principal de TitreMenu à mettre à jour et donc ignorer</param>
        /// <param name="nom">Nom du titre de menu</param>
        /// <param name="sectionId">SectionId int? ou le nom doit être unique</param>
        Task<bool> TitreMenuExistsUpdate(int id, string nom, int? sectionId);
        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="sectionId">SectionId facultatif int?</param>
        Task<int> LastPositionTitreMenu(int? sectionId);
        /// <summary>
        /// Cette méthode refait la liste des positions pour les titre de menus
        /// </summary>
        /// <param name="sectionId">Clé du model Section int? pour trier cette section</param>
        Task<bool> SortPositionTitreMenu(int? sectionId);
        /// <summary>
        /// Monter un titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model TitreMenu à monter</param>
        Task<bool> UpTitreMenu(int id);
        /// <summary>
        /// Descendre un titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model TitreMenu à descendre</param>
        Task<bool> DownTitreMenu(int id);

        /**
         * SousTitreMenus
         */

        /// <summary> Cette méthode permet d'obtenir un sous titre de menu bien précis</summary>
        /// <param name="id"> Clé principale du model SousTitreMenu</param> 
        Task<SousTitreMenu> GetSousTitreMenu(int id);
        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les sous titre de menus hors ligne int? TitreMenuId
        /// </summary>
        Task<List<SousTitreMenu>> GetSousTitreMenuHorsLigne();
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de sous titre de menu existe déjà
        /// </summary>  
        /// <param name="nom">Nom du sous titre de menu</param>
        /// <param name="titreMenuId">TitremenuId int? ou le nom doit être unique</param>
        Task<bool> SousTitreMenuExists(string nom, int? titreMenuId);
        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de sous titre de menu existe déjà dans le cas d'une mise à jour des données
        /// </summary>  
        /// <param name="id">Id de la clé principal de SousTitreMenu à mettre à jour et donc ignorer</param>
        /// <param name="nom">Nom du sous titre de menu</param>
        /// <param name="titreMenuId">TitreMenuid int? ou le nom doit être unique</param>
        Task<bool> SousTitreMenuExistsUpdate(int id, string nom, int? titreMenuId);
        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="titreMenuId">TitreMenuId facultatif int?</param>
        Task<int> LastPositionSousTitreMenu(int? titreMenuId);
        /// <summary>
        /// Cette méthode refait la liste des positions pour les sous titre de menus
        /// </summary>
        /// <param name="titreMenuId">Clé du model TitreMenu int? pour trier ce titre de menu</param>
        Task<bool> SortPositionSousTitreMenu(int? titreMenuId);
        /// <summary>
        /// Monter un sous titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model SousTitreMenu à monter</param>
        Task<bool> UpSousTitreMenu(int id);
        /// <summary>
        /// Descendre un sous titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model SousTitreMenu à descendre</param>
        Task<bool> DownSousTitreMenu(int id);

        /**
         * Icones
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir une icone bien précise
        /// </summary>  
        /// <param name="id"> Clé principale du model Icone</param>
        Task<Icone> GetIcone(int id);
        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les icones
        /// </summary> 
        Task<List<Icone>> GetIcones();
    }
}
