using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Input
{
    /// <summary>
    /// Dto pour la création d'un nouveau sujet dans le forum
    /// </summary>
    public class MessagerieForNewMessageDto
    {
        /// <summary>
        /// Utilisateur de destination
        /// </summary>
        /// <remarks>Va être SendByUserId dans Messagerie et nom pas UserId</remarks>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        [DisplayName("Utilisateur")]
        public int UserId { get; set; }

        /// <summary>
        /// Contenu du message
        /// </summary>
        [Required(ErrorMessage = "Le champ « {0} » est obligatoire.")]
        public string Contenu { get; set; }
    }
}