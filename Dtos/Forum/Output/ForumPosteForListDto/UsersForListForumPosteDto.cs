using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartagesWeb.API.Dtos.Forum.Output
{
    public class UsersForListForumPosteDto
    {
        public int Id;
        public string Username;
        public DateTime Created { get; set; }
        public int? MessageCount;
    }
}
