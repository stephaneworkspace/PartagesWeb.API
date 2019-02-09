using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SectionsController : ControllerBase
    {
        /// <summary>
        /// Repository GestionPages
        /// </summary>
        private readonly IGestionPagesRepository _repo;

        /// <summary>
        /// Configuration
        /// </summary>
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo"> Repository GestionPages</param>
        /// <param name="config"> Configuration</param>
        /// <returns></returns>
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
        /// <returns>Liste des <see cref="Section">Sections</see>.</returns>
        [HttpGet("gestion-pages-avec-arbre-complet")]
        [SwaggerResponse("200", typeof(Section))]
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
        /// <returns>Liste des <see cref="Section">Sections</see>.</returns>
        [HttpGet]
        [SwaggerResponse("200", typeof(Section))]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        /// <summary>  
        /// Cette méthode permet de créer une section
        /// </summary> 
        /// <param name="sectionForCreateDto"> DTO de ce qui est envoyé depuis le frontend</param>
        /// <returns>Ok</returns>
        /// <exception cref="BadRequestResult">Le nom de la section est déjà utilisé ou Impossible d'ajouter la section</exception>
        [HttpPost]
        [SwaggerResponse("400", typeof(string))]
        [SwaggerResponse("200", null)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Create(SectionForCreateDto sectionForCreateDto)
        {
            // Déterminer si le nom de la section existe déjà
            if (await _repo.SectionExists(sectionForCreateDto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé !");

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
                return Ok();

            return BadRequest("Impossible d'ajouter la section");
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une section
        /// </summary> 
        /// <remarks>
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// </remarks>/// 
        /// <param name="id"> Id de la section à effacer</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
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
 