using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Data
{
    public class GestionPagesRepository : IGestionPagesRepository
    {
        private readonly DataContext _context;

        public GestionPagesRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Section> GetSection(int id)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(x => x.Id == id);

            return section;
        }

        public async Task<List<Section>> GetSections()
        {
            var sections = await _context.Sections.ToListAsync();//.Include(p => p.Photos).ToListAsync();

            return sections;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

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

        /*
         * Détermine la dernière position
         * J'ai trouvé le DefaultIfEmpty(0) avec Max ici : (pour info 8 février)
         * https://stackoverflow.com/questions/7542021/how-to-get-max-value-of-a-column-using-entity-framework
         */
        public async Task<int> LastPositionSection(bool swHorsLigne)
        {
            int lastPositon = _context.Sections
                .Where(x => swHorsLigne == x.SwHorsLigne)
                .Select(p => p.Position)
                .DefaultIfEmpty(0)
                .Max();
            await Task.FromResult(lastPositon);
            return lastPositon;
        }
    }
}
