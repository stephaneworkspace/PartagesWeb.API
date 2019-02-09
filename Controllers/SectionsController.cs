using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos.GestionPages;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SectionsController : ControllerBase
    {
        private readonly IGestionPagesRepository _repo;
        private readonly IConfiguration _config;
        public SectionsController(IGestionPagesRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpGet("gestion-pages-avec-arbre-complet")]
        public async Task<IActionResult> GetArbreCompletSections()
        {
            // 8 février - à faire automap
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SectionForCreateDto sectionForCreateDto)
        {
            // sectionForCreateDto.Nom = sectionForCreateDto.Nom.ToLower();

            if (await _repo.SectionExists(sectionForCreateDto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé !");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSection(sectionForCreateDto.SwHorsLigne); // a faire sw boolean si en ligne ou hors ligne... 
            // une fois que create, read fonctionne migration dans entity framework

            position++;

            var sectionToCreate = new Section
            {
                Nom = sectionForCreateDto.Nom,
                Icone = sectionForCreateDto.Icone,
                Type = sectionForCreateDto.Type,
                Position = position,
                SwHorsLigne = sectionForCreateDto.SwHorsLigne
            };

            _repo.Add(sectionToCreate);

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Impossible d'ajouter la section");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetSection(id);

            if (item != null)
            {
                _repo.Delete(item);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Impossible d'effacer la section");
        }
    }
}
 