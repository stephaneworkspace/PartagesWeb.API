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
using PartagesWeb.API.Helpers.Forum;

namespace PartagesWeb.API.Controllers.Forum
{
    /// <summary>
    /// Classe Controller pour ForumCategories
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("ForumCategories", Description = "Controller pour model ForumCategories")]
    public class ForumCategoriesController : ControllerBase
    {
        private readonly IForumRepository _repo;
        // private readonly IMapper _mapper;
        // private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo">Repository Forum</param>
        /// <param name="mapper">Mapper de AutoMapper</param>
        /// <param name="config">Configuration</param>
        public ForumCategoriesController(IForumRepository repo)//, IMapper mapper, IConfiguration config)
        {
            // _config = config;
            // _mapper = mapper;
            _repo = repo;
        }

        /// <summary>  
        /// Cette méthode permet de retourner les catégories du forum
        /// </summary> 
        [HttpGet]
        // [SwaggerResponse(HttpStatusCode.OK, typeof(SectionForListDto[]), Description = "Liste des sections")]
        public async Task<IActionResult> GetSections([FromQuery] ForumCategorieParams forumCategorieParams)
        {
            var items = await _repo.GetForumCategories(forumCategorieParams);
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

            return Ok(items);
        }
    }
}