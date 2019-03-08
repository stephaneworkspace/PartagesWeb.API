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

namespace PartagesWeb.API.Dtos.GestionPages
{
    /// <summary>
    /// Dto pour la création d'un sous titre de menu depuis le frontend
    /// </summary>
    public class SousTitreMenuForSeedDto
    {
        /// <summary>
        /// Nom TitreMenu unique
        /// </summary>
        public string NomTitreMenu { get; set; }
        /// <summary>
        /// Nom du sous titre de menu
        /// </summary>
        public string Nom { get; set; }
    }
}
