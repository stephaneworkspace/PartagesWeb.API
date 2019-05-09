using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Messagerie.Output
{
    /// <summary>
    /// Dto
    /// </summary>
    public class MessagerieForReadDto
    {
        /// <summary>
        /// Clé principale
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Message envoyé par
        /// </summary>
        public int? SendByUserId { get; set; }
        /// <summary>
        /// Date du message
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// Contenu du message
        /// </summary>
        public string Contenu { get; set; }
        /// <summary>
        /// Message lu
        /// </summary>
        public bool SwLu { get; set; }
    }
}
