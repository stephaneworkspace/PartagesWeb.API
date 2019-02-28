
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
                .Select(p => SortIncludeSection(p))
                // .OrderBy(u => u.TitreMenus.OrderBy(b => b.Position).FirstOrDefault().Position)
                .ToListAsync();

            return sections;
        }

        /// <summary>
        /// Sort include
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/15378136/entity-framework-ordering-includes</remarks>
        /// <param name="p"></param>
        /// <returns></returns>
        private Section SortIncludeSection(Section p)
        {
            p.TitreMenus = (p.TitreMenus as HashSet<TitreMenu>)?
                .OrderBy(s => s.Position)
                .Select(x => SortIncludeTitreMenu(x))
                .ToHashSet<TitreMenu>();
            return p;
        }

        /// <summary>
        /// Sort include
        /// </summary>
        /// <remarks>https://stackoverflow.com/questions/15378136/entity-framework-ordering-includes</remarks>
        /// <param name="p"></param>
        /// <returns></returns>
        private TitreMenu SortIncludeTitreMenu(TitreMenu p)
        {
            p.SousTitreMenus = (p.SousTitreMenus as HashSet<SousTitreMenu>)?
                .OrderBy(s => s.Position)
                .ToHashSet<SousTitreMenu>();
            return p;
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
        /// Cette méthode permet d'obtenir un titre de menu bien précise
        /// </summary>  
        /// <param name="id">Clé principale du model TitreMenu</param>
        public async Task<TitreMenu> GetTitreMenu(int id)
        {
            var item = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == id);

            return item;
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les titre de menus hors ligne int? SectionId
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
        /// Cette méthode permet de vérifier si un nom de titre de menu existe déjà
        /// </summary>  
        /// <param name="nom">Nom du titre de menu</param>
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
        /// Cette méthode permet de vérifier si un nom de titre de menu existe déjà dans le cas d'une mise à jour des données
        /// </summary>  
        /// <param name="id">Id de la clé principal de TitreMenu à mettre à jour et donc ignorer</param>
        /// <param name="nom">Nom du titre de menu</param>
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
        /// Cette méthode refait la liste des positions pour les titre de menus
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

        /// <summary>
        /// Monter un titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model TitreMenu à monter</param>
        public async Task<bool> UpTitreMenu(int id)
        {
            var recordEnCours = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                int? sectionId = recordEnCours.SectionId > 0 ? recordEnCours.SectionId : null;
                var titreMenus = await _context.TitreMenus
                    .Where(w => w.SectionId == sectionId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                titreMenus.Reverse();
                foreach (var unite in titreMenus)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position++;
                        _context.TitreMenus.Update(recordInversion);
                        break;
                    }
                    else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position--;
                            swFind = true;
                            _context.TitreMenus.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Descendre un titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model TitreMenu à descendre</param>
        public async Task<bool> DownTitreMenu(int id)
        {
            var recordEnCours = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                int? sectionId = recordEnCours.SectionId > 0 ? recordEnCours.SectionId : null;
                var titreMenus = await _context.TitreMenus
                    .Where(w => w.SectionId == sectionId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                foreach (var unite in titreMenus)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.TitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position--;
                        _context.TitreMenus.Update(recordInversion);
                        break;
                    }
                    else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position++;
                            swFind = true;
                            _context.TitreMenus.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }
        }

        /**
         * SousTitreMenu
         */

        /// <summary>  
        /// Cette méthode permet d'obtenir un sous titre de menu bien précise
        /// </summary>  
        /// <param name="id">Clé principale du model SousTitreMenu</param>
        public async Task<SousTitreMenu> GetSousTitreMenu(int id)
        {
            var item = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == id);

            return item;
        }

        /// <summary>  
        /// Cette méthode permet d'obtenir toutes les sous titre de menus hors ligne int? TitreMenuId
        /// </summary>
        public async Task<List<SousTitreMenu>> GetSousTitreMenuHorsLigne()
        {
            var items = await _context.SousTitreMenus
                .Where(x => x.TitreMenuId == null)
                .OrderBy(y => y.Position)
                .ToListAsync();

            return items;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de sous titre de menu existe déjà
        /// </summary>  
        /// <param name="nom">Nom du sous titre de menu</param>
        /// <param name="titreMenuId">TitremenuId int? ou le nom doit être unique</param>
        public async Task<bool> SousTitreMenuExists(string nom, int? titreMenuId)
        {
            if (await _context.SousTitreMenus
                .Where(s => s.TitreMenuId == titreMenuId)
                .AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de sous titre de menu existe déjà dans le cas d'une mise à jour des données
        /// </summary>  
        /// <param name="id">Id de la clé principal de SousTitreMenu à mettre à jour et donc ignorer</param>
        /// <param name="nom">Nom du sous titre de menu</param>
        /// <param name="titreMenuId">TitreMenuid int? ou le nom doit être unique</param>
        public async Task<bool> SousTitreMenuExistsUpdate(int id, string nom, int? titreMenuId)
        {
            if (await _context.SousTitreMenus
                .Where(x => x.Id != id)
                .Where(s => s.TitreMenuId == titreMenuId)
                .AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
                return true;
            return false;
        }

        /// <summary>  
        /// Cette méthode permet de détermine la dernière position
        /// </summary>  
        /// <param name="titreMenuId">TitreMenuId facultatif int?</param>
        public async Task<int> LastPositionSousTitreMenu(int? titreMenuId)
        {
            int lastPositon;
            lastPositon = _context.SousTitreMenus
                .Where(x => titreMenuId == x.TitreMenuId)
                .OrderByDescending(x => x.Position)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }

        /// <summary>
        /// Cette méthode refait la liste des positions pour les sous titre de menus
        /// </summary>
        /// <param name="titreMenuId">Clé du model TitreMenu int? pour trier ce titre de menu</param>
        public async Task<bool> SortPositionSousTitreMenu(int? titreMenuId)
        {
            var i = 0;
            var titreMenus = await _context.SousTitreMenus
                .Where(x => titreMenuId == x.TitreMenuId)
                .OrderBy(x => x.Position)
                .ToListAsync();
            foreach (var unite in titreMenus)
            {
                var record = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                i++;
                record.Position = i;
                _context.SousTitreMenus.Update(record);
            }
            return true;
        }

        /// <summary>
        /// Monter un sous titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model SousTitreMenu à monter</param>
        public async Task<bool> UpSousTitreMenu(int id)
        {
            var recordEnCours = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                int? titreMenuId = recordEnCours.TitreMenuId > 0 ? recordEnCours.TitreMenuId : null;
                var sousTitreMenus = await _context.SousTitreMenus
                    .Where(w => w.TitreMenuId == titreMenuId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                sousTitreMenus.Reverse();
                foreach (var unite in sousTitreMenus)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position++;
                        _context.SousTitreMenus.Update(recordInversion);
                        break;
                    }
                    else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position--;
                            swFind = true;
                            _context.SousTitreMenus.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Descendre un sous titre de menu
        /// </summary>
        /// <param name="id">Clé principale du model SousTitreMenu à descendre</param>
        public async Task<bool> DownSousTitreMenu(int id)
        {
            var recordEnCours = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == id);
            if (recordEnCours == null)
            {
                return false;
            }
            else
            {
                var positionRecordEnCours = recordEnCours.Position;
                int? titreMenuId = recordEnCours.TitreMenuId > 0 ? recordEnCours.TitreMenuId : null;
                var sousTitreMenus = await _context.SousTitreMenus
                    .Where(w => w.TitreMenuId == titreMenuId)
                    .OrderBy(x => x.Position)
                    .ToListAsync();
                var swFind = false;
                foreach (var unite in sousTitreMenus)
                {
                    // id principal est déjà trouvé, donc le prochain logiquement arrive ici
                    if (swFind)
                    {
                        var recordInversion = await _context.SousTitreMenus.FirstOrDefaultAsync(x => x.Id == unite.Id);
                        recordInversion.Position--;
                        _context.SousTitreMenus.Update(recordInversion);
                        break;
                    }
                    else
                    {
                        if (unite.Id == id)
                        {
                            recordEnCours.Position++;
                            swFind = true;
                            _context.SousTitreMenus.Update(recordEnCours);
                        }
                    }
                }
                return true;
            }
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
