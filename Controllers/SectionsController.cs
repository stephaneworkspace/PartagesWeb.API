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
    [Authorize]
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
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section[]), Description = "Liste des sections")]
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
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section[]), Description = "Liste des sections")]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            return Ok(sections);
        }

        /// <summary>
        /// Cette méthode permet d'acceder à une section précise
        /// </summary>
        /// <param name="id">Clé principale de la section à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(Section), Description = "La section à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder à la section")]
        public async Task<IActionResult> GetSection(int id)
        {
            var item = await _repo.GetSection(id);

            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest("Impossible d'acceder à la section");
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer une section
        /// </summary> 
        /// <param name="sectionForCreateDto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de la section est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter la section")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Icone[0] == Le champ « Icone » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Type[0] == Le champ « Type de section » est obligatoire.")]
        public async Task<IActionResult> Create(SectionForCreateDto sectionForCreateDto)
        {
            if (await _repo.SectionExists(sectionForCreateDto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSection(sectionForCreateDto.SwHorsLigne);
            
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
        /// Cette méthode permet de modifier une section
        /// </summary> 
        /// <param name="id">Clé de l'enregistrement</param>
        /// <param name="sectionForUpdateDto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de la section est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de mettre à jour la section")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Icone[0] == Le champ « Icone » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Type[0] == Le champ « Type de section » est obligatoire.")]
        public async Task<IActionResult> Update(int id, SectionForUpdateDto sectionForUpdateDto)
        {
            if (await _repo.SectionExistsUpdate(id, sectionForUpdateDto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé");
            var section = await _repo.GetSection(id);

            // Déterminer la positon
            if (section.SwHorsLigne != sectionForUpdateDto.SwHorsLigne)
            {
                // Déterminer la dernière position en ligne ou hors ligne
                var position = await _repo.LastPositionSection(sectionForUpdateDto.SwHorsLigne);
                // Prochaine position
                position++;
                section.Position = position;
            }

            // Préparation du model
            section.Nom = sectionForUpdateDto.Nom;
            section.Icone = sectionForUpdateDto.Icone;
            section.Type = sectionForUpdateDto.Type;
            section.SwHorsLigne = sectionForUpdateDto.SwHorsLigne;

            _repo.Update(section);

            if (await _repo.SaveAll())
            {
                // continue
            }
            else
            {
                return BadRequest("Impossible de mettre à jour la section");
            }

            await _repo.SortPositionSections();
            await _repo.SaveAll();
            return Ok(section);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une section et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// 11 Février : Trouver un moyen de RollBack
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
                await _repo.SaveAll();
            } else
            {
                return BadRequest("Impossible d'effacer la section");
            }

            // Pas de vérification, ça pourrait être le dernier enregistrement
            await _repo.SortPositionSections();
            await _repo.SaveAll();
            return Ok();
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
 