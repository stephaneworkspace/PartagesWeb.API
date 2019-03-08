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
    /// Dto pour la création d'un article de menu depuis le frontend
    /// </summary>
    public class ArticleForSeedDto
    {
        /// <summary>
        /// Nom SousTitreMenu unique
        /// </summary>
        public string NomSousTitreMenu { get; set; }
        /// <summary>
        /// Nom de l'article
        /// </summary>
        public string Nom { get; set; }
        /// <summary>
        /// Contenu de l'article
        /// </summary>
        public string Contenu { get; set; }
    }
}
