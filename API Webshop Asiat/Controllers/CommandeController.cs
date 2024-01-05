using API_Webshop_Asiat.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop_DAL.Interfaces;
using Webshop_DAL.Models;

namespace API_Webshop_Asiat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandeController : ControllerBase
    {
        private readonly ICommandeService _commandeService;
        private readonly JwtTokenService _tokenService;
        public CommandeController(ICommandeService commandeService, JwtTokenService tokenService)
        {
            _commandeService = commandeService;
            _tokenService = tokenService;
        }
        [Authorize("IsAdmin")]
        [HttpGet("admin-get-all")]
        public IActionResult GetAll() 
        {
            return Ok(_commandeService.GetAll("Commande"));
        }

        [Authorize("IsConnected")]
        [HttpGet()]
        public IActionResult GetAllByUser()
        {
            return Ok(_commandeService.GetCommands(GetUserId()));
        }

        [Authorize("IsConnected")]
        [HttpGet("{id}")]
        public IActionResult GetDetails(int id) 
        {
            return Ok(_commandeService.GetById("command", id));
        }

        [Authorize("IsConnected")]
        [HttpPost()]
        public IActionResult BuyCommand(List<Product> product)
        {
            return Ok(_commandeService.BuyCommand(product));
        }

        [Authorize("IsConnected")]
        [HttpDelete()]
        public IActionResult DeleteCommand(int id)
        {
            _commandeService.Delete("Commande", id);
            return Ok();   
        }
        private int? GetUserId()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _tokenService.GetUserIdFromToken(token);
            return id;
        }
    }
}
