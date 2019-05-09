using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Dtos.Messagerie.Output
{
    /// <summary>
    /// Dto
    /// </summary>
    public class MessagerieForReadDtoWithVirtual
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
        /// Message envoyé par virtuel
        /// </summary>
        public UsersForReadMessagerieDto SendByUser { get; set; }
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