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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly JwtTokenService _jwtTokenService;
        public ProductController(IProductService productService, JwtTokenService tokenService)
        {
            _productService = productService;
            _jwtTokenService = tokenService;
        }
        [HttpGet("/")]
        public ActionResult GetItems()
        {
            return Ok(_productService.GetAll());
        }
        [HttpGet("search/{search}")]
        public IActionResult SearchProduct(string search)
        {
            return Ok(_productService.GetProductBySearch(search));
        }
        [HttpGet("category/{id}")]
        public IActionResult GetByCategory(int id)
        {
            return Ok(_productService.GetProductByCategory(id));
        }

        [Authorize("IsVendeur")]
        [HttpPost("add-product")] 
        public IActionResult VendeurCreateProduct(ProductFormDTO newProduct)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_productService.VendeurCreateproduct(newProduct, id));
        }
        [Authorize("IsVendeur")]
        [HttpPost("modify-product")]
        public IActionResult VendeurUpdate(ProductFormDTO product)
        {
            return Ok(_productService.VendeurUpdateProduct(product));
        }
        [Authorize("IsVendeur")]
        [HttpDelete("delete-product")]
        public IActionResult DeleteProduct(int id)
        {
            return Ok(_productService.VendeurDeleteProduct(id));
        }
    }
}
