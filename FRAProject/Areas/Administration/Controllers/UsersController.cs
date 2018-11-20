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

namespace FRA.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UsersController(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        [ActionName("get-all")]
        public ViewResult GetAllUsers()
        {
            return View();
        }

        [HttpPost]
        [ActionName("get-all-json")]
        public async Task<JsonResult> GetAllUsersJson(DataTable dataTable)
        {
            int pageNumber = dataTable.Start / dataTable.Length + 1;
            Order order = dataTable.Order.FirstOrDefault();

            SortDirection sortDirection = order != null
                ? (order.Direction == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                : SortDirection.Ascending;

            User[] users = (await _userRepository.GetUsersAsync(pageNumber, dataTable.Length, order?.Column ?? 0, sortDirection,
                string.IsNullOrEmpty(dataTable.Search.Value) ? string.Empty : dataTable.Search.Value)).ToArray();

            int totalNumberOfUsers = _userRepository.GetTotalNumberOfUsers();            

            return Json(new DataTableResponse<User>
            {
                Data = users,
                RecordsFiltered = string.IsNullOrEmpty(dataTable.Search.Value) ? totalNumberOfUsers : users.Length,
                Draw = dataTable.Draw,
                RecordsTotal = totalNumberOfUsers
            });
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            AddUserViewModel model = new AddUserViewModel();
            return PartialView("AddUser", model);
        }

        [HttpPost]
        public async Task<ViewResult> AddUser(AddUserViewModel model)
        {

            ApplicationUser user = new ApplicationUser();

            user.UserName = model.Email;
            user.Email = model.Email;
            user.Address = model.Address;   
            user.PasswordHash = model.Password;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.FullName = model.FirstName + " " + model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.LockoutEnabled = false;
            user.EmailConfirmed = true;
            user.Organization = model.Orginization;
            user.RegistrationDate = DateTime.Now;
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            return View("Index");
        }

        [HttpGet]
        [ActionName("EditUser")]
        public async Task<PartialViewResult> EditUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            EditUserViewModel editUserViewModel = user != null ? new EditUserViewModel
                                                    {
                                                        LastName = user.LastName,
                                                        Email = user.Email,
                                                        FullName = user.FullName,
                                                        Orginization = user.Organization,
                                                        EmailConfirmed = user.EmailConfirmed,
                                                        LockoutEnabled = user.LockoutEnabled,
                                                        PhoneNumber = user.PhoneNumber,
                                                        FirstName = user.FirstName,
                                                        Address = user.Address,
                                                        Id = user.Id
                                                    } : null;


            return PartialView("EditUser", editUserViewModel);

        }

        [HttpPost]
        [ActionName("EditUser")]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Response = new EditUserResponseViewModel
                {
                    Succeeded = false,
                    Description = "Request does not contain the required information to update the user."
                };

                return View("Index");
            }

            ApplicationUser user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                ViewBag.Response = new EditUserResponseViewModel
                {
                    Succeeded = false,
                    Description = $"User with id: {model.Id} could not be found."
                };

                return View("Index");
            }

            user.Address = model.Address;
            user.EmailConfirmed = model.EmailConfirmed;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.LockoutEnabled = model.LockoutEnabled;
            user.PhoneNumber = model.PhoneNumber;
            user.LockoutEnabled = model.LockoutEnabled;
            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ViewBag.Response = new EditUserResponseViewModel
                {
                    Succeeded = false,
                    Description = "The was a problem updating the user. Please try again."
                };

                return View("Index");
            }

            ViewBag.Response = new EditUserResponseViewModel
            {
                Succeeded = true,
                Description = "User information were updated successfully."
            };

            return View("Index");
        }
    }
}