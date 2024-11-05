using Ecommerce_Webservices.Common;
using Ecommerce_Webservices.DataBase;
using Ecommerce_Webservices.DataObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Webservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Category : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<User> _userManager;

        public Category(DatabaseContext databaseContext, UserManager<User> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        [HttpPost("add")]
       public async Task<IActionResult> AddCategory([FromBody] Categories categories)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (categories.UserId == "" || categories.Name== "" || categories.Description == "")
                {
                    objRes.message = "Provide all values to add category";
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
                    return Unauthorized(objRes) ;
                }

                // checking categorie exits or not of that user
                var category = await _databaseContext.Categories
                    .Where(obj => obj.UserId == userId && obj.Name == categories.Name)
                    .FirstOrDefaultAsync();         

                if(category != null)
                {
                    objRes.message = "This category " + categories.Name + " Already exits";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }
                Categories objCategory = new Categories();
                objCategory.Name = categories.Name;
                objCategory.Description = categories.Description;
                objCategory.UserId = userId;
                objCategory.Id = Guid.NewGuid().ToString();

                // adding category
                await _databaseContext.Categories.AddAsync(objCategory);
                await _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully added category " + categories.Name;
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

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                UserResponse<List<Categories>> objRes = new UserResponse<List<Categories>>();

               
                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = [];

                    return Unauthorized(objRes);
                }
                string userId = CommonVar.getUserIdfromClaims(User);
                // checking valid user or not
                var objUser = await _userManager.FindByIdAsync(userId);
                if (objUser == null)
                {
                    objRes.message = CommonVar.InvalidUser;
                    objRes.isSuccess = false;
                    objRes.Data = [];
                    return Unauthorized(objRes);
                }

                // getting list 
                var listcategories = await _databaseContext.Categories.Where(obj => obj.UserId == userId).ToListAsync();

                objRes.Data = listcategories;
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

        [HttpPost("update")]
        public async Task<IActionResult> UpdateCategory([FromBody] Categories categories)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (categories.UserId == "" || categories.Name == "" || categories.Description == "" || categories.Id == "")
                {
                    objRes.message = "Provide all values to update category";
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

                // checking categorie exits or not of that user
                var category = await _databaseContext.Categories
                    .Where(obj => obj.UserId == userId && obj.Id == categories.Id)
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    objRes.message = "This category does not exits.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }

                category.Name = categories.Name;
                category.Description = categories.Description;

                // updating category
                _databaseContext.Categories.Update(category);
                await _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully updated category " + categories.Name;
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
        public async Task<IActionResult> DeleteCategory([FromQuery] string Id)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (Id == "" )
                {
                    objRes.message = "Provide all values to delete category";
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

                // checking categorie exits or not of that user
                var category = await _databaseContext.Categories
                    .Where(obj => obj.UserId == userId && obj.Id == Id)
                    .FirstOrDefaultAsync();

                if (category == null)
                {
                    objRes.message = "There is no such category to delete.";
                    objRes.isSuccess = false;
                    objRes.Data = "";
                    return BadRequest(objRes);
                }

                 _databaseContext.Categories.Remove(category);
                await  _databaseContext.SaveChangesAsync();

                objRes.Data = "Successfully Deleted category " + category.Name;
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
    }
}
