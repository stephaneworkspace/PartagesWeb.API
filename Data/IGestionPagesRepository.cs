using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Data
{
    public interface IGestionPagesRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<Section> GetSection(int id);
        // serait plus propre avec un helper (pour le filtrage, mais pour le panneau admin pas de filtrage)
        // avec PagedList
        Task<List<Section>> GetSections();
        // Task<Section> Login(string username, string password);
        Task<bool> SectionExists(string nom);
        // 8 févier - desactivé - utilisation de public void Add<T>(T entity)
        // Task<Section> CreateSection(Section section);
        Task<int> LastPositionSection(bool swHorsLigne);
    }
}
