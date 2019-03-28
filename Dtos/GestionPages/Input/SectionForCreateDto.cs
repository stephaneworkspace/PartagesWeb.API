//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages.Input
{
    /// <summary>
    /// Dto pour la création d'une section depuis le frontend
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization?view=aspnetcore-2.2#globalization-and-localization-terms
    /// </remarks>
    public class SectionForCreateDto
    {
        /// <summary>
        /// Nom de la section
        /// </summary>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        public string Nom { get; set; }
        /// <summary>
        /// Icone fa de font awesome
        /// </summary>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        public string Icone { get; set; }
        /// <summary>
        /// Type de section
        /// </summary>
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment il n'y a pas de model relié à ça
        /// </remarks>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        [DisplayName("Type de section")]
        public string Type { get; set; }
        /// <summary>
        /// Switch si c'est en ligne true, si c'est hors ligne false
        /// </summary>
        public bool SwHorsLigne { get; set; }
    }
}
