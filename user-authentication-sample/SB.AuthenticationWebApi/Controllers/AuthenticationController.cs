using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SB.AuthenticationDomain.Service.Interface;
using SB.Core.Filters;
using SB.Core.UnitOfWork.Interface;
using SB.Model.DTO;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationWebApi.Controllers
{
    [ExceptionHandler]
    [Route("authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AuthenticationController(IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [Transaction]
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto User)
        {
            User user = _authenticationService.Login(User.Username, User.Password);

            if (user == null)
            {
                return BadRequest("Username or password is incorrect");
            }                

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [Transaction]
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            try
            {
                // save 
                _authenticationService.Register(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _authenticationService.GetUsers();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var user = _authenticationService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [Transaction]
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try
            {
                // save 
                _authenticationService.UpdateUser(user, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [Transaction]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _authenticationService.DeleteUser(id);
            return Ok();
        }
    }
}
