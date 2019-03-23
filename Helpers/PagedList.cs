using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Helpers
{
    /// <summary>
    ///  Pagination
    /// </summary>
    /// <typeparam name="T">Entité T au choix</typeparam>
    public class PagedList<T> : List<T>
    {
        /// <summary>
        /// Page actuel
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Nombre total de pages
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Taille de la page
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Total éléments toutes les pages
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="items">record de entité T au choix</param>
        /// <param name="count">Total éléments toutes les pages</param>
        /// <param name="pageNumber">Nombre de pages</param>
        /// <param name="pageSize">Taille des pages</param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }


        /// <summary>
        /// Méthode CreateAsync
        /// </summary>
        /// <param name="source">Source entité IQueryable T au choix</param>
        /// <param name="pageNumber">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns></returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
