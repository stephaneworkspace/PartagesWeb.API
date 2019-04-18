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
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de mettre à jour le nombre de vue du sujet")]
        public async Task<IActionResult> GetForumPostes([FromQuery] ForumPosteParams forumPosteParams, int id)
        {
            var boolTrue = await _repo.IncView(id);
            if (!boolTrue)
            {
                return BadRequest("Impossible de mettre à jour le nombre de vue du sujet");
            }
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
            return Ok(itemsDto);
        }

        /// <summary>
        /// Reponse ForumPoste à la fin de ForumSujet
        /// </summary>
        /// <param name="Dto">Dto Input</param>
        /// <returns></returns>
        /// <remarks>à faire tuto - 
        /// http://jasonwatmore.com/post/2019/01/08/aspnet-core-22-role-based-authorization-tutorial-with-example-api 
        /// la reponse a été trouvé ici
        /// https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core</remarks>
        [Authorize]
        [HttpPut]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste[]), Description = "Poste qui a été rajouté")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de répondre à ce poste")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Contenu » est obligatoire.")]
        public async Task<IActionResult> ReponseForumPoste(ForumPosteForReplyDto Dto)
        {
            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Préparation du model
            var Item = new ForumPoste
            {
                ForumSujetId = Dto.ForumSujetId,
                UserId = userId,
                Contenu = Dto.Contenu
            };

            _repo.Add(Item);

            if (await _repo.SaveAll())
                return Ok(Item);

            return BadRequest("Impossible de répondre à ce poste");
        }
    }
}