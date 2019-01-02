using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PartagesWeb.API.Data;

namespace PartagesWeb.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        // GET api/categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public IActionResult GetCategorie(int id)
        {
            var categorie = _context.Categories.FirstOrDefault(x => x.Id == id);
            return Ok(categorie);
        }

        // POST api/categories
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/categories/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
