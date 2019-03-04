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
        /// Cette méthode permet de retourner les titres menu d'une clé principale section
        /// </summary> 
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TitreMenuForSelectDto[]), Description = "Liste des titres du menus")]
        public async Task<IActionResult> GetTitreMenus()
        {
            var titreMenus = await _repo.GetTitreMenus();
            var titreMenusToReturn = _mapper.Map<List<TitreMenuForSelectDto>>(titreMenus);
            var newTitreMenu = new TitreMenuForSelectDto();
            newTitreMenu.Id = default(int);
            newTitreMenu.Nom = "Titre de menus hors ligne";
            newTitreMenu.SectionId = default(int);
            newTitreMenu.Section = new SectionForSelectInsideDto();
            newTitreMenu.Section.Id = default(int);
            newTitreMenu.Section.Nom = "Hors ligne";
            titreMenusToReturn.Add(newTitreMenu);

            /*
            var sections = await _repo.GetSections();
            var titreMenusHorsLigne = await _repo.GetTitreMenuHorsLigne();
            var sousTitreMenusHorsligne = await _repo.GetSousTitreMenuHorsLigne();

            var sectionsToReturn = _mapper.Map<List<SectionForListDto>>(sections);
            var titreMenusHorsLigneToReturn = _mapper.Map<List<TitreMenuForListDto>>(titreMenusHorsLigne);
            var sousTitreMenusHorsLigneToReturn = _mapper.Map<List<SousTitreMenuForListDto>>(sousTitreMenusHorsligne);

            // Section
            var newSection = new SectionForListDto();
            newSection.Id = default(int);
            newSection.Nom = "Titre de menus hors ligne";
            newSection.Icone = "toggle-off";
            newSection.Position = 1;
            newSection.SwHorsLigne = true;

            // Titre Menu
            var newTitreMenu = new TitreMenuForListDto();
            newTitreMenu.Id = default(int);
            newTitreMenu.Nom = "Sous titre de menus hors ligne";
            // newTitreMenu.Position = 1;
            newTitreMenu.SousTitreMenus = sousTitreMenusHorsLigneToReturn;


            var swFind = false;
            var position = 0;
            titreMenusHorsLigneToReturn.Reverse();
            foreach (var unite in titreMenusHorsLigneToReturn)
            {
                if (swFind == false)
                {
                    swFind = true;
                    position = unite.Position;
                }
            }
            titreMenusHorsLigneToReturn.Reverse();
            position++;
            newTitreMenu.Position = position;



            titreMenusHorsLigneToReturn.Add(newTitreMenu);

            // Section
            newSection.TitreMenus = titreMenusHorsLigneToReturn;

            sectionsToReturn.Add(newSection);

            return Ok(sectionsToReturn);*/
            return Ok(titreMenusToReturn);
        }

        /// <summary>
        /// Cette méthode permet d'acceder à un titre de menu précis
        /// </summary>
        /// <param name="id">Clé principale du titre menu à atteindre</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(TitreMenuForReadDto), Description = "Le titre du menu à atteindre")]
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
        /// <param name="dto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du titre du menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le titre du menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Create(TitreMenuForCreateDto dto)
        {
            if (await _repo.TitreMenuExists(dto.Nom.ToLower(), dto.SectionId > 0 ? dto.SectionId : null))
                return BadRequest("Le nom du titre du menu est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionTitreMenu(dto.SectionId > 0 ? dto.SectionId : null);
            position++;

            // Préparation du model
            var item = new TitreMenu();
            item.SectionId = dto.SectionId > 0 ? dto.SectionId : null;
            item.Nom = dto.Nom;
            item.Position = position;

            _repo.Add(item);

            if (await _repo.SaveAll())
                return Ok(item);

            return BadRequest("Impossible d'ajouter le titre du menu");
        }

        /// <summary>
        /// Cette méthode permet de mettre à jour un titre de menu
        /// </summary>
        /// <param name="id">Clé principale de titre de menu à éditer</param>
        /// <param name="dto">Dto de ce qui est envoyé par le frontend</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom du titre du menu est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter le titre du menu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        public async Task<IActionResult> Update(int id, TitreMenuForUpdateDto dto)
        {
            if (await _repo.TitreMenuExistsUpdate(id, dto.Nom.ToLower(), dto.SectionId))
                return BadRequest("Le nom du titre de menu est déjà utilisé");

            var item = await _repo.GetTitreMenu(id);
            var oldId = item.SectionId;

            // Déterminer la positon
            if (item.SectionId != dto.SectionId)
            {
                // Déterminer la dernière position en ligne ou hors ligne
                var position = await _repo.LastPositionTitreMenu(dto.SectionId > 0 ? dto.SectionId : null);
                // Prochaine position
                position++;
                item.Position = position;
            }

            // Préparation du model
            item.Nom = dto.Nom;
            item.SectionId = dto.SectionId > 0 ? dto.SectionId : null;
            _repo.Update(item);

            if (await _repo.SaveAll())
            {
                // continue
            }
            else
            {
                return BadRequest("Impossible de mettre à jour le titre du menu");
            }

            await _repo.SortPositionTitreMenu(oldId);
            await _repo.SaveAll();
            return Ok(item);
        }

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