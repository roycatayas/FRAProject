using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Data.View;
using FRA.Web.Models.DataTables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator")]
    public class RiskSettingController : Controller
    {
        private readonly ITableDataRepository<Catergory> _categoryRepository;
        private readonly ITableDataRepository<Section> _sectionRepository;
        private readonly ITableDataRepository<RiskTemplate> _riskTemplateRepository;

        public RiskSettingController(ITableDataRepository<Catergory> categoryRepository, ITableDataRepository<Section> sectionRepository, ITableDataRepository<RiskTemplate> riskTemplateRepository)
        {
            _categoryRepository = categoryRepository;
            _sectionRepository = sectionRepository;
            _riskTemplateRepository = riskTemplateRepository;
        }

        [HttpGet]
        public ViewResult Index() => View();             

        #region Risk Template                   

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> GetRiskTemplateList()
        {
            RiskTemplateListView model = new RiskTemplateListView();
            IEnumerable<RiskTemplate> totalRecords = await _riskTemplateRepository.GetAllRecords();
            RiskTemplate[] riskTemplate = (await _riskTemplateRepository.GetDataRocordsAsync(1, totalRecords.Count(), 0, SortDirection.Ascending, string.Empty)).ToArray();
            model.ListRiskTemplate = riskTemplate;

            return PartialView("GetRiskTemplate", model); ;
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> AddRiskTemplate()
        {
            RiskTemplateView riskTemplateModel = new RiskTemplateView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();

            IEnumerable<Catergory> catergories = category as IList<Catergory> ?? category.ToList();
            Dictionary<string, string> categoryList = catergories.ToDictionary(items => items.CategoryID, items => items.CategoryName);            
            riskTemplateModel.CategoryList = categoryList;            
            riskTemplateModel.FirstCategoryRecordId = catergories.Select(d => d.CategoryID).FirstOrDefault();
            return PartialView("AddRiskTemplate", riskTemplateModel);
        }


        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> AddRiskTemplate([FromBody]RiskTemplate model)
        {
            RiskTemplate riskTemplate = new RiskTemplate
            {
                CategoryID = model.CategoryID,
                SectionID = model.SectionID,
                TempNumber = model.TempNumber,
                Questions = model.Questions,
                ControlGuidelines = model.ControlGuidelines,
                Impact = model.Impact
            };

            if (!string.IsNullOrEmpty(model.CategoryID))
            {
                OperationResult result = await _riskTemplateRepository.AddRecordyAsync(riskTemplate);
                if (result.Succeeded)
                {
                    return Json(result.Succeeded);
                }
            }

            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> DeleteRiskTemplate(string TemplateID)
        {
            RiskTemplate riskTemplate = await _riskTemplateRepository.FindByIdAsync(TemplateID);
            if (!string.IsNullOrEmpty(TemplateID))
            {
                if (riskTemplate != null)
                {
                    return PartialView("DeleteRiskTemplate", riskTemplate);
                }
            }
            return PartialView("DeleteRiskTemplate");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteRiskTemplate([FromBody]RiskTemplate model)
        {
            if (!string.IsNullOrEmpty(model.TemplateID.ToString()))
            {
                RiskTemplate riskTemplate = await _riskTemplateRepository.FindByIdAsync(model.TemplateID.ToString());
                if (riskTemplate != null)
                {
                    OperationResult result = await _riskTemplateRepository.DeleteRecordAsync(riskTemplate);
                    if (result.Succeeded)
                    {
                        return Json(result.Succeeded);
                    }
                }
            }
            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> EditRiskTemplate(string TemplateID)
        {
            if (string.IsNullOrEmpty(TemplateID)) return PartialView("EditRiskTemplate");

            RiskTemplate riskTemplate = await _riskTemplateRepository.FindByIdAsync(TemplateID);

            RiskTemplateView riskTemplateView = new RiskTemplateView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();
            IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();

            Dictionary<string, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);
            Dictionary<string, string> sectionList = sections.ToDictionary(items => items.SectionID, items => items.SectionName);

            riskTemplateView.DataId = riskTemplate.TemplateID.ToString();
            riskTemplateView.CategoryList = categoryList;
            riskTemplateView.SectionList = sectionList;
            riskTemplateView.CategoryIndex = riskTemplate.CategoryID;
            riskTemplateView.SectionIndex = riskTemplate.SectionID;
            riskTemplateView.TempNumber = riskTemplate.TempNumber;
            riskTemplateView.Questions = riskTemplate.Questions;
            riskTemplateView.ControlGuidelines = riskTemplate.ControlGuidelines;
            riskTemplateView.Impact = riskTemplate.Impact;

            return PartialView("EditRiskTemplate", riskTemplateView);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> EditRiskTemplate([FromBody]RiskTemplate model)
        {
            if (string.IsNullOrEmpty(model.TemplateID.ToString())) return Json("Failed");
            RiskTemplate riskTemplate = await _riskTemplateRepository.FindByIdAsync(model.TemplateID.ToString());

            if (riskTemplate != null)
            {
                riskTemplate.CategoryID = model.CategoryID;
                riskTemplate.SectionID = model.SectionID;
                riskTemplate.TempNumber = model.TempNumber;
                riskTemplate.Questions = model.Questions;
                riskTemplate.ControlGuidelines = model.ControlGuidelines;
                riskTemplate.Impact = model.Impact;
            }

            OperationResult result = await _riskTemplateRepository.EditRecordAsync(riskTemplate);
            if (result.Succeeded)
            {
                return Json("Success");
            }

            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> GetSectiocForComboboxRiskTemplate(string CategoryID)
        {
            RiskTemplateView riskTemplateModel = new RiskTemplateView();
            
            IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();
            IEnumerable<Section> newSections = sections.Where(data => data.CategoryID == CategoryID);

            Dictionary<string, string> sectionList = newSections.ToDictionary(items => items.SectionID, items => items.SectionName);

            riskTemplateModel.SectionList = sectionList;
            riskTemplateModel.FirstSectionRecordId = sectionList.Select(d => d.Key).FirstOrDefault();

            return PartialView("ComboBoxSection", riskTemplateModel);
        }

        #endregion

    }
 }