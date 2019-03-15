using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models.Forum
{
    public class ForumCategorie
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        public string Nom { get; set; }
    }
}
