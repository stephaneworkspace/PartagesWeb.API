
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
        /// <param name="entity"> Entité (par exemple Section)</param>
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une entité dans le DataContext
        /// </summary>  
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

        /// <summary>  
        /// Cette méthode permet d'obtenir une section bien précise
        /// </summary>  
        /// <param name="id"> Clé principale du model Section</param>
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
            var sections = await _context.Sections.ToListAsync();//.Include(p => p.Photos).ToListAsync();

            return sections;
        }



        /// <summary>  
        /// Cette méthode permet de vérifier si un nom de section existe déjà
        /// </summary>  
        /// <param name="nom"> Nom de section</param>
        public async Task<bool> SectionExists(string nom)
        {
            if (await _context.Sections.AnyAsync(x => x.Nom.ToLower() == nom.ToLower()))
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
        /// <param name="swHorsLigne"> Switch si on est en ligne true ou hors ligne false</param>
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
        /// <remarks>
        /// 9 février, je n'ai pas testé la section
        /// Vérifier que la clé principale ne change pas
        /// </remarks>
        public async Task<bool> SortPositionSections()
        {
            // 1ère étape les position en ligne
            var sectionsEnLigne = await _context.Sections
                .Where(x => false == x.SwHorsLigne)
                .OrderBy(x => x.Position)
                .ToListAsync();
            var i = 0;
            foreach(var unite in sectionsEnLigne)
            {
                // _context.Sections.Remove(unite);
                i++;
                unite.Position = i;
                // await _context.Sections.AddAsync(unite);
            }
            // 1ère étape les position en ligne
            var sectionsHorsLigne = await _context.Sections
                .Where(x => true == x.SwHorsLigne)
                .OrderBy(x => x.Position)
                .ToListAsync();
            i = 0;
            foreach (var unite in sectionsHorsLigne)
            {
                // _context.Sections.Remove(unite);
                i++;
                unite.Position = i;
                // await _context.Sections.AddAsync(unite);
            }
            return true;
        }
    }
}
