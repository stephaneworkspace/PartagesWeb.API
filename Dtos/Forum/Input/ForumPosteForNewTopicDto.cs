using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Input
{
    /// <summary>
    /// Dto pour la création d'un nouveau sujet dans le forum
    /// </summary>
    public class ForumPosteForNewTopicDto
    {
        /***
         * ForumPoste
         */

        /// <summary>
        /// Clé principal du topic
        /// </summary>
        public int ForumSujetId { get; set; }
        /// <summary>
        /// Clé principal de l'utilisateur
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Date du poste
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Nom du topic
        /// </summary>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        public string Contenu { get; set; }

        /***
         * ForumSujet
         */

        /// <summary>
        /// Nom du sujet pour créer le record ForumSujet / ForumSujetId
        /// </summary>
        public string NomSujet { get; set; }
        /// <summary>
        /// Id de la catégorie en relation avec ForumSujet
        /// </summary>
        public int ForumCategorieId { get; set; }
    }
}
