//-----------------------------------------------------------------------
// <license>https://github.com/stephaneworkspace/PartagesWeb.API/blob/master/LICENSE.md</license>
// <author>Stéphane</author>
//-----------------------------------------------------------------------
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSwag.Annotations;
using PartagesWeb.API.Data;
using PartagesWeb.API.Dtos;
using PartagesWeb.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PartagesWeb.API.Controllers
{
    /// <summary>
    /// Controller pour authentification
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Auth", Description = "Controller pour authentification")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IMessagerieRepository _repoMessagerie;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        /// <summary>  
        /// Cette méthode est le constructeur 
        /// </summary> 
        /// <param name="repo"> Repository Auth</param>
        /// <param name="repoMessagerie">Messagerie de l'utilisateur</param>
        /// <param name="config"> Configuration</param>
        /// <param name="mapper">Automapp</param>
        public AuthController(IAuthRepository repo, IMessagerieRepository repoMessagerie, IConfiguration config, IMapper mapper)
        {
            _config = config;
            _repo = repo;
            _repoMessagerie = repoMessagerie;
            _mapper = mapper;
        }

        /// <summary>  
        /// Cette méthode permet de créer un compte
        /// </summary> 
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment [Authorize] est un utilisateur administrateur
        /// </remarks>
        /// <param name="userForRegisterDto"> DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost("register")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(void), Description = "Ok")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(void), Description = "L'utilisateur existe déjà")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("L'utilisateur existe déjà");

            var userToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        /// <summary>  
        /// Cette méthode permet de se connecter
        /// </summary> 
        /// <remarks>
        /// 8 Février : 
        /// Pour le moment [Authorize] est un utilisateur administrateur
        /// </remarks>
        /// <param name="userForLoginDto"> DTO de ce qui est envoyé depuis le frontend</param>
        [HttpPost("login")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Token")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, typeof(void), Description = "Pas autorisé à se connecter")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForWorkDto>(userFromRepo);

            // Messagerie non lu
            var messagesNonLu = await _repoMessagerie.GetCountMessagesNonLu(userFromRepo.Id);
            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user,
                messagesNonLu
            });
        }

        /// <summary>
        /// Permet de vérifier la deconnexion du token (pour vider Claim de asp.net)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(void), Description = "Echec de logout (ne devrait jamais sortir)")]
        public IActionResult Logout()
        {
            return Ok();
        }
    }
}
