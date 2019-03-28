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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos.GestionPages.Input;
using PartagesWeb.API.Dtos.GestionPages.Output;
using PartagesWeb.API.Models;

namespace PartagesWeb.API.Controllers
{
    /// <summary>
    /// Controller pour model Section
    /// </summary>
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Sections", Description = "Controller pour model Section")]
    public class SectionsController : ControllerBase
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
        public SectionsController(IGestionPagesRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner les sections
        /// </summary> 
        /// <remarks>
        /// 24 février à tester : 
        /// if (newSection.TitreMenus.Count > 0)
        /// 24 février : (pas urgent, mais fais un unit test de ça)
        /// Vérifier si unique Nom ainsi que mise offline
        /// 5 mars fais le reverse pour titre menu sous titre menu... ne fonctionne pas à lire comme ça
        /// </remarks>
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(SectionForListDto[]), Description = "Liste des sections")]
        public async Task<IActionResult> GetSections()
        {
            var sections = await _repo.GetSections();
            var titreMenusHorsLigne = await _repo.GetTitreMenuHorsLigne();
            var sousTitreMenusHorsLigne = await _repo.GetSousTitreMenuHorsLigne();
            var articleHorsLigne = await _repo.GetArticleHorsLigne();

            var sectionsToReturn = _mapper.Map<List<SectionForListDto>>(sections);
            var titreMenusHorsLigneToReturn = _mapper.Map<List<TitreMenuForListDto>>(titreMenusHorsLigne);
            var sousTitreMenusHorsLigneToReturn = _mapper.Map<List<SousTitreMenuForListDto>>(sousTitreMenusHorsLigne);
            var articleHorsLigneToReturn = _mapper.Map<List<ArticleForListDto>>(articleHorsLigne);

            // Section
            SectionForListDto newSection = new SectionForListDto
            {
                Id = default(int),
                Nom = "Titre de menus hors ligne",
                Icone = "toggle-off",
                Position = 1,
                SwHorsLigne = true
            };

            // Titre Menu
            TitreMenuForListDto newTitreMenu = new TitreMenuForListDto
            {
                Id = default(int),
                Nom = "Sous titre de menus hors ligne",
                // newTitreMenu.Position = 1;
                SousTitreMenus = sousTitreMenusHorsLigneToReturn
            };

            // Sous titre menu
            SousTitreMenuForListDto newSousTitreMenu = new SousTitreMenuForListDto
            {
                Id = default(int),
                Nom = "Articles hors ligne",
                Articles = articleHorsLigneToReturn
            };
            newTitreMenu.SousTitreMenus.Add(newSousTitreMenu);


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

            return Ok(sectionsToReturn);
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
        /// <param name="dto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de la section est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter la section")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Icone[0] == Le champ « Icone » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Type[0] == Le champ « Type de section » est obligatoire.")]
        public async Task<IActionResult> Create(SectionForCreateDto dto)
        {
            if (await _repo.SectionExists(dto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionSection(dto.SwHorsLigne);
            
            // Prochaine position
            position++;

            // Préparation du model
            var item = new Section
            {
                Nom = dto.Nom,
                Icone = dto.Icone,
                Type = dto.Type,
                Position = position,
                SwHorsLigne = dto.SwHorsLigne
            };

            _repo.Add(item);

            if (await _repo.SaveAll())
                return Ok(item);

            return BadRequest("Impossible d'ajouter la section");
        }

        /// <summary>  
        /// Cette méthode permet de modifier une section
        /// </summary> 
        /// <param name="id">Clé de l'enregistrement</param>
        /// <param name="dto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de la section est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de mettre à jour la section")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Icone[0] == Le champ « Icone » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Type[0] == Le champ « Type de section » est obligatoire.")]
        public async Task<IActionResult> Update(int id, SectionForUpdateDto dto)
        {
            if (await _repo.SectionExistsUpdate(id, dto.Nom.ToLower()))
                return BadRequest("Le nom de la section est déjà utilisé");
            var item = await _repo.GetSection(id);

            // Déterminer la positon
            if (item.SwHorsLigne != dto.SwHorsLigne)
            {
                // Déterminer la dernière position en ligne ou hors ligne
                var position = await _repo.LastPositionSection(dto.SwHorsLigne);
                // Prochaine position
                position++;
                item.Position = position;
            }

            // Préparation du model
            item.Nom = dto.Nom;
            item.Icone = dto.Icone;
            item.Type = dto.Type;
            item.SwHorsLigne = dto.SwHorsLigne;

            _repo.Update(item);

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
            return Ok(item);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer une section et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// 11 Février : Trouver un moyen de RollBack
        /// </remarks>/// 
        /// <param name="id">Id de la section à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer la section")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetSection(id);

            if (item != null)
            {
                await _repo.DeleteSection(item);
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
        [HttpPost("up/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de monter la section")]
        public async Task<IActionResult> Up(int id)
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
        [HttpPost("down/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de descendre la section")]
        public async Task<IActionResult> Down(int id)
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
 