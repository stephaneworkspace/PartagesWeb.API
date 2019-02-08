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
    public class SectionController : ControllerBase
    {
        private readonly IGestionPagesRepository _repo;
        private readonly IConfiguration _config;
        public SectionController(IGestionPagesRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        // GET api/categories
        [HttpGet]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(SectionForCreateDto sectionForCreateDto)
        {
            sectionForCreateDto.Nom = sectionForCreateDto.Nom.ToLower();

            if (await _repo.SectionExists(sectionForCreateDto.Nom))
                return BadRequest("Le nom de la section est déjà utilisé !");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSection(); // a faire sw boolean si en ligne ou hors ligne... 
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

            var createdSection = await _repo.CreateSection(sectionToCreate);

            return StatusCode(201);
        }
    }
}
 