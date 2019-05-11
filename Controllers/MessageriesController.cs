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
using PartagesWeb.API.Dtos.Messagerie.Output;
using PartagesWeb.API.Helpers;
using PartagesWeb.API.Helpers.Forum;
using PartagesWeb.API.Models;
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

        /// <summary>  
        /// Cette méthode permet de compter les messages non lu
        /// </summary> 
        /// <returns></returns>
        [Authorize]
        [HttpGet("Count")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste), Description = "Comptage effectué")]
        public async Task<IActionResult> GetCountNonLu()
        {
            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var count = 0;
            if (userId > 0)
            {
                count = await _repo.GetCountMessagesNonLu(userId);
            }
            return Ok(count);
        }

        /// <summary>  
        /// Cette méthode permet de retourner les postes du forum à un sujet bien précis
        /// </summary> 
        /// <param name="messagerieParams">Pagination</param>
        /// <remarks>
        /// 8 mai : a tester avec le frontend, je suis pas sur pour la partie "Reponse."
        /// </remarks>
        /// <returns></returns> 
        [Authorize]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessagerieForReadDtoWithVirtual[]), Description = "Liste des messages")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de mettre à jour le nombre de vue du sujet")]
        public async Task<IActionResult> GetMessageries([FromQuery] MessagerieParams messagerieParams)
        {
            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Messages
            var items = await _repo.GetMessageries(messagerieParams, userId);
            var itemsDto = _mapper.Map<List<MessagerieForReadDto>>(items);
            Response.AddPagination(items.CurrentPage, items.PageSize, items.TotalCount, items.TotalPages);
            // Dto Virtual
            var itemsDtoFinal = new List<MessagerieForReadDtoWithVirtual>();
            foreach (var itemDto in itemsDto)
            {
                var sendByUser = new UsersForReadMessagerieDto();
                if (itemDto.SendByUserId > 0)
                {
                    var itemUser = await _repo.GetSendByUser(itemDto.SendByUserId ?? default(int));
                    sendByUser = _mapper.Map<UsersForReadMessagerieDto>(itemUser);
                }
                var itemDtoWithVirtual = new MessagerieForReadDtoWithVirtual();
                itemDtoWithVirtual.Id = itemDto.Id;
                itemDtoWithVirtual.SendByUserId = itemDto.SendByUserId;
                itemDtoWithVirtual.SendByUser = sendByUser;
                itemDtoWithVirtual.Date = itemDto.Date;
                itemDtoWithVirtual.Contenu = itemDto.Contenu;
                itemDtoWithVirtual.SwLu = itemDto.SwLu;
                itemsDtoFinal.Add(itemDtoWithVirtual);
            }
            return Ok(itemsDtoFinal);
        }

        /// <summary>
        /// Envoi d'un message privé
        /// </summary>
        /// <param name="Dto">Dto Input</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ForumPoste), Description = "Message délivré")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible d'envoyer le message")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Utilisateur » est obligatoire.")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "error.errors.Nom[0] == Le champ « Contenu » est obligatoire.")]
        public async Task<IActionResult> EnvoiMessagerie(MessagerieForNewMessageDto Dto)
        {
            // Trouver l'utilisateur actuel
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            // Préparation du model
            var Item = new Messagerie
            {
                UserId = Dto.UserId,
                SendByUserId = userId,
                Date = DateTime.Now,
                Contenu = Dto.Contenu,
                SwLu = false
            };
            _repo.Add(Item);
            if (await _repo.SaveAll())
                return Ok(Item);
            return BadRequest("Impossible d'envoyer le message");
        }

        /// <summary>  
        /// Cette méthode permet d'ouvrir un message bien précis
        /// </summary> 
        /// <param name="id">Clé principale Messagerie</param>
        [Authorize]
        [HttpGet("{id}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(MessagerieForReadDtoWithVirtual), Description = "Message desormais lu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Impossible de mettre à jour le message en message lu")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "Accès refusé")]
        public async Task<IActionResult> GetMessagerie(int id)
        {
            var boolTrue = await _repo.SwLu(id);
            if (!boolTrue)
            {
                return BadRequest("Impossible de mettre à jour le message en message lu");
            }
            var item = await _repo.GetMessagerie(id);
            if (item.UserId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value))
            {
                MessagerieForReadDto newDto = new MessagerieForReadDto();
                var itemDto = _mapper.Map<MessagerieForReadDto>(item);
                // Dto Virtual
                var itemDtoFinal = new List<MessagerieForReadDtoWithVirtual>();
                // User
                var sendByUser = new UsersForReadMessagerieDto();
                if (itemDto.SendByUserId > 0)
                {
                    var itemUser = await _repo.GetSendByUser(itemDto.SendByUserId ?? default(int));
                    sendByUser = _mapper.Map<UsersForReadMessagerieDto>(itemUser);
                }
                var itemDtoWithVirtual = new MessagerieForReadDtoWithVirtual();
                itemDtoWithVirtual.Id = itemDto.Id;
                itemDtoWithVirtual.SendByUserId = itemDto.SendByUserId;
                itemDtoWithVirtual.SendByUser = sendByUser;
                itemDtoWithVirtual.Date = itemDto.Date;
                itemDtoWithVirtual.Contenu = itemDto.Contenu;
                itemDtoWithVirtual.SwLu = itemDto.SwLu;
                itemDtoFinal.Add(itemDtoWithVirtual);
                return Ok(itemDtoFinal);
            }
            else
            {
                return BadRequest("Accès refusé");
            }
        }
    }
}