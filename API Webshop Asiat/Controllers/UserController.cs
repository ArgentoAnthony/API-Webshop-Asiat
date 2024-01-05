using API_Webshop_Asiat.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Webshop_DAL.Exceptions;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace API_Webshop_Asiat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenManager _tokenManager;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(IUserService userService, TokenManager tokenmanager, JwtTokenService _jWtTokenService)
        {
            _userService = userService;
            _tokenManager = tokenmanager;
            _jwtTokenService = _jWtTokenService;
        }

        [HttpPost("login/")]
        public IActionResult Login(LoginUserFormDTO loginForm)
        {
            User user = _userService.Login(loginForm);
            string json = JsonConvert.SerializeObject(
                    new
                    {
                        token = _tokenManager.GenerateToken(
                            user
                        )
                    }
                ); ;

            return Ok(json);
        }
        [HttpPost("register/")]
        public IActionResult CreateAccount(CreateUserDTO user)
        {
            _userService.Register(user);
            return Ok();
        }
        [Authorize("IsConnected")]
        [HttpPost("update/")]
        public IActionResult UpdateAccount(AdminUpdateFormDTO newUserInfo)
        {
            int? id = GetUserId();

            if (id != null)
            {
                return Ok(_userService.Update(newUserInfo, (int)id));
            }
            else
            { return BadRequest("id not found in token claims."); }
        }

        [HttpPost("create-vendeur")]
        public IActionResult CreateVendeur(CreateVendeurDTO vendeur) 
        {
            try
            {
                return Ok(_userService.CreateVendeur(vendeur));
            }
            catch(Exception ex) 
            { 
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPut("update-vendeur")]
        [Authorize("IsVendeur")]
        public IActionResult UpdateVendeurAccount(VendeurUpdateFormDTO newUserInfo)
        {
            int? id = GetUserId();

            if (id != null)
            {
                return Ok(_userService.Update(newUserInfo, (int)id));
            }
            else
            { return BadRequest("id not found in token claims."); }
        }

        [Authorize("IsConnected")]
        [HttpDelete("delete/")]
        public IActionResult DeleteAccount()
        {
            int? id = GetUserId();

            if (id != null)
            {
                _userService.Delete("Users",(int)id);
                return Ok();
            }
            else
            { return BadRequest("id not found in token claims."); }
        }
        [Authorize("IsConnected")]
        [HttpGet("details/")]
        public IActionResult DetailsAccount()
        {
            int? id = GetUserId(); ;

            if (id != null)
                try
                {
                    return Ok(_userService.GetById("Users", (int)id));
                }
                catch (IdNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            else
                return BadRequest("ID not found in token claims.");

        }
        [Authorize("IsAdmin")]
        [HttpPut("admin-update/")]
        public IActionResult UpdateUser(AdminUpdateFormDTO adminUpdate) 
        {
            return Ok(_userService.Update(adminUpdate, adminUpdate.Id));
        }
        [Authorize("IsAdmin")]
        [HttpDelete("admin-delete/")]
        public IActionResult DeleteUser(int id)
        {
            _userService.Delete("Users", id);
            return Ok();
        }
        [Authorize("IsAdmin")]
        [HttpGet("get-user/")]
        public IActionResult GetUser()
        {
            
            return Ok(_userService.GetAll("Users"));
        }
        [Authorize("IsAdmin")]
        [HttpGet("details-user/{id}")]
        public IActionResult DetailsUser(int id)
        {
            return Ok(_userService.GetById("Users", id));
        }
        private int? GetUserId()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);
            return id;
        }
    }
}
