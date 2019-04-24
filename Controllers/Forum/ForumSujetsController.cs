using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos.Forum.Input;
using PartagesWeb.API.Dtos.Forum.Output;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Helpers.Forum;
using PartagesWeb.API.Models.Forum;

namespace PartagesWeb.API.Controllers.Forum
{
    /// <summary>
    /// Classe Controller pour ForumPoste
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("ForumSujets", Description = "Controller pour model ForumSujets")]
    public class ForumSujetsController : ControllerBase
    {
        private readonly IForumRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo">Repository Forum</param>
        /// <param name="mapper">Mapper de AutoMapper</param>
        /// <param name="config">Configuration</param>
        public ForumSujetsController(IForumRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner la liste des sujets
        /// </summary> 
        /// <param name="forumSujetParams">Pagination</param>
        /// <param name="id">ForumCategorie Id</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumSujetForListDto[]), Description = "Liste des sujets")]
        public async Task<IActionResult> GetForumSujets([FromQuery] ForumSujetParams forumSujetParams, int id)
        {
            var items = await _repo.GetForumSujets(forumSujetParams, id);
            List<ForumSujetForListDto> newDto = new List<ForumSujetForListDto>();
            foreach (var unite in items)
            {
                ForumSujetForListDto Dto = new ForumSujetForListDto
                {
                    Id = unite.Id,
                    Nom = unite.Nom,
                    ForumCategorieId = unite.ForumCategorieId,
                    ForumCategorie = _mapper.Map<ForumCategorieForListForumSujetDto>(unite.ForumCategorie),
                    Date = unite.Date,
                    View = unite.View
                };
                Dto.CountPoste = await _repo.GetCountPosteForumSujet(unite.Id);
                newDto.Add(Dto);
            }
            // BACKUP avant procedure automap
            // var itemsDto = _mapper.Map<List<ForumSujetForListDto>>(items);
            // Traitement après procedure automap
            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            return Ok(newDto);
        }

        /// <summary>  
        /// Cette méthode permet de retourner un sujet bien précis
        /// </summary> 
        /// <param name="id">Clé principale ForumSujet</param>
        [HttpGet("ForumSujetId/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumSujetForSelectDto), Description = "Sujet à atteindre")]
        public async Task<IActionResult> GetForumSujet(int id)
        {
            var item = await _repo.GetForumSujet(id);
            ForumSujetForSelectDto newDto = new ForumSujetForSelectDto();
            var itemDto = _mapper.Map<ForumSujetForSelectDto>(item);
            return Ok(itemDto);
        }

        /// <summary>
        /// Reponse ForumPoste à la fin de ForumSujet
        /// </summary>
        /// <param name="Dto">Dto Input</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste), Description = "Poste + Sujet qui est été rajouté")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de créer le sujet")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Contenu » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Nom du sujet » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Catégorie du sujet » est obligatoire.")]
        public async Task<IActionResult> NouveauSujetEtPoste(ForumPosteForNewTopicDto Dto)
        {
            // Préparation du record ForumSujet
            var ItemForumSujet = new ForumSujet
            {
                Date = DateTime.Now,
                ForumCategorieId = Dto.ForumCategorieId,
                Nom = Dto.NomSujet,
                View = 0
            };

            _repo.Add(ItemForumSujet);

            if (! await _repo.SaveAll()) {
                return BadRequest("Impossible de créer le sujet");
            }

            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Préparation du model
            var Item = new ForumPoste
            {
                ForumSujetId = ItemForumSujet.Id,
                UserId = userId,
                Date = DateTime.Now,
                Contenu = Dto.Contenu
            };

            _repo.Add(Item);

            if (await _repo.SaveAll())
                return Ok(Item);

            return BadRequest("Impossible de créer le sujet");
        }

        /// <summary>
        /// Effacer un sujet
        /// Delete en cascade entre ForumSujet et ForumPoste
        /// </summary>
        /// <param name="id">ForumSujetId</param>
        /// <returns></returns>
        /// <remarks>magouille pour effacer des erreurs, la fonction n'est pas utilisé
        /// j'implementrai cette fonction plus tard depuis le panel administrateur quand j'aurrai programé les droits et autorisation 
        /// entre user simple et admin</remarks>
        [Authorize]
        [HttpDelete]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'effacer le sujet")]
        public async Task<IActionResult> EffacerSujetMagouille(int id)
        {
            var item = await _repo.GetForumSujet(id);

            if (item != null)
            { 
                _repo.Delete(item);
                if (await _repo.SaveAll())
                    return Ok();
            }

            return BadRequest("Impossible d'effacer le sujet");
        }
    }
}