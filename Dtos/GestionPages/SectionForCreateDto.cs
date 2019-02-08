using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.GestionPages
{
    public class SectionForCreateDto
    {
        [Required]
        public string Nom { get; set; }

        [Required]
        public string Icone { get; set; }

        [Required]
        public string Type { get; set; }

        public bool SwHorsLigne { get; set; }
    }
}
