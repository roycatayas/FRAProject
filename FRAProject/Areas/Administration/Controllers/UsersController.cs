using System;
using System.Linq;
using System.Threading.Tasks;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.IdentityProvider.Entities;
using FRA.Web.Areas.Administration.Models;
using FRA.Web.Models.DataTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FRA.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        public UsersController(UserManager<ApplicationUser> userManager, IUserRepository userRepository, RoleManager<ApplicationRole> roleManager, IUserRoleRepository userRoleRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _roleManager = roleManager;
            _userRoleRepository = userRoleRepository;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }       

        [HttpPost]        
        public async Task<ActionResult> GetUserList()
        {
            UserListView model = new UserListView();            

            User[] users = (await _userRepository.GetUsersAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();
            model.ListUser = users;

            return PartialView("GetUser", model);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult AddUser()
        {
            AddUserViewModel model = new AddUserViewModel();           
            model.ApplicationRoles = _roleManager.Roles.ToDictionary(items => items.Id, items => items.Name);
            return PartialView("AddUser", model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> AddUser([FromBody]AddUserViewModel model)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                PasswordHash = model.Password,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName + " " + model.LastName,
                PhoneNumber = model.PhoneNumber,
                LockoutEnabled = false,
                EmailConfirmed = true,
                Organization = model.Orginization,
                RegistrationDate = DateTime.Now
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId.ToString());
                if (applicationRole != null)
                {
                    IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                    if (roleResult.Succeeded)
                    {
                        return Json("success");
                    }
                }
            }

            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            EditUserViewModel editUserViewModel = new EditUserViewModel();

            if (user != null)
            {                
                editUserViewModel.LastName = user.LastName;
                editUserViewModel.Email = user.Email;
                editUserViewModel.FullName = user.FullName;
                editUserViewModel.Orginization = user.Organization;
                editUserViewModel.EmailConfirmed = user.EmailConfirmed;
                editUserViewModel.LockoutEnabled = user.LockoutEnabled;
                editUserViewModel.PhoneNumber = user.PhoneNumber;
                editUserViewModel.FirstName = user.FirstName;
                editUserViewModel.Address = user.Address;
                editUserViewModel.Id = user.Id;
                editUserViewModel.ApplicationRoles = _roleManager.Roles.ToDictionary(items => items.Id, items => items.Name);
                editUserViewModel.ApplicationRoleId = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
            }
            else editUserViewModel = null;
            

            return PartialView("EditUser", editUserViewModel);

        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> EditUser([FromBody]EditUserViewModel model)
        {            
            ApplicationUser user = await _userManager.FindByIdAsync(model.Id.ToString());            

            user.Address = model.Address;
            user.EmailConfirmed = model.EmailConfirmed;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.LockoutEnabled = model.LockoutEnabled;
            user.PhoneNumber = model.PhoneNumber;
            user.LockoutEnabled = model.LockoutEnabled;
            string existingRole = _userManager.GetRolesAsync(user).Result.Single();
            Guid existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                if (existingRoleId != model.ApplicationRoleId)
                {
                    //IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                    OperationResult userRoleResult = await _userRoleRepository.RemoveFromRoleAsync(user, existingRoleId);
                    if (userRoleResult.Succeeded)
                    {
                        ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId.ToString());
                        if (applicationRole != null)
                        {
                            IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                            if (newRoleResult.Succeeded)
                            {
                                return Json("success");
                            }
                        }
                    }                    
                }
            }

            if (!result.Succeeded)
            {
                ViewBag.Response = new EditUserResponseViewModel
                {
                    Succeeded = false,
                    Description = "The was a problem updating the user. Please try again."
                };

                return Json("Failed");
            }

            ViewBag.Response = new EditUserResponseViewModel
            {
                Succeeded = true,
                Description = "User information were updated successfully."
            };

            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            DeleteUserViewModel deleteUserViewModel = new DeleteUserViewModel();
            
            if (!string.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    deleteUserViewModel.FullName = applicationUser.FullName;
                    deleteUserViewModel.Id = applicationUser.Id;
                }
            }
            return PartialView("DeleteUser", deleteUserViewModel);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteUser([FromBody]DeleteUserViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Id.ToString()))
            {
                ApplicationUser applicationUser = await _userManager.FindByIdAsync(model.Id.ToString());
                if (applicationUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return Json("success");
                    }
                }
            }
            return Json("success");
        }
    }
}