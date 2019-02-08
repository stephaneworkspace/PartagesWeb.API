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
            if (await _context.Sections.AnyAsync(x => x.Nom == nom))
                return true;

            return false;
        }
    }
}
