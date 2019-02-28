﻿//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages
{
    /// <summary>
    /// Dto pour la création d'un titre de menu depuis le frontend
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#globalization-and-localization-terms
    /// </remarks>
    public class TitreMenuForCreateDto
    {
        /// <summary>
        /// SectionId si sélectioné (sinon 0)
        /// </summary>
        public int? SectionId { get; set; }
        /// <summary>
        /// Nom du titre de menu
        /// </summary>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        public string Nom { get; set; }
    }
}
