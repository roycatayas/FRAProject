using System.Linq;
using System.Threading.Tasks;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Data.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class CategoryController : Controller
    {
        private readonly ITableDataRepository<Catergory> _categoryRepository;

        public CategoryController(ITableDataRepository<Catergory> categoryRepository)
        {
            _categoryRepository = categoryRepository;           
        }
        [HttpGet]
        public ViewResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetCagetoryList()
        {
            CatergoryListView model = new CatergoryListView();

            Catergory[] catergories = (await _categoryRepository.GetDataRocordsAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();
            model.ListCatergories = catergories;

            return PartialView("GetCategory", model);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult AddCategory()
        {
            CategoryView model = new CategoryView();
            return PartialView("AddCategory", model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> AddCategory([FromBody]CategoryView model)
        {
            Catergory catergory = new Catergory { CategoryName = model.CategoryName };
            await _categoryRepository.AddRecordyAsync(catergory);

            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EditCategory(string CategoryID)
        {
            if (string.IsNullOrEmpty(CategoryID)) return PartialView("EditCategory");

            Catergory catergory = await _categoryRepository.FindByIdAsync(CategoryID);
            if (catergory != null)
            {
                return PartialView("EditCategory", catergory);
            }
            return PartialView("EditCategory");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> EditCategory([FromBody]Catergory model)
        {
            if (string.IsNullOrEmpty(model.CategoryID.ToString())) return Json("Fail");

            Catergory catergory = await _categoryRepository.FindByIdAsync(model.CategoryID.ToString());

            if (catergory != null)
            {
                catergory.CategoryName = model.CategoryName;
            }

            OperationResult result = await _categoryRepository.EditRecordAsync(catergory);
            if (result.Succeeded)
            {
                return Json("success");
            }

            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteCategory(string CategoryID)
        {            
            if (!string.IsNullOrEmpty(CategoryID))
            {
                Catergory catergory = await _categoryRepository.FindByIdAsync(CategoryID);
                if (catergory != null)
                {
                    return PartialView("DeleteCategory", catergory);
                }
            }
            return PartialView("DeleteCategory");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteCategory([FromBody]Catergory model)
        {
            if (!string.IsNullOrEmpty(model.CategoryID.ToString()))
            {
                Catergory catergory = await _categoryRepository.FindByIdAsync(model.CategoryID.ToString());
                if (catergory != null)
                {
                    OperationResult result = await _categoryRepository.DeleteRecordAsync(catergory);
                    if (result.Succeeded)
                    {
                        return Json("Success");
                    }
                }
            }
            return Json("Failed");
        }
    }
}