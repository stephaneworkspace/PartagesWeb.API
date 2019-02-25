using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages
{
    /// <summary>
    /// Dto pour l'affichage du titre menu à éditer et traitement du champ SectionId int?
    /// </summary>
    public class TitreMenuForReadDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SectionId si sélectioné (sinon 0)
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
    }
}
