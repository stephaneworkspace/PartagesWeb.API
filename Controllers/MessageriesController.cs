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
using PartagesWeb.API.Models.Messagerie;

namespace PartagesWeb.API.Controllers
{
    /// <summary>
    /// Classe Controller pour Messagerie
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Messageries", Description = "Controller pour model Messagerie")]
    public class MessageriesController : ControllerBase
    {
        private readonly IMessagerieRepository _repo;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary>  
        /// <param name="repo">Repository Messagerie</param>
        /// <param name="mapper">Mapper de AutoMapper</param>
        /// <param name="config">Configuration</param>
        public MessageriesController(IMessagerieRepository repo, IMapper mapper, IConfiguration config)
        {
            _config = config;
            _mapper = mapper;
            _repo = repo;
        }
        /*

        /// <summary>  
        /// Cette méthode permet de retourner les postes du forum à un sujet bien précis
        /// </summary> 
        /// <param name="forumPosteParams">Pagination</param>
        /// <param name="id">ForumSujet Id</param>
        /// <remarks>
        /// ForumPosteForListDto => Pour Automap
        /// ForumPosteForListDtoWithVirtual => Avec champ suppl pas dans la DB en SQL
        /// </remarks>
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
            var itemsDtoFinal = new List<ForumPosteForListDtoWithVirtual>();
            foreach (var itemDto in itemsDto)
            {
                var itemDtoWithVirtual = new ForumPosteForListDtoWithVirtual();
                itemDtoWithVirtual.Id = itemDto.Id;
                itemDtoWithVirtual.ForumSujet = itemDto.ForumSujet;
                itemDtoWithVirtual.ForumSujetId = itemDto.ForumSujetId;
                itemDtoWithVirtual.User = itemDto.User;
                itemDtoWithVirtual.UserId = itemDto.UserId;
                itemDtoWithVirtual.Contenu = itemDto.Contenu;
                itemDtoWithVirtual.Date = itemDto.Date;
                if (int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) == itemDto.UserId)
                {
                    itemDtoWithVirtual.SwCurrentUser = true;
                }
                else
                {
                    itemDtoWithVirtual.SwCurrentUser = false;
                }
                itemDtoWithVirtual.User.MessageCount = await _repo.GetCountUser(itemDto.UserId);
                itemsDtoFinal.Add(itemDtoWithVirtual);
            }
            return Ok(itemsDtoFinal);
        }

        */
        [Authorize]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste), Description = "Message délivré")]
        public async Task<IActionResult> EnvoiMessagerie(MessagerieForNewMessageDto Dto)
        {
            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            // Préparation du model
            var Item = new Messagerie();
            // Item.SendByUserId
            return Ok(Item);
        }
        /*

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
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste), Description = "Poste qui a été rajouté")]
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
                Date = DateTime.Now,
                Contenu = Dto.Contenu
            };

            _repo.Add(Item);

            if (await _repo.SaveAll())
                return Ok(Item);

            return BadRequest("Impossible de répondre à ce poste");
        }

        /// <summary>  
        /// Cette méthode permet de retourner un poste bien précis
        /// </summary> 
        /// <param name="id">Clé principale ForumPoste</param>
        [HttpGet("ForumPosteId/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPosteForSelectDto), Description = "Poste à atteindre")]
        public async Task<IActionResult> GetForumSujet(int id)
        {
            var item = await _repo.GetForumPoste(id);
            ForumPosteForSelectDto newDto = new ForumPosteForSelectDto();
            var itemDto = _mapper.Map<ForumPosteForSelectDto>(item);
            return Ok(itemDto);
        }*/
    }
}