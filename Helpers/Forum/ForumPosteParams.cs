﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Helpers.Forum
{
    /// <summary>
    /// Classe des params de ForumPoste
    /// </summary>
    public class ForumPosteParams
    {
        /// <summary>
        /// Maximum page size
        /// </summary>
        private const int MaxPageSize = 50;
        /// <summary>
        /// Numéro de pages
        /// </summary>
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;
        /// <summary>
        /// Taille de la page
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        // public int UserId { get; set; }
        // public string Gender { get; set; }
        // public int MinAge { get; set; } = 18;
        // public int MaxAge { get; set; } = 99;
        // public string OrderBy { get; set; }
    }
}
