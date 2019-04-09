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
    [SwaggerTag("ForumPostes", Description = "Controller pour model ForumPostes")]
    public class ForumPostesController : ControllerBase
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
        public ForumPostesController(IForumRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner les postes du forum à un sujet bien précis
        /// </summary> 
        /// <param name="forumPosteParams">Pagination</param>
        /// <param name="id">ForumSujet Id</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPosteForListDto[]), Description = "Liste des sections")]
        public async Task<IActionResult> GetForumPostes([FromQuery] ForumPosteParams forumPosteParams, int id)
        {
            var items = await _repo.GetForumPostes(forumPosteParams, id);
            var itemsDto = _mapper.Map<List<ForumPosteForListDto>>(items);
            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            // NombreDePostes MessageCount
            var itemsDtoFinal = new List<ForumPosteForListDto>();
            foreach (var itemDto in itemsDto)
            {
                itemDto.User.MessageCount = await _repo.GetCountUser(itemDto.UserId);
                itemsDtoFinal.Add(itemDto);
            }

            // var discussions = db.Discussions.ToList();

            return Ok(itemsDto);

            // A ajouter nombre de view par utilisateur
            // 



            /*

            List<ForumCategorieForListDto> newDto = new List<ForumCategorieForListDto>();
            foreach (var unite in items)
            {
                ForumCategorieForListDto Dto = new ForumCategorieForListDto
                {
                    Id = unite.Id,
                    Nom = unite.Nom
                };
                Dto.CountSujet = await _repo.GetCountSujet(unite.Id);
                Dto.CountPoste = await _repo.GetCountPosteForumCategorie(unite.Id);
                var dernierPoste = await _repo.GetDernierForumPosteDeUneCategorie(unite.Id);
                Dto.DernierPoste = _mapper.Map<ForumPosteForListForumCategorieDtoController>(dernierPoste);
                newDto.Add(Dto);
            }*/
            /*
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


            sectionsToReturn.Add(newSection);*/

            // return Ok(newDto);
        }
    }
}