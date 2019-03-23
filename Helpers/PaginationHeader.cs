using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Helpers
{
    /// <summary>
    /// Classe PaginationHeader, entête de la pagination
    /// </summary>
    public class PaginationHeader
    {
        /// <summary>
        /// Page actuel
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// Nombre d'éléments par pages
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// Nombre total d'éléments
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// Nombre total de pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="currentPage">Page actuel</param>
        /// <param name="itemsPerPage">Éléments par pages</param>
        /// <param name="totalItems">Total des éléments toutes les pages</param>
        /// <param name="totalPages">Total des pages</param>
        public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.ItemsPerPage = itemsPerPage;
            this.TotalItems = totalItems;
            this.TotalPages = totalPages;
        }
    }
}
