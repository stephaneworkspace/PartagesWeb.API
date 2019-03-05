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
    /// Controller pour model Article
    /// </summary>
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Articles", Description = "Controller pour model Article")]
    public class ArticlesController : ControllerBase
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
        public ArticlesController(IGestionPagesRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>
        /// Cette méthode permet d'acceder à un article bien précis
        /// </summary>
        /// <param name="id">Clé principale de l'article</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ArticleForReadDto), Description = "L'article à atteindre")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'acceder à l'article")]
        public async Task<IActionResult> GetArticle(int id)
        {
            var item = await _repo.GetArticle(id);
            if (item != null)
            {
                return Ok(_mapper.Map<ArticleForReadDto>(item));
            }
            else
            {
                return BadRequest("Impossible d'acceder à l'article");
            }
        }

        /// <summary>  
        /// Cette méthode permet de créer un article
        /// </summary> 
        /// <param name="dto">DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de l'article est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter l'article")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Contenu » est obligatoire.")]
        public async Task<IActionResult> Create(ArticleForCreateDto dto)
        {
            if (await _repo.ArticleExists(dto.Nom.ToLower(), dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null))
                return BadRequest("Le nom de l'article est déjà utilisé");

            // Déterminer la dernière position en ligne ou hors ligne
            var position = await _repo.LastPositionArticle(dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null);
            position++;

            // Préparation du model
            Article item = new Article
            {
                SousTitreMenuId = dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null,
                Nom = dto.Nom,
                Position = position,
                Contenu = dto.Contenu
            };

            _repo.Add(item);

            if (await _repo.SaveAll())
                return Ok(item);

            return BadRequest("Impossible d'ajouter l'article");
        }

        /// <summary>
        /// Cette méthode permet de mettre à jour un article
        /// </summary>
        /// <param name="id">Clé principale de l'article à éditer</param>
        /// <param name="dto">Dto de ce qui est envoyé par le frontend</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Le nom de l'article est déjà utilisé")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'ajouter l'article")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Contenu » est obligatoire.")]
        public async Task<IActionResult> Update(int id, ArticleForUpdateDto dto)
        {
            if (await _repo.ArticleExistsUpdate(id, dto.Nom.ToLower(), dto.SousTitreMenuId))
                return BadRequest("Le nom de l'article est déjà utilisé");

            var item = await _repo.GetArticle(id);
            var oldId = item.SousTitreMenuId;

            // Déterminer la positon
            if (item.SousTitreMenuId != dto.SousTitreMenuId)
            {
                // Déterminer la dernière position en ligne ou hors ligne
                var position = await _repo.LastPositionArticle(dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null);
                // Prochaine position
                position++;
                item.Position = position;
            }

            // Préparation du model
            item.Nom = dto.Nom;
            item.SousTitreMenuId = dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null;
            _repo.Update(item);

            if (await _repo.SaveAll())
            {
                // continue
            }
            else
            {
                return BadRequest("Impossible de mettre à jour l'article");
            }

            await _repo.SortPositionArticle(oldId);
            await _repo.SaveAll();
            return Ok(item);
        }

        /// <summary>  
        /// Cette méthode permet d'effacer un article et de remettre dans l'ordre les position en ligne et hors ligne
        /// </summary> 
        /// <remarks>
        /// 
        /// 8 Février : Mettre hors ligne l'arbre "titre menu - sous titre menu - article"
        /// 11 Février : Trouver un moyen de RollBack
        /// 18 Février : Ne pas tenir compte des 2 lignes précédant... je nettoyerai les remarques plus tard une fois que le code fonctionne
        /// </remarks> 
        /// <param name="id">Id de l'article à effacer</param>
        [HttpDelete("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer l'article")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _repo.GetArticle(id);
            int? sousTitreMenuId = item.SousTitreMenuId > 0 ? item.SousTitreMenuId : null;

            if (item != null)
            {
                _repo.Delete(item);
                await _repo.SaveAll();
            }
            else
            {
                return BadRequest("Impossible d'effacer l'article");
            }

            await _repo.SortPositionArticle(sousTitreMenuId);
            await _repo.SaveAll();
            return Ok();
        }

        /// <summary>  
        /// Cette méthode permet de monter un article
        /// </summary> 
        /// <param name="id"> Id de l'article à monter</param>
        [HttpPost("up/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de monter l'article")]
        public async Task<IActionResult> Up(int id)
        {
            var status = await _repo.UpArticle(id);
            if (!status)
            {
                return BadRequest("Impossible de monter l'article");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de monter l'article");
            }
        }

        /// <summary>  
        /// Cette méthode permet de descendre un article
        /// </summary> 
        /// <param name="id"> Id de l'article descendre</param>
        [HttpPost("down/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de descendre l'article")]
        public async Task<IActionResult> Down(int id)
        {
            var status = await _repo.DownArticle(id);
            if (!status)
            {
                return BadRequest("Impossible de descendre l'article");
            }
            else
            {
                if (await _repo.SaveAll())
                    return Ok();
                return BadRequest("Impossible de descendre l'article");
            }
        }
    }
}