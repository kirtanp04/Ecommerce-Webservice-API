using Ecommerce_Webservices.Common;
using Ecommerce_Webservices.DataBase;
using Ecommerce_Webservices.DataObject;
using Ecommerce_Webservices.RequestDateObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Product : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DatabaseContext _databaseContext;

        public Product(UserManager<User> userManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductReq objProdReq)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if(objProdReq.Name == ""  || objProdReq.Dimensions == "" || objProdReq.Price <=0 ||  objProdReq.Brand == "" || objProdReq.CategoryId == "" || objProdReq.Description == "" || objProdReq.Weight <=0)
                {
                    objRes.message = "Provide all required and valid values to add product";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return BadRequest(objRes);
                }

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }
                string userId = CommonVar.getUserIdfromClaims(User);
                // checking valid user or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // check category valid or not
                var objCategory = await _databaseContext.Categories.FindAsync(objProdReq.CategoryId);
                if(objCategory is null)
                {
                    objRes.message = "Invalid product category, no such category available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // adding product
               DataObject.Product objProduct = new DataObject.Product();
                objProduct.Id = Guid.NewGuid().ToString();
                objProduct.Name = objProdReq.Name;
                objProduct.Description = objProdReq.Description;
                objProduct.Price = objProdReq.Price;
                objProduct.StockQuantity = objProdReq.StockQuantity;
                objProduct.Brand = objProdReq.Brand;
                objProduct.Rating = 0;
                objProduct.CategoryId = objProdReq.CategoryId;
                objProduct.CreatedAt = DateTime.UtcNow;
                objProduct.UpdatedAt = DateTime.UtcNow;
                objProduct.TotalReviews = 0;
                objProduct.Weight = objProdReq.Weight;
                objProduct.Dimensions = objProdReq.Dimensions;
                objProduct.IsActive = false;
                objProduct.UserId = userId;

                await _databaseContext.Products.AddAsync(objProduct);
                await _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully added new product";
                objRes.isSuccess = true;
                objRes.message = "";
                return Ok(objRes);
            }
            catch (Exception ex)
            {

                UserResponse<string> objRes = new UserResponse<string>
                {
                    isSuccess = false,
                    message = ex.Message,
                    Data = "",
                };
                return BadRequest(objRes);
            }
        }

        [HttpPost("active-inactive/{productId}")]
        public async Task<IActionResult> ActiveInActiveProduct(string productId,[FromBody] bool isActive)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (productId == ""  )
                {
                    objRes.message = "Provide all required values to active / In-active product";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return BadRequest(objRes);
                }

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                
                string userId = CommonVar.getUserIdfromClaims(User);
                // checking valid user or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                // checking product exits or not
                var objProduct = await _databaseContext.Products.FindAsync(productId);
                if(objProduct is null)
                {
                    objRes.Data = "";
                    objRes.isSuccess=false;
                    objRes.message = "Invalid product, no such product available";
                    return BadRequest(objRes);
                }

                // verifying same user product or not
                if(objProduct.UserId != userId)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "You are not allowed to do any update to this product";
                    return Unauthorized(objRes);
                }
                objProduct.IsActive = isActive;
                 _databaseContext.Products.Update(objProduct);
                await _databaseContext.SaveChangesAsync();

                objRes.message = "";
                objRes.isSuccess = true;
                if (isActive)
                {
                    objRes.Data = "Success, your product is active";
                }
                else
                {
                    objRes.Data = "Success, your product is Inactive";
                }
               
                return Ok(objRes);
                
            }
            catch (Exception ex)
            {

                UserResponse<string> objRes = new UserResponse<string>
                {
                    isSuccess = false,
                    message = ex.Message,
                    Data = "",
                };
                return BadRequest(objRes);
            }
        }
    }
}
