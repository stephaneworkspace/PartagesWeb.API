//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo">Repository GestionPages</param>
        /// <param name="mapper">Mapper de AutoMapper</param>
        /// <param name="config">Configuration</param>
        public TitreMenusController(IGestionPagesRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Cette méthode permet d'acceder à un titre de menu précis
        /// </summary>
        /// <param name="id">Clé principale du titre menu à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TitreMenuForReadDto), Description = "Le titre de menu à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder au titre du menu")]
        public async Task<IActionResult> GetTitreMenu(int id)
        {
            var item = await _repo.GetTitreMenu(id);
            if (item != null)
            {
                return Ok(_mapper.Map<TitreMenuForReadDto>(item));
            }
            else
            {
                return BadRequest("Impossible d'acceder au titre du menu");
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer un titre de menu
        /// </summary> 
        /// <param name="titreMenuForCreateDto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du titre du menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le titre du menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Create(TitreMenuForCreateDto titreMenuForCreateDto)
        {
            if (await _repo.TitreMenuExists(titreMenuForCreateDto.Nom.ToLower(), titreMenuForCreateDto.SwHorsLigne == true ? null : titreMenuForCreateDto.SectionId))
                return BadRequest("Le nom du titre du menu est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionTitreMenu(titreMenuForCreateDto.SectionId > 0 ? titreMenuForCreateDto.SectionId : null);
            position++;

            // Préparation du model
            var titreMenuToCreate = new TitreMenu();
            titreMenuToCreate.SectionId = titreMenuForCreateDto.SectionId > 0 ? titreMenuForCreateDto.SectionId : null;
            titreMenuToCreate.Nom = titreMenuForCreateDto.Nom;
            titreMenuToCreate.Position = position;

            _repo.Add(titreMenuToCreate);

            if (await _repo.SaveAll())
                return Ok(titreMenuToCreate);

            return BadRequest("Impossible d'ajouter le titre du menu");
        }

        // Update a faire

        /// <summary>  
        /// Cette méthode permet d'effacer un titre de menu et de remettre dans l'ordre les position en ligne et hors ligne
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
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer le titre du menu")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetTitreMenu(id);
            int? sectionId = item.SectionId > 0 ? item.SectionId  : null;

            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Impossible d'effacer le titre du menu");
            }

            await _repo.SortPositionTitreMenu(sectionId);
            await _repo.SaveAll();
            return Ok();
        }

        /// <summary>  
        /// Cette méthode permet de monter un titre de menu
        /// </summary> 
        /// <param name="id"> Id du titre de menu à monter</param>
        [HttpPost("up/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de monter le titre de menu")]
        public async Task<IActionResult> Up(int id)
        {
            var status = await _repo.UpTitreMenu(id);
            if (!status)
            {
                return BadRequest("Impossible de monter le titre de menu");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de monter le titre de menu");
            }
        }

        /// <summary>  
        /// Cette méthode permet de descendre un titre de menu
        /// </summary> 
        /// <param name="id"> Id du titre de menu à descendre</param>
        [HttpPost("down/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de descendre le titre de menu")]
        public async Task<IActionResult> Down(int id)
        {
            var status = await _repo.DownTitreMenu(id);
            if (!status)
            {
                return BadRequest("Impossible de descendre le titre de menu");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de descendre le titre de menu");
            }
        }
    }
}