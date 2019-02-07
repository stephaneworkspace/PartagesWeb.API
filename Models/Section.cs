using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Icone { get; set; }
        public string Type { get; set; }
        public int Position { get; set; }
    }
}
