using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetSousTitreMenu(int id)
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
            var item = new Article();
            item.SousTitreMenuId = dto.SousTitreMenuId > 0 ? dto.SousTitreMenuId : null;
            item.Nom = dto.Nom;
            item.Position = position;
            item.Contenu = dto.Contenu;

            _repo.Add(item);

            if (await _repo.SaveAll())
                return Ok(item);

            return BadRequest("Impossible d'ajouter l'article");
        }
    }
}