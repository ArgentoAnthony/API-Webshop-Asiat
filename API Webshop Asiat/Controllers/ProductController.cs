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
        [HttpGet()]
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

            return Ok(_productService.Createproduct(newProduct, id));
        }
        [Authorize("IsVendeur")]
        [HttpPut("modify-product/{idProduct}")]
        public IActionResult VendeurUpdate(ProductFormDTO product, int idProduct)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            try
            {
                return Ok(_productService.UpdateProduct(product, idProduct, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize("IsVendeur")]
        [HttpDelete("delete-product/{idProduct}")]
        public IActionResult DeleteProduct(int idProduct)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_productService.DeleteProduct(idProduct, id));
        }

        [Authorize("IsVendeur")]
        [HttpGet("vendeur-product")]
        public IActionResult GetVendeurProduct()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_productService.GetAllVendeur(id));
        }

        [Authorize("IsAdmin")]
        [HttpPost("admin-create-product")]
        public IActionResult AdminAddProduct(ProductFormDTO product)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_productService.Createproduct(product, id));
        }
        [Authorize("IsAdmin")]
        [HttpDelete("admin-delete-product/{idProduct}")]
        public IActionResult AdminDeleteProduct(int idProduct)
        {
            return Ok(_productService.DeleteProduct(idProduct));
        }
        [Authorize("IsAdmin")]
        [HttpPut("admin-update-product/{idProduct}")]
        public IActionResult AdminUpdateProduct(ProductFormDTO product, int idProduct)
        {
            return Ok(_productService.UpdateProduct(product, idProduct));
        }
    }
}
