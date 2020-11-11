using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using MyBatiment.API.Resources;
using MyBatiment.API.Validation;
using MyBatiment.Core.Models;
using MyBatiment.Core.Services;

namespace MyBatiment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _serviceUser;
        private readonly IMapper _mapper;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;

        public UserController(IUserService serviceUser, IMapper mapper,
            Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _serviceUser = serviceUser;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UserResource userResource)
        {
            var user = await _serviceUser.Authenticate(userResource.Username, userResource.Password);
            if (user == null) return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                 }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(SaveUserResource userResource)
        {
            // validation
            var validation = new SaveUserResourceValidation();
            var validationResult = await validation.ValidateAsync(userResource);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var user = _mapper.Map<SaveUserResource, User>(userResource);
            // mappage
            var userSave = await _serviceUser.Create(user, userResource.Password);

            //send tocken 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("AppSettings:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                 }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceItemResource>> GetUserItem(Guid id)
        {
            try
            {
                var user = await _serviceUser.GetById(id);

                if (user == null)
                    return NotFound();

                //--> Mpper permet de convertir la liste des ServiceItem vers userResource
                var userResource = _mapper.Map<User, UserResource>(user);

                return Ok(userResource);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updateUser")]
        //[Authorize]
        public async Task<ActionResult<SaveUserResource>> UpdateUser(SaveUserResource saveUserResource)
        {
            /// validation
            var validation = new SaveUserResourceValidation();
            var validationResult = await validation.ValidateAsync(saveUserResource);

            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            // si la ServiceItem existe depuis le id
            var userUpdate = await _serviceUser.GetById(saveUserResource.Id);
            if (userUpdate == null) return BadRequest("User n'existe pas ");

            string password = saveUserResource.Password;

            var user = _mapper.Map<SaveUserResource, User>(saveUserResource);
            // mappage
            _serviceUser.Update(user, saveUserResource.Password);

            var userUpdateNew = await _serviceUser.GetById(user.Id);
            var userResourceUpdate = _mapper.Map<User, SaveUserResource>(userUpdateNew);

            return Ok(userResourceUpdate);
        }
    }
}
