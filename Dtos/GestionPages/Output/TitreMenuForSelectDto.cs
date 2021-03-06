﻿using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Output
{
    /// <summary>
    /// Dto pour l'affichage du selectBox
    /// </summary>
    public class TitreMenuForSelectDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// SectionId relié à cette section
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Relation avec model Section
        /// </summary>
        public virtual SectionForSelectInsideDto Section { get; set; }
        /// <summary>
        /// Nom du titre menu
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Position
        /// </summary>
        public int Position { get; set; }
    }
}
