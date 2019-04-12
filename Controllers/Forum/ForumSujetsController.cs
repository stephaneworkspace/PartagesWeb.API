using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos.Forum.Output;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Helpers.Forum;

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
        /// Cette méthode permet de retourner les postes du forum à un sujet bien précis
        /// </summary> 
        /// <param name="forumSujetParams">Pagination</param>
        /// <param name="id">ForumCategorie Id</param>
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumSujetForListDto[]), Description = "Liste des sections")]
        public async Task<IActionResult> GetForumSujets([FromQuery] ForumSujetParams forumSujetParams, int id)
        {
            var items = await _repo.GetForumSujets(forumSujetParams, id);
            var itemsDto = _mapper.Map<List<ForumSujetForListDto>>(items);
            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            // NombreDePostes MessageCount
            /*var itemsDtoFinal = new List<ForumSujetForListDto>();
            foreach (var itemDto in itemsDto)
            {
                itemDto.User.MessageCount = await _repo.GetCountUser(itemDto.UserId);
                itemsDtoFinal.Add(itemDto);
            }*/ // a faire foreach imbriqué et ForumSujetForListDto dans catégorie voir comment c^'est map les count
            return Ok(itemsDto);
        }
    }
}