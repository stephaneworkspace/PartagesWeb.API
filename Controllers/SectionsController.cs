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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos.GestionPages;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Controllers
{
    /// <summary>
    /// Controller pour model Section
    /// </summary>
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Sections", Description = "Controller pour model Section")]
    public class SectionsController : ControllerBase
    {
        private readonly IGestionPagesRepository _repo;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo"> Repository GestionPages</param>
        /// <param name="config"> Configuration</param>
        public SectionsController(IGestionPagesRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner l'arbre complet avec un mapping special qui permet d'avoir
        /// la hiérachie section - titre menu - sous titre menu - article
        /// </summary>  
        /// <remarks>
        /// 8 Février : 
        /// A faire automap avec l'arbre entier, pour le moment il y a seulement "section"
        /// </remarks>
        [HttpGet("gestion-pages-avec-arbre-complet")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section), Description = "Liste des Sections")]
        public async Task<IActionResult> GetArbreCompletSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        /// <summary>  
        /// Cette méthode permet de retourner les sections
        /// </summary> 
        /// <remarks>
        /// 8 Février : 
        /// Non utilisé pour le moment. Il faudra peut être l'améliorer avec un choix, aucune section (pour le mode hors ligne)
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section), Description = "Liste des Sections")]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        /// <summary>  
        /// Cette méthode permet de créer une section
        /// </summary> 
        /// <remarks>
        /// 9 février : Status Created comme dans Auth à faire
        /// </remarks>
        /// <param name="sectionForCreateDto"> DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description="Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de la section est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter la section")]
        public async Task<IActionResult> Create(SectionForCreateDto sectionForCreateDto)
        {
            if (await _repo.SectionExists(sectionForCreateDto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSection(sectionForCreateDto.SwHorsLigne); // a faire sw boolean si en ligne ou hors ligne... 
            
            // Prochaine position
            position++;

            // Préparation du model
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
                return Ok(sectionToCreate);

            return BadRequest("Impossible d'ajouter la section");
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une section et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// </remarks>/// 
        /// <param name="id"> Id de la section à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer la section")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetSection(id);

            if (item != null)
            {
                _repo.Delete(item);
            }

            await _repo.SortPositionSections();

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Impossible d'effacer la section");
        }

        /// <summary>  
        /// Cette méthode permet de monter une section
        /// </summary> 
        /// <param name="id"> Id de la section à monter</param>
        [HttpPost("monter/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de monter la section")]
        public async Task<IActionResult> Monter(int id)
        {
            var status = await _repo.UpSection(id);
            if (!status)
            {
                return BadRequest("Impossible de monter la section");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de monter la section");
            }
        }

        /// <summary>  
        /// Cette méthode permet de descendre une section
        /// </summary> 
        /// <param name="id"> Id de la section à descendre</param>
        [HttpPost("descendre/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de descendre la section")]
        public async Task<IActionResult> Descendre(int id)
        {
            var status = await _repo.DownSection(id);
            if (!status)
            {
                return BadRequest("Impossible de descendre la section");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de descendre la section");
            }
        }
    }
}
 