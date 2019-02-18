//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    /// Controller pour model TitreMenu
    /// </summary>
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("TitreMenus", Description = "Controller pour model TitreMenu")]
    public class TitreMenusController : ControllerBase
    {
        private readonly IGestionPagesRepository _repo;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo"> Repository GestionPages</param>
        /// <param name="config"> Configuration</param>
        public TitreMenusController(IGestionPagesRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        /// <summary>
        /// Cette méthode permet d'acceder à un titre menu précis
        /// </summary>
        /// <param name="id">Clé principale du titre menu à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TitreMenu), Description = "Le titre menu à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder au titre menu")]
        public async Task<IActionResult> GetTitreMenu(int id)
        {
            var item = await _repo.GetTitreMenu(id);

            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return BadRequest("Impossible d'acceder au titre menu");
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer un titre menu
        /// </summary> 
        /// <param name="titreMenuForCreateDto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du titre menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le titre menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Create(TitreMenuForCreateDto titreMenuForCreateDto)
        {
            if (await _repo.TitreMenuExists(titreMenuForCreateDto.Nom.ToLower(), titreMenuForCreateDto.SectionId, titreMenuForCreateDto.SwHorsLigne))
                return BadRequest("Le nom du titre menu est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionTitreMenu(titreMenuForCreateDto.SwHorsLigne, titreMenuForCreateDto.SectionId);

            // Prochaine position
            position++;

            // Préparation du model
            var titreMenuToCreate = new TitreMenu
            {
                SectionId = titreMenuForCreateDto.SectionId,
                Nom = titreMenuForCreateDto.Nom,
                Position = position,
                SwHorsLigne = titreMenuForCreateDto.SwHorsLigne
            };

            _repo.Add(titreMenuToCreate);

            if (await _repo.SaveAll())
                return Ok(titreMenuToCreate);

            return BadRequest("Impossible d'ajouter le titre menu");
        }

        // Update a faire

        /// <summary>  
        /// Cette méthode permet d'effacer un titre menu et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// 11 Février : Trouver un moyen de RollBack
        /// 18 Février : Ne pas tenir compte des 2 lignes précédant... je nettoyerai les remarques plus tard une fois que le code fonctionne
        /// </remarks>/// 
        /// <param name="id">Id du titre menu à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer le titre menu")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetTitreMenu(id);
            var swHorsLigne = item.SwHorsLigne;
            int sectionId = item.SectionId ?? default(int);

            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Impossible d'effacer le titre menu");
            }

            await _repo.SortPositionTitreMenu(swHorsLigne, sectionId);
            await _repo.SaveAll();
            return Ok();
        }
    }
}