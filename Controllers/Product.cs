using Ecommerce_Webservices.Common;
using Ecommerce_Webservices.DataBase;
using Ecommerce_Webservices.DataObject;
using Ecommerce_Webservices.RequestDateObject;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

                if (objProdReq.Name == "" || objProdReq.Dimensions == "" || objProdReq.Price <= 0 || objProdReq.Brand == "" || objProdReq.CategoryId == "" || objProdReq.Description == "" || objProdReq.Weight <= 0)
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
                if (objCategory is null)
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
        public async Task<IActionResult> ActiveInActiveProduct(string productId, [FromBody] bool isActive)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (productId == "")
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
                if (objProduct is null)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "Invalid product, no such product available";
                    return BadRequest(objRes);
                }

                // verifying same user product or not
                if (objProduct.UserId != userId)
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

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateReq objProdReq)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (objProdReq.Name == "" || objProdReq.ProductId == "" || objProdReq.Dimensions == "" || objProdReq.Price <= 0 || objProdReq.Brand == "" || objProdReq.CategoryId == "" || objProdReq.Description == "" || objProdReq.Weight <= 0)
                {
                    objRes.message = "Provide all required and valid values to update product";
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
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // checking Product exits or not
                var objProduct = await _databaseContext.Products.FindAsync(objProdReq.ProductId);
                if (objProduct is null)
                {
                    objRes.message = "Invalid product, no such product available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }
                // checking same user or not who created product
                if (objProduct.UserId != userId)
                {
                    objRes.message = "You are not allowed to update this product.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }
                // checking category exits or not
                if (objProdReq.CategoryId != objProduct.CategoryId)
                {
                    var objcategory = await _databaseContext.Categories.FindAsync(objProdReq.CategoryId);
                    if (objcategory is null)
                    {
                        objRes.message = "Invalid category, no such category available.";
                        objRes.isSuccess = false;
                        objRes.Data = "";
                        return BadRequest(objRes);
                    }
                }
                // updating product
                objProduct.Name = objProdReq.Name;
                objProduct.Description = objProdReq.Description;
                objProduct.Price = objProdReq.Price;
                objProduct.StockQuantity = objProdReq.StockQuantity;
                objProduct.Brand = objProdReq.Brand;
                objProduct.CategoryId = objProdReq.CategoryId;
                objProduct.UpdatedAt = DateTime.UtcNow;
                objProduct.Weight = objProdReq.Weight;
                objProduct.Dimensions = objProdReq.Dimensions;
                _databaseContext.Products.Update(objProduct);
                await _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully updated product";
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

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteProduct([FromQuery] string productId)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (productId == "")
                {
                    objRes.message = "Getting product id null";
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
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // checking Product exits or not
                var objProduct = await _databaseContext.Products.FindAsync(productId);
                if (objProduct is null)
                {
                    objRes.message = "Invalid product, no such product available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }
                // checking same user or not who created product
                if (objProduct.UserId != userId)
                {
                    objRes.message = "You are not allowed to delete this product.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }

                // checking is product is in order item with status pending
                
                var objOrderItem = await _databaseContext.OrderItems.Where(objOrd => objOrd.ProductId == productId).ToListAsync();
                bool isAllow = true;
                if(objOrderItem.Count > 0)
                {
                    for (global::System.Int32 i = 0; i < objOrderItem.Count; i++)
                    {
                        OrderItems OrderItem = objOrderItem[i];

                        var objOrder = await _databaseContext.Orders.FindAsync(OrderItem.OrderId);
                        if (objOrder != null)
                        {
                            if (objOrder.Status == enumOrderStatus.Processing || objOrder.Status == enumOrderStatus.Pending)
                            {
                                objRes.message = "You are not allowed to delete as this product is in pending or in processing status to order.";
                                objRes.isSuccess = false;
                                objRes.Data = "";
                                isAllow = false;
                                break;
                            }
                        }

                    }
                }


                if (!isAllow)
                {
                    return BadRequest(objRes);
                }

                // deleting reviews of this product
                var arrReview = await _databaseContext.Review.Where(objrev => objrev.ProductId == productId).ToListAsync();
                for (global::System.Int32 i = 0; i < arrReview.Count; i++)
                {
                    _databaseContext.Review.Remove(arrReview[i]);
                }

                // deleting product varients of this product
                var arrProductVarient = await _databaseContext.ProductVariant.Where(objrev => objrev.ProductId == productId).ToListAsync();
                for (global::System.Int32 i = 0; i < arrProductVarient.Count; i++)
                {
                    _databaseContext.ProductVariant.Remove(arrProductVarient[i]);
                }

                // deleting product images of this product
                var arrProductImgs = await _databaseContext.ProductImage.Where(objrev => objrev.ProductId == productId).ToListAsync();
                for (global::System.Int32 i = 0; i < arrProductImgs.Count; i++)
                {
                    _databaseContext.ProductImage.Remove(arrProductImgs[i]);
                }

                _databaseContext.Products.Remove(objProduct);

                await _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully deleted product";
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

        [HttpGet("getall-product")]
        public async Task<IActionResult> GetAllProduct()
        {
            try
            {
                UserResponse<List<DataObject.Product>> objRes = new UserResponse<List<DataObject.Product>>();

                if (!User.IsInRole("User"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = [];

                    return Unauthorized(objRes);
                }

                var arrProduct = await _databaseContext.Products.Where(objProd => objProd.IsActive).ToListAsync();

                
                //foreach (var objProduct in arrProduct)
                //{
                //    // getting product variants and imgs
                //    var productImgs = await _databaseContext.ProductImage.Where(objImg => objImg.ProductId == objProduct.Id).ToListAsync();
                //}
                objRes.Data = arrProduct!;
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

        [HttpGet("get-user-products")]
        public async Task<IActionResult> GetUserProducts()
        {
            try
            {
                UserResponse<List<DataObject.Product>> objRes = new UserResponse<List<DataObject.Product>>();

                if (!User.IsInRole("User"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = [];

                    return Unauthorized(objRes);
                }

                var userId = CommonVar.getUserIdfromClaims(User);
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = [];
                    return Unauthorized(objRes);
                }

                // getting all user products
                var products = await _databaseContext.Products.Where(objPro => objPro.UserId == userId).ToListAsync();

                objRes.isSuccess = true;
                objRes.Data = products;
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
    }
}
