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
        private readonly ICommentaireEvaluationService _comEvalService;
        public ProductController(IProductService productService, JwtTokenService tokenService, ICommentaireEvaluationService comEvalService)
        {
            _productService = productService;
            _jwtTokenService = tokenService;
            _comEvalService = comEvalService;
        }
        private bool isConnected()
        {
            return GetUserId() is not null;
        }
        
        [HttpGet()]
        public ActionResult GetItems()
        {
            return Ok(_productService.GetAll());
        }
        [HttpGet("{idProduct}")]
        public ActionResult GetItem(int idProduct)
        {
            Product product = _productService.GetOne(idProduct);
            if (isConnected())
            _productService.AddToRecommandation(GetUserId(), _productService.GetCategory(product.Category));
            return Ok(product);
        }
        
        [HttpGet("search/{search}")]
        public IActionResult SearchProduct(string search)
        {
            IEnumerable<Product> products = _productService.GetProductBySearch(search);
            if(isConnected())
            _productService.AddToRecommandation(GetUserId(), _productService.GetCategory(products.FirstOrDefault().Category));
            return Ok(products);
        }


        [HttpGet("category/{idCategory}")]
        public IActionResult GetByCategory(int idCategory)
        {
            if(isConnected())
            _productService.AddToRecommandation(GetUserId(), idCategory);
            return Ok(_productService.GetProductByCategory(idCategory));
        }

        [Authorize("IsVendeur")]
        [HttpPost("add-product")] 
        public IActionResult VendeurCreateProduct(ProductFormDTO newProduct)
        {
            int? id = GetUserId();

            return Ok(_productService.Createproduct(newProduct, id));
        }
        
        [Authorize("IsVendeur")]
        [HttpPut("modify-product/{idProduct}")]
        public IActionResult VendeurUpdate(ProductFormDTO product, int idProduct)
        {
            int? id = GetUserId();

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
            int? id = GetUserId();

            return Ok(_productService.DeleteProduct(idProduct, id));
        }

        [Authorize("IsVendeur")]
        [HttpGet("vendeur-product")]
        public IActionResult GetVendeurProduct()
        {
            int? id = GetUserId();

            return Ok(_productService.GetAllVendeur(id));
        }

        [Authorize("IsAdmin")]
        [HttpPost("admin-create-product")]
        public IActionResult AdminAddProduct(ProductFormDTO product)
        {
            int? id = GetUserId();

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

        [Authorize("IsConnected")]
        [HttpPatch("rating/")]
        public IActionResult RatingProduct(Evaluation rating)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_comEvalService.RatingProduct(rating, id));
        }
        
        [Authorize("IsConnected")]
        [HttpPost("commentaire/")]
        public IActionResult CommentProduct(Commentaires commentaire)
        {
            int? id = GetUserId();
            try
            {
                return Ok(_comEvalService.LeaveComment(commentaire, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [Authorize("IsConnected")]
        [HttpPatch("update-commentaire/")]
        public IActionResult UpdateCommentaire(Commentaires commentaire)
        {
            int? id = GetUserId();

            return Ok(_comEvalService.UpdateComment(commentaire, id));
        }

        [Authorize("IsConnected")]
        [HttpDelete("delete-commentaire/{idProduct}")]
        public IActionResult DeleteCommentaire(int idProduct)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);

            return Ok(_comEvalService.DeleteComment(idProduct, id));
        }

        [Authorize("IsConnected")]
        [HttpGet("get-recommandation")]
        public IActionResult GetRecommandation()
        {
            int? id = GetUserId();
            return Ok(_productService.GetRecommendedItems(id));
        }
        
        [Authorize("IsModo")]
        [HttpDelete("modo-delete-comment")]
        public IActionResult ModoDeleteCommentaire(Commentaires commentaire)
        {
            return Ok(_comEvalService.DeleteComment(commentaire.Id, commentaire.Id_User));
        }
        
        [HttpGet("get-comment")]
        public IActionResult GetComments()
        {
            return Ok(_comEvalService.GetAll("Commentaires"));
        }
        
        [HttpGet("get-comment/{idProduct}")]
        public IActionResult GetCommentsByProduct(int idProduct)
        {
            return Ok(_comEvalService.GetCommentsByProduct(idProduct));
        }
        private int? GetUserId()
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            int? id = _jwtTokenService.GetUserIdFromToken(token);
            return id;
        }
    }
}
