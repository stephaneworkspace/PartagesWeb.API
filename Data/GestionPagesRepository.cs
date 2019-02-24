
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Data
{
    /// <summary>
    /// Repository pour la gestion des pages
    /// </summary>
    public class GestionPagesRepository : IGestionPagesRepository
    {
        private readonly DataContext _context;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="context"> DataContext</param>
        public GestionPagesRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>  
        /// Cette méthode permet d'ajouter une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity"> Entité (par exemple Section)</param>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        /// <summary>
        /// Cette méthode permet de mettre à jour l'entité dans le DataContext
        /// </summary>
        /// <typeparam name="T">Type d'entité</typeparam>
        /// <param name="entity"></param>
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une entité dans le DataContext
        /// </summary>  
        /// <typeparam name="T">Type d'entité</typeparam>/// 
        /// <param name="entity"> Entité (par exemple Section)</param>
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
         * Sections
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir une section bien précise
        /// </summary>  
        /// <param name="id">Clé principale du model Section</param>
        public async Task<Section> GetSection(int id)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id);

            return section;
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les sections
        /// </summary> 
        public async Task<List<Section>> GetSections()
        {
            var sections = await _context.Sections
                .OrderBy(x => x.SwHorsLigne)
                .ThenBy(y => y.Position)
                .Include(t => t.TitreMenus)
                .ToListAsync();//.Include(p => p.Photos).ToListAsync();

            return sections;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà
        /// </summary>  
        /// <param name="nom">Nom de section</param>
        public async Task<bool> SectionExists(string nom)
        {
            if (await _context.Sections.AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà et ignorer l'enregistrement en cours
        /// </summary>  
        /// <param name="id">Clé de l'enregistrement à igonrer</param>
        /// <param name="nom"> Nom de section</param>
        public async Task<bool> SectionExistsUpdate(int id, string nom)
        {
            if (await _context.Sections.Where(x => x.Id != id)
                .AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /* 8 février - désactivé, utilisation de public void Add<T>(T entity)
        public async Task<Section> CreateSection(Section section)
        {
            await _context.Sections.AddAsync(section);
            await _context.SaveChangesAsync();

            return section;
        }*/

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <remarks>
        /// 9 février, je n'ai pas testé le OrderByDescending que j'ai rajouté
        /// </remarks>
        /// <param name="swHorsLigne">Switch si on est en ligne true ou hors ligne false</param>
        public async Task<int> LastPositionSection(bool swHorsLigne)
        {
            int lastPositon = _context.Sections
                .Where(x => swHorsLigne == x.SwHorsLigne)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)                
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }

        /// <summary>
        /// Cette méthode refait la liste des positions pour les sections
        /// </summary>
        /// <remark>
        /// Cette méthode est copier collé dans Seed
        /// </remark>
        public async Task<bool> SortPositionSections()
        {
            var sections = await _context.Sections
                .OrderBy(x => x.SwHorsLigne)
                .ThenBy(x => x.Position)
                .ToListAsync();
            var i = 0;
            var j = 0;
            foreach(var unite in sections)
            {
                var record = await _context.Sections.FirstOrDefaultAsync(x => x.Id == unite.Id);
                if (unite.SwHorsLigne == true)
                {
                    i++;
                    record.Position = i;
                }
                else
                {
                    j++;
                    record.Position = j;
                }
                _context.Sections.Update(record);
            }
            return true;
        }

        /// <summary>
        /// Cette méthode permet d'effacer une entitée Section ainsi que rendre offline TitreMenu[]
        /// </summary>
        /// <param name="section">Section à effacer</param>
        public async Task<bool> DeleteSection(Section section)
        {

            int lastPositonTitreMenusOffline = _context.TitreMenus
                .Where(x => x.SectionId == null)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();


            var titreMenus = await _context.TitreMenus
                .Where(x => x.SectionId == section.Id)
                .ToListAsync();
            // 23 février : LAST OFFLINE POSITION A TESTER


            foreach (var unite in titreMenus)
            {
                unite.SectionId = null;
                unite.Position = lastPositonTitreMenusOffline++;
                _context.TitreMenus.Update(unite);
            }
            _context.Sections.Remove(section);
            return true;
        }

        /// <summary>
        /// Monter une section
        /// </summary>
        /// <param name="id">Clé principale du model Section à monter</param>
        public async Task<bool> UpSection(int id)
        {
            var recordEnCours = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                var swHorsLigne = recordEnCours.SwHorsLigne;
                var sections = await _context.Sections
                    .Where(w => w.SwHorsLigne == swHorsLigne)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                sections.Reverse();
                foreach (var unite in sections)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.Sections.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position++;
                        _context.Sections.Update(recordInversion);
                        break;
                    } else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position--;
                            swFind = true;
                            _context.Sections.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }            
        }

        /// <summary>
        /// Descendre une section
        /// </summary>
        /// <param name="id">Clé principale du model Section à descendre</param>
        public async Task<bool> DownSection(int id)
        {
            var recordEnCours = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                var swHorsLigne = recordEnCours.SwHorsLigne;
                var sections = await _context.Sections
                    .Where(w => w.SwHorsLigne == swHorsLigne)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                foreach (var unite in sections)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.Sections.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position--;
                        _context.Sections.Update(recordInversion);
                        break;
                    }
                    else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position++;
                            swFind = true;
                            _context.Sections.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }
        }

        /**
         * TitreMenus
         */

        /// <summary>  
        /// Cette méthode permet d'obtenir un titre menu bien précise
        /// </summary>  
        /// <param name="id">Clé principale du model TitreMenu</param>
        public async Task<TitreMenu> GetTitreMenu(int id)
        {
            var item = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == id);

            return item;
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les titre menus hors ligne int? SectionId
        /// </summary>
        public async Task<List<TitreMenu>> GetTitreMenuHorsLigne()
        {
            var items = await _context.TitreMenus
                .Where(x => x.SectionId == null) // default(int) ? 24 février
                .OrderBy(y => y.Position)
                .ToListAsync();//.Include(p => p.TitreMenus).ToListAsync();

            return items;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà
        /// </summary>  
        /// <param name="nom">Nom du titre menu</param>
        /// <param name="sectionId">SectionId int? ou le nom doit être unique</param>
        public async Task<bool> TitreMenuExists(string nom, int? sectionId)
        {
            if (await _context.TitreMenus
                .Where(s => s.SectionId == sectionId)
                .AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà dans le cas d'une mise à jour des données
        /// </summary>  
        /// <param name="id">Id de la clé principal de TitreMenu à mettre à jour et donc ignorer</param>
        /// <param name="nom">Nom du titre menu</param>
        /// <param name="sectionId">SectionId int? ou le nom doit être unique</param>
        public async Task<bool> TitreMenuExistsUpdate(int id, string nom, int? sectionId)
        {
            if (await _context.TitreMenus
                .Where(x => x.Id != id)
                .Where(s => s.SectionId == sectionId)
                .AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="sectionId">SectionId facultatif int?</param>
        public async Task<int> LastPositionTitreMenu(int? sectionId)
        {
            int lastPositon;
            lastPositon = _context.TitreMenus
                .Where(x => sectionId == x.SectionId)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }

        /// <summary>
        /// Cette méthode refait la liste des positions pour les titre menus
        /// </summary>
        /// <param name="sectionId">Clé du model Section int?</param>
        public async Task<bool> SortPositionTitreMenu(int? sectionId)
        {
            var i = 0;
            var sections = await _context.TitreMenus
                .Where(x => sectionId == x.SectionId)
                .OrderBy(x => x.Position)
                .ToListAsync();
            foreach (var unite in sections)
            {
                var record = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                i++;
                record.Position = i;
                _context.TitreMenus.Update(record);
            }
            return true;
        }

        public Task<bool> UpTitreMenu(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DownTitreMenu(int id)
        {
            throw new NotImplementedException();
        }

        /**
         * Icones
         **/

        /// <summary>  
        /// Cette méthode permet d'obtenir une icone bien précise
        /// </summary>  
        /// <param name="id"> Clé principale du model Icone</param>
        public async Task<Icone> GetIcone(int id)
        {
            return await _context.Icones.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les icones
        /// </summary> 
        public async Task<List<Icone>> GetIcones()
        {
            return await _context.Icones
                .OrderBy(x => x.NomSelectBox)
                .ToListAsync();
        }

     }
}
