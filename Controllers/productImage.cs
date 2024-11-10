using Ecommerce_Webservices.Common;
using Ecommerce_Webservices.DataBase;
using Ecommerce_Webservices.DataObject;
using Ecommerce_Webservices.RequestDateObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImage : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<User> _userManager;

        public ProductImage(DatabaseContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProductImage([FromBody] DataObject.ProductImage objImg)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                var userId = CommonVar.getUserIdfromClaims(User);
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                if(objImg.Url == "" || objImg.ProductId == "" || objImg.AltText == "")
                {
                    objRes.message = "Provide all required and valid data to add image.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                // checking product exits or not
                var objProduct = await _dbContext.Products.FindAsync(objImg.ProductId);
                if(objProduct is null)
                {
                    objRes.message = "Invalid product info, no such product available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                DataObject.ProductImage objNewImg = new DataObject.ProductImage();
                objNewImg = objImg;
                objNewImg.Id = Guid.NewGuid().ToString();

                await _dbContext.ProductImage.AddAsync(objNewImg);
                await _dbContext.SaveChangesAsync();

                objRes.isSuccess = true;
                objRes.message = "";
                objRes.Data = "Succeesfull added images for product " + objProduct.Name;
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

        [HttpPost("add-multiple")]
        public async Task<IActionResult> AddMultipleProductImages([FromBody] MultipleProdImgReq objImg)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                var userId = CommonVar.getUserIdfromClaims(User);
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // checking product exits or not
                var objProduct = await _dbContext.Products.FindAsync(objImg.ProductId);
                if (objProduct is null)
                {
                    objRes.message = "Invalid product info, no such product available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                DataObject.ProductImage objNewImg = new DataObject.ProductImage();
                
                
                foreach (var item in objImg.arrImg)
                {
                    objNewImg = new DataObject.ProductImage();
                    objNewImg.Id = Guid.NewGuid().ToString();
                    objNewImg.ProductId = objImg.ProductId;
                    objNewImg.Url = item.Url;
                    objNewImg.AltText = item.AltText;
                    objNewImg.IsPrimary = item.IsPrimary;
                    await _dbContext.ProductImage.AddAsync(objNewImg);
                }

                await _dbContext.SaveChangesAsync();

                objRes.Data = "Successfully added all images for product " + objProduct.Name;
                objRes.message = "";
                objRes.isSuccess = true;
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
        public async Task<IActionResult> DeleteProductImage([FromQuery] string imgId)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                var userId = CommonVar.getUserIdfromClaims(User);
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // checking img exits or not
                var objImg = await _dbContext.ProductImage.FindAsync(imgId);
                if (objImg is null)
                {
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    objRes.message = "Invalid Img info, no such image found.";
                    return BadRequest(objRes);
                }
                // deleting img
                _dbContext.ProductImage.Remove(objImg);
                await _dbContext.SaveChangesAsync();

                objRes.isSuccess = true;
                objRes.Data = "Successfully delete image";
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

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProductImage([FromBody] DataObject.ProductImage objImg)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoAdminPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                var userId = CommonVar.getUserIdfromClaims(User);
                // checking user exits or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                if (objImg.Url == "" || objImg.ProductId == "" || objImg.AltText == "")
                {
                    objRes.message = "Provide all required and valid data to add image.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }

                // checking product exits or not
                var objProduct = await _dbContext.Products.FindAsync(objImg.ProductId);
                if (objProduct is null)
                {
                    objRes.message = "Invalid product info, no such product available.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return Unauthorized(objRes);
                }
                // checking Img exits or not
                var ImageInfo = await _dbContext.ProductImage.FindAsync(objImg.Id);
                if (ImageInfo is null)
                {
                    objRes.isSuccess = false;
                    objRes.message = "Invalid Info, no such image found.";
                    objRes.Data = "";
                    return BadRequest(objRes);
                }
                ImageInfo.AltText = objImg.AltText;
                ImageInfo.Url = objImg.Url;
                ImageInfo.IsPrimary = objImg.IsPrimary;

                 _dbContext.ProductImage.Update(ImageInfo);
                await _dbContext.SaveChangesAsync();

                objRes.isSuccess = true;
                objRes.message = "";
                objRes.Data = "Succeesfull updated image";
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
