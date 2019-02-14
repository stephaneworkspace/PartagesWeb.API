#pragma warning disable 1591
//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Icones", Description = "Controller pour model Icones")]
    public class IconesController : ControllerBase
    {
        private readonly IGestionPagesRepository _repo;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo"> Repository GestionPages</param>
        /// <param name="config"> Configuration</param>
        public IconesController(IGestionPagesRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner les icones
        /// </summary> 
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Icone[]), Description = "Liste des icones")]
        public async Task<IActionResult> GetIcones()
        {
            var items = await _repo.GetIcones();
            return Ok(items);
        }

        /// <summary>
        /// Cette méthode permet d'acceder à une icone précise
        /// </summary>
        /// <param name="id">Clé principale de l'icone à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section), Description = "L'iconne à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder à l'icone")]
        public async Task<IActionResult> GetIcone(int id)
        {
            var item = await _repo.GetIcone(id);

            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest("Impossible d'acceder à l'icone");
            }
        }

        /// <summary>
        /// A faire
        /// </summary>
        /// <remarks>Vérifier unique</remarks>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// A faire
        /// </summary>
        /// <remarks>Vérifier unique</remarks>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une icone
        /// </summary> 
        /// <param name="id"> Id de l'iconne à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer l'icone")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetIcone(id);

            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Impossible d'effacer l'icone");
            }
            return Ok();
        }

        /// <summary>  
        /// Cette méthode permet d'effacer toutes les icones
        /// </summary> 
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer les icones")]
        public async Task<IActionResult> DeleteIcones()
        {
            var items = await _repo.GetIcones();
            foreach (var item in items)
            {
                _repo.Delete(item);
            }
            await _repo.SaveAll();
            return Ok();
        }
    }
}
