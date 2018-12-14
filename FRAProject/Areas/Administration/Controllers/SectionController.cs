using System;
using System.Collections.Generic;
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
    public class SectionController : Controller
    {
        private readonly ITableDataRepository<Section> _sectionRepository;
        private readonly ITableDataRepository<Catergory> _categoryRepository;

        public SectionController(ITableDataRepository<Section> sectionRepository, ITableDataRepository<Catergory> categoryRepository)
        {
            _sectionRepository = sectionRepository;
            _categoryRepository = categoryRepository;
        }
        [HttpGet]
        public ViewResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetSectionList()
        {
            SectionListView model = new SectionListView();
            IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();
            Section[] section = (await _sectionRepository.GetDataRocordsAsync(1, sections.Count(), 0, SortDirection.Ascending, string.Empty)).ToArray();
            model.ListSections = section;

            return PartialView("GetSection", model);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> AddSection()
        {
            SectionView sectionModel = new SectionView();
            var category = await _categoryRepository.GetAllRecords();

            Dictionary<string, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);

            sectionModel.CategoryList = categoryList;

            return PartialView("AddSection", sectionModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddSection([FromBody]SectionView model)
        {
            Section section = new Section
            {
                CategoryID = model.CategoryId,
                SectionName = model.SectionName
            };

            if (!string.IsNullOrEmpty(model.CategoryId))
            {
                OperationResult result = await _sectionRepository.AddRecordyAsync(section);
                if (result.Succeeded)
                {
                    return Json("Success");
                }
            }

            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EditSection(string sectionId)
        {
            if (string.IsNullOrEmpty(sectionId)) return PartialView("EditSection");

            Section section = await _sectionRepository.FindByIdAsync(sectionId);

            SectionView sectionViewModel = new SectionView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();

            Dictionary<string, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);

            sectionViewModel.SectionID = section.SectionID;
            sectionViewModel.CategoryList = categoryList;
            sectionViewModel.CategoryId = section.CategoryID;
            sectionViewModel.SectionName = section.SectionName;

            return PartialView("EditSection", sectionViewModel);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> EditSection([FromBody]SectionView model)
        {
            if (string.IsNullOrEmpty(model.SectionID)) return Json("Fail");
            Section section = await _sectionRepository.FindByIdAsync(model.SectionID);

            if (section != null)
            {
                section.CategoryID = model.CategoryId;
                section.SectionName = model.SectionName;
            }

            OperationResult result = await _sectionRepository.EditRecordAsync(section);
            if (result.Succeeded)
            {
                return Json("success");
            }

            return Json("Fail");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteSection(string sectionId)
        {
            if (!string.IsNullOrEmpty(sectionId))
            {
                Section section = await _sectionRepository.FindByIdAsync(sectionId);
                if (section != null)
                {
                    return PartialView("DeleteSection", section);
                }
            }
            return PartialView("DeleteSection");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteSection([FromBody]Section model)
        {
            if (!string.IsNullOrEmpty(model.SectionID))
            {
                Section section = await _sectionRepository.FindByIdAsync(model.SectionID);
                if (section != null)
                {
                    OperationResult result = await _sectionRepository.DeleteRecordAsync(section);
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