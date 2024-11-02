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
    public class Role : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<RoleInfo> _roleManager;
        private readonly DatabaseContext _databaseContext;

        public Role(UserManager<User> userManager, RoleManager<RoleInfo> roleManager, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _databaseContext = databaseContext;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] RoleReq objRolereq)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (objRolereq.userEmail == "" || objRolereq.roleName == "")
                {
                    objRes.message = "Provide all values to add role";
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

                var user = await _userManager.FindByEmailAsync(objRolereq.userEmail);

                if (user is null)
                {
                    objRes.message = "Your email Id does not exist.";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return NotFound(objRes);
                }

                RoleInfo objRole = new RoleInfo
                {
                    Name = objRolereq.roleName,
                    CreatedBy = user.Id,
                    CreatedAt = DateTime.UtcNow,
                };

                var role = await _roleManager.RoleExistsAsync(objRole.Name);

                if (role)
                {
                    objRes.message = "Role already exits add another role.";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return BadRequest(objRes);
                }

                IdentityResult isAdded = await _roleManager.CreateAsync(objRole);

                if (!isAdded.Succeeded)
                {
                    objRes.message = isAdded.Errors.ToString()!;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return BadRequest(objRes);
                }


                objRes.message = "";
                objRes.isSuccess = true;
                objRes.Data = "successfully added role" + objRolereq.roleName;

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

        [HttpPost("assignee-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleReq objAssignRoleReq)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (objAssignRoleReq.AssignByEmail == "" || objAssignRoleReq.AssignToEmail == "" || objAssignRoleReq.RoleName == "")
                {
                    objRes.message = "Provide all values to assign role";
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



                // assign to user exits or not
                var AssigntoUser = await _userManager.FindByEmailAsync(objAssignRoleReq.AssignToEmail);
                if (AssigntoUser is null)
                {
                    objRes.message = objAssignRoleReq.AssignToEmail + "user with this Id doesnot exist";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return NotFound(objRes);
                }

                // assign by user exits or not
                var AssignByUser = await _userManager.FindByEmailAsync(objAssignRoleReq.AssignByEmail);
                if (AssigntoUser is null)
                {
                    objRes.message = "Your email Id does not exist.";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return NotFound(objRes);
                }
                // Role exist or not
                var isRoleexits = await _roleManager.FindByNameAsync(objAssignRoleReq.RoleName);
                if (isRoleexits is null)
                {
                    objRes.message = "No such role exist, first create new role";
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return NotFound(objRes);
                }

                // already assigned or not
                var roles = await _userManager.GetRolesAsync(AssigntoUser);

                if (roles.Contains(isRoleexits.Name!))
                {
                    objRes.message = "Role already assigned to " + AssigntoUser.UserName;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return BadRequest(objRes);
                }


                RoleAssignInfo objRoleAssign = new RoleAssignInfo();
                objRoleAssign.AssignedAt = DateTime.UtcNow;
                objRoleAssign.UserId = AssigntoUser.Id;
                objRoleAssign.AssignedBy = AssignByUser!.Email!;
                objRoleAssign.RoleId = isRoleexits.Id;

                await _databaseContext.UserRoles.AddAsync(objRoleAssign);
                await _databaseContext.SaveChangesAsync();

                objRes.message = "";
                objRes.isSuccess = true;
                objRes.Data = "successfully assigned role to " + AssigntoUser.UserName;

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

        [HttpPost("update-role")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleReq objRoleReq)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();

                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                // checking role exits or not
                var objRoleInfo = await _roleManager.FindByIdAsync(objRoleReq.RoleId);

                if (objRoleInfo is null)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "There is no shuch role that has been created";

                    return BadRequest(objRes);
                }

                // checking role created by and updating by is same user or not

                if (objRoleInfo.CreatedBy != objRoleReq.UpdateByUser)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "You are not allowed to update this role as this role had not created by you";

                    return BadRequest(objRes);
                }

                // updating role
                objRoleInfo.Name = objRoleReq.NewRoleName;

                // Saving the updated role
                var isUpdated = await _roleManager.UpdateAsync(objRoleInfo);
                if (!isUpdated.Succeeded)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = string.Join(", ", isUpdated.Errors.Select(e => e.Description));

                    return BadRequest(objRes);
                }


                objRes.Data = "Successfully updated role";
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
                    Data = ""
                };
                return BadRequest(objRes);

            }

        }

        [HttpGet("getcreated-roles/{userEmail}")]
        public async Task<IActionResult> GetAllRoles(string userEmail)
        {
            try
            {
                UserResponse<List<RoleInfo>> objRes = new UserResponse<List<RoleInfo>>();
                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = [];

                    return Unauthorized(objRes);
                }


                // check valid user or not
                var objUser = await _userManager.FindByEmailAsync(userEmail);
                if (objUser is null)
                {
                    objRes.message = "No such user found by email " + userEmail;
                    objRes.isSuccess = false;
                    objRes.Data = [];

                    return BadRequest(objRes);
                }

                // getting all role created by this user
                var roles = await _roleManager.Roles.Where(objRole => objRole.CreatedBy == objUser.Id).ToListAsync();
                objRes.Data = roles;
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
                    Data = ""
                };
                return BadRequest(objRes);
            }
        }

        [HttpDelete("delete-role/{roleName}/{userEmail}")]
        public async Task<IActionResult> DeleteRole(string roleName, string userEmail)
        {
            try
            {
                UserResponse<string> objRes = new UserResponse<string>();
                if (!User.IsInRole("Admin"))
                {
                    objRes.message = CommonVar.NoPermission;
                    objRes.isSuccess = false;
                    objRes.Data = "";

                    return Unauthorized(objRes);
                }

                // checking user exits or not
                var objUser = await _userManager.FindByEmailAsync(userEmail);
                if (objUser is null)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "Invalid email id, no such user found";
                    return NotFound(objRes);
                }
                // checking role exits or not
                var roles = await _roleManager.FindByNameAsync(roleName);
                if (roles is null)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "Invalid role, there is no such role to delete";
                    return NotFound(objRes);
                }
                // checking uere has created or not 
                if (roles.CreatedBy != objUser.Id!)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = "You are not allowed to delete this role as you haven't created it.";
                    return Unauthorized(objRes);
                }

                // deleting from role db , this will also delete from assigne.
                var isDelete = await _roleManager.DeleteAsync(roles);
                if (!isDelete.Succeeded)
                {
                    objRes.Data = "";
                    objRes.isSuccess = false;
                    objRes.message = isDelete.Errors.ToString()!;
                    return BadRequest(objRes);
                }

                objRes.Data = "Successfully deleted Role " + roleName;
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
                    Data = ""
                };
                return BadRequest(objRes);
            }
        }

        [HttpGet("getAssigned-roles/{userEmail}")]
        public async Task<IActionResult> GetAssignedRoles(string userEmail)
        {
            try
            {
                UserResponse<IList<string>> objRes = new UserResponse<IList<string>>();

                // checking user exits or not
                var objUser = await _userManager.FindByEmailAsync(userEmail);
                if (objUser is null)
                {
                    objRes.Data = [];
                    objRes.isSuccess = false;
                    objRes.message = "Invalid email id, no such user found";
                    return NotFound(objRes);
                }
                var roleAssignInfo = await _userManager.GetRolesAsync(objUser);

               

                objRes.Data = roleAssignInfo;
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
                    Data = ""
                };
                return BadRequest(objRes);
            }
        }
    }
}
