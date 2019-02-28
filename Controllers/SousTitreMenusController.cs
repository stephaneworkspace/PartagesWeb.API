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
    /// Controller pour model SousTitreMenu
    /// </summary>
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("SousTitreMenus", Description = "Controller pour model SousTitreMenu")]
    public class SousTitreMenusController : ControllerBase
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
        public SousTitreMenusController(IGestionPagesRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Cette méthode permet d'acceder à un sous titre de menu précis
        /// </summary>
        /// <param name="id">Clé principale du sous titre menu à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SousTitreMenuForReadDto), Description = "Le sous titre de menu à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder au sous titre du menu")]
        public async Task<IActionResult> GetSousTitreMenu(int id)
        {
            var item = await _repo.GetSousTitreMenu(id);
            if (item != null)
            {
                return Ok(_mapper.Map<SousTitreMenuForReadDto>(item));
            }
            else
            {
                return BadRequest("Impossible d'acceder au sous titre du menu");
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer un sous titre de menu
        /// </summary> 
        /// <param name="dto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du sous titre du menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le sous titre du menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Create(SousTitreMenuForCreateDto dto)
        {
            if (await _repo.TitreMenuExists(dto.Nom.ToLower(), dto.TitreMenuId > 0 ? dto.TitreMenuId : null))
                return BadRequest("Le nom du sous titre du menu est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSousTitreMenu(dto.TitreMenuId > 0 ? dto.TitreMenuId : null);
            position++;

            // Préparation du model
            var item = new SousTitreMenu();
            item.TitreMenuId = dto.TitreMenuId > 0 ? dto.TitreMenuId : null;
            item.Nom = dto.Nom;
            item.Position = position;

            _repo.Add(item);

            if (await _repo.SaveAll())
                return Ok(item);

            return BadRequest("Impossible d'ajouter le sous titre du menu");
        }

        /// <summary>
        /// Cette méthode permet de mettre à jour un sous titre de menu
        /// </summary>
        /// <param name="id">Clé principale de titre de menu à éditer</param>
        /// <param name="dto">Dto de ce qui est envoyé par le frontend</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du sous titre du menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le sous titre du menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Update(int id, SousTitreMenuForUpdateDto dto)
        {
            if (await _repo.SousTitreMenuExistsUpdate(id, dto.Nom.ToLower(), dto.TitreMenuId))
                return BadRequest("Le nom du sous titre de menu est déjà utilisé");

            var item = await _repo.GetSousTitreMenu(id);
            var oldId = item.TitreMenuId;

            // Déterminer la positon
            if (item.TitreMenuId != dto.TitreMenuId)
            {
                // Déterminer la dernière position en ligne ou hors ligne
                var position = await _repo.LastPositionSousTitreMenu(dto.TitreMenuId > 0 ? dto.TitreMenuId : null);
                // Prochaine position
                position++;
                item.Position = position;
            }

            // Préparation du model
            item.Nom = dto.Nom;
            item.TitreMenuId = dto.TitreMenuId > 0 ? dto.TitreMenuId : null;
            _repo.Update(item);

            if (await _repo.SaveAll())
            {
                // continue
            }
            else
            {
                return BadRequest("Impossible de mettre à jour le titre du menu");
            }

            await _repo.SortPositionSousTitreMenu(oldId);
            await _repo.SaveAll();
            return Ok(item);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer un sous titre de menu et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// 11 Février : Trouver un moyen de RollBack
        /// 18 Février : Ne pas tenir compte des 2 lignes précédant... je nettoyerai les remarques plus tard une fois que le code fonctionne
        /// </remarks> 
        /// <param name="id">Id du sous titre de menu à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer le sous titre du menu")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetSousTitreMenu(id);
            int? titreMenuId = item.TitreMenuId > 0 ? item.TitreMenuId : null;

            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Impossible d'effacer le sous titre du menu");
            }

            await _repo.SortPositionSousTitreMenu(titreMenuId);
            await _repo.SaveAll();
            return Ok();
        }

        /// <summary>  
        /// Cette méthode permet de monter un sous titre de menu
        /// </summary> 
        /// <param name="id"> Id du sous titre de menu à monter</param>
        [HttpPost("up/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de monter le sous titre de menu")]
        public async Task<IActionResult> Up(int id)
        {
            var status = await _repo.UpSousTitreMenu(id);
            if (!status)
            {
                return BadRequest("Impossible de monter le sous titre de menu");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de monter le sous titre de menu");
            }
        }

        /// <summary>  
        /// Cette méthode permet de descendre un sous titre de menu
        /// </summary> 
        /// <param name="id"> Id du sous titre de menu à descendre</param>
        [HttpPost("down/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de descendre le sous titre de menu")]
        public async Task<IActionResult> Down(int id)
        {
            var status = await _repo.DownSousTitreMenu(id);
            if (!status)
            {
                return BadRequest("Impossible de descendre le sous titre de menu");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de descendre le sous titre de menu");
            }
        }
    }
}