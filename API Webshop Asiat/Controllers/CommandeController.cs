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
            return Ok(_commandeService.GetCommands());
        }

        [Authorize("IsConnected")]
        [HttpGet()]
        public IActionResult GetAllByUser()
        {
            return Ok(_commandeService.GetCommands(GetUserId()));
        }

        [Authorize("IsConnected")]
        [HttpGet("{commandNumber}")]
        public IActionResult GetDetails(Guid commandNumber) 
        {
            return Ok(_commandeService.GetCommandByCommandNumber(commandNumber));
        }

        [Authorize("IsConnected")]
        [HttpPost()]
        public IActionResult BuyCommand(List<Product> product)
        {
            try
            {
                _commandeService.BuyCommand(product, GetUserId());
                return Ok("Commande effectuée");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize("IsConnected")]
        [HttpDelete("{commandNumber}")]
        public IActionResult DeleteCommand(Guid commandNumber)
        {
            _commandeService.DeleteCommande(commandNumber);
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
