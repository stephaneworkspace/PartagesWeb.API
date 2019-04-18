﻿using System.Collections.Generic;
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
    }
}