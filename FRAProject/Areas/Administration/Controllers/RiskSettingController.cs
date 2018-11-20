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

        #region Category
        [HttpGet]
        public IActionResult GetCagetory() => PartialView("GetCategory");
        
        [HttpPost]
        [ActionName("GetCategorys")]
        public async Task<JsonResult> GetAllCatergoryJson(DataTable dataTable)
        {
            int pageNumber = dataTable.Start / dataTable.Length + 1;
            Order order = dataTable.Order.FirstOrDefault();

            SortDirection sortDirection = order != null
                ? (order.Direction == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                : SortDirection.Ascending;

            Catergory[] categorys = (await _categoryRepository.GetDataRocordsAsync(pageNumber, dataTable.Length, order?.Column ?? 0, sortDirection,
                string.IsNullOrEmpty(dataTable.Search.Value) ? string.Empty : dataTable.Search.Value)).ToArray();

            int totalNumberOfRecords = _categoryRepository.GetTotalNumberOfRecords();

            return Json(new DataTableResponse<Catergory>
            {
                Data = categorys,
                RecordsFiltered = string.IsNullOrEmpty(dataTable.Search.Value) ? totalNumberOfRecords : categorys.Length,
                Draw = dataTable.Draw,
                RecordsTotal = totalNumberOfRecords
            });
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            CategoryView model = new CategoryView();
            return PartialView("AddCategory", model);
        }

        [HttpPost]
        public JsonResult AddCategory([FromBody]CategoryView model)
        {
            Catergory catergory = new Catergory {CategoryName = model.CategoryName};

            _categoryRepository.AddRecordyAsync(catergory);
       
            //return PartialView("GetCategory");
            return Json("success");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(string CategoryID)
        {
            string CategoryName = string.Empty;

            Catergory catergory = await _categoryRepository.FindByIdAsync(CategoryID);
            if (!string.IsNullOrEmpty(CategoryID))
            {
                if (catergory != null)
                {
                    CategoryName = catergory.CategoryName;
                }
            }
            return PartialView("DeleteCategory", catergory);
        }

        [HttpPost]
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
                        return Json(result.Succeeded);
                    }
                }
            }                        
            return Json("Failed");
        }

        [HttpGet]
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
        public async Task<IActionResult> EditCategory([FromBody]Catergory model)
        {
            if (string.IsNullOrEmpty(model.CategoryID.ToString())) return PartialView("GetCategory");
            Catergory catergory = await _categoryRepository.FindByIdAsync(model.CategoryID.ToString());

            if (catergory != null)
            {
                catergory.CategoryName = model.CategoryName;
            }

            OperationResult result = await _categoryRepository.EditRecordAsync(catergory);

            return PartialView("GetCategory");
        }

        #endregion

        #region Section
        [HttpGet]
        public IActionResult GetSection() => PartialView("GetSection");

        [HttpPost]
        [ActionName("GetSections")]
        public async Task<JsonResult> GetAllSectionJson(DataTable dataTable)
        {
            int pageNumber = dataTable.Start / dataTable.Length + 1;
            Order order = dataTable.Order.FirstOrDefault();

            SortDirection sortDirection = order != null
                ? (order.Direction == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                : SortDirection.Ascending;

            Section[] sections = (await _sectionRepository.GetDataRocordsAsync(pageNumber, dataTable.Length, order?.Column ?? 0, sortDirection,
                string.IsNullOrEmpty(dataTable.Search.Value) ? string.Empty : dataTable.Search.Value)).ToArray();

            int totalNumberOfRecords = _categoryRepository.GetTotalNumberOfRecords();

            return Json(new DataTableResponse<Section>
            {
                Data = sections,
                RecordsFiltered = string.IsNullOrEmpty(dataTable.Search.Value) ? totalNumberOfRecords : sections.Length,
                Draw = dataTable.Draw,
                RecordsTotal = totalNumberOfRecords
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddSection()
        {
            SectionView sectionModel = new SectionView();
            var category = await _categoryRepository.GetAllRecords();

            Dictionary<int,string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);

            sectionModel.CategoryList = categoryList;

            return PartialView("AddSection", sectionModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddSection([FromBody]Section model)
        {
            Section section = new Section
            {
                CategoryID = model.CategoryID,
                SectionName = model.SectionName
            };

            if (!string.IsNullOrEmpty(model.CategoryID.ToString()))
            {
                OperationResult result = await _sectionRepository.AddRecordyAsync(section);
                if (result.Succeeded)
                {
                    return Json(result.Succeeded);
                }
            }
                        
            return Json("Failed");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSection(string SectionID)
        {
            Section section = await _sectionRepository.FindByIdAsync(SectionID);
            if (!string.IsNullOrEmpty(SectionID))
            {
                if (section != null)
                {
                    return PartialView("DeleteSection", section);
                }
            }
            return PartialView("DeleteSection");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteSection([FromBody]Section model)
        {
            if (!string.IsNullOrEmpty(model.SectionID.ToString()))
            {
                Section section = await _sectionRepository.FindByIdAsync(model.SectionID.ToString());
                if (section != null)
                {
                    OperationResult result = await _sectionRepository.DeleteRecordAsync(section);
                    if (result.Succeeded)
                    {
                        return Json(result.Succeeded);
                    }
                }
            }
            return Json("Failed");
        }

        [HttpGet]        
        public async Task<IActionResult> EditSection(string SectionID)
        {
            if (string.IsNullOrEmpty(SectionID)) return PartialView("EditCategory");

            Section section = await _sectionRepository.FindByIdAsync(SectionID);

            SectionView sectionViewModel = new SectionView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();

            Dictionary<int, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);

            sectionViewModel.DataId = section.SectionID.ToString();
            sectionViewModel.CategoryList = categoryList;
            sectionViewModel.SelectedIndex = section.CategoryID.ToString();
            sectionViewModel.SectionName = section.SectionName;
            
            return PartialView("EditSection", sectionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSection([FromBody]Section model)
        {
            if (string.IsNullOrEmpty(model.SectionID.ToString())) return PartialView("GetSection");
            Section section = await _sectionRepository.FindByIdAsync(model.SectionID.ToString());

            if (section != null)
            {
                section.CategoryID = Convert.ToInt32(model.CategoryID);
                section.SectionName = model.SectionName;
            }

            OperationResult result = await _sectionRepository.EditRecordAsync(section);

            return PartialView("GetSection");
        }

        #endregion

        #region Risk Template
        [HttpGet]
        public IActionResult GetRiskTemplate() => PartialView("GetRiskTemplate");

        [HttpPost]
        [ActionName("GetRiskTemplates")]
        public async Task<JsonResult> GetAllRiskTemplateJson(DataTable dataTable)
        {
            int pageNumber = dataTable.Start / dataTable.Length + 1;
            Order order = dataTable.Order.FirstOrDefault();

            SortDirection sortDirection = order != null
                ? (order.Direction == "asc" ? SortDirection.Ascending : SortDirection.Descending)
                : SortDirection.Ascending;

            RiskTemplate[] riskTemplate = (await _riskTemplateRepository.GetDataRocordsAsync(pageNumber, dataTable.Length, order?.Column ?? 0, sortDirection,
                string.IsNullOrEmpty(dataTable.Search.Value) ? string.Empty : dataTable.Search.Value)).ToArray();

            int totalNumberOfRecords = _categoryRepository.GetTotalNumberOfRecords();

            return Json(new DataTableResponse<RiskTemplate>
            {
                Data = riskTemplate,
                RecordsFiltered = string.IsNullOrEmpty(dataTable.Search.Value) ? totalNumberOfRecords : riskTemplate.Length,
                Draw = dataTable.Draw,
                RecordsTotal = totalNumberOfRecords
            });
        }

        [HttpGet]
        public async Task<IActionResult> AddRiskTemplate()
        {
            RiskTemplateView riskTemplateModel = new RiskTemplateView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();
            //IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();

            Dictionary<int, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);            
            riskTemplateModel.CategoryList = categoryList;            

            return PartialView("AddRiskTemplate", riskTemplateModel);
        }


        [HttpPost]
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
        public async Task<IActionResult> EditRiskTemplate(string TemplateID)
        {
            if (string.IsNullOrEmpty(TemplateID)) return PartialView("EditRiskTemplate");

            RiskTemplate riskTemplate = await _riskTemplateRepository.FindByIdAsync(TemplateID);

            RiskTemplateView riskTemplateView = new RiskTemplateView();
            IEnumerable<Catergory> category = await _categoryRepository.GetAllRecords();
            IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();

            Dictionary<int, string> categoryList = category.ToDictionary(items => items.CategoryID, items => items.CategoryName);
            Dictionary<int, string> sectionList = sections.ToDictionary(items => items.SectionID, items => items.SectionName);

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
        public async Task<IActionResult> EditRiskTemplate([FromBody]RiskTemplate model)
        {
            if (string.IsNullOrEmpty(model.TemplateID.ToString())) return PartialView("GetRiskTemplate");
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

            return PartialView("GetRiskTemplate");
        }

        [HttpGet]
        public async Task<IActionResult> GetSectiocForComboboxRiskTemplate(string CategoryID)
        {
            RiskTemplateView riskTemplateModel = new RiskTemplateView();
            
            IEnumerable<Section> sections = await _sectionRepository.GetAllRecords();
            IEnumerable<Section> newSections = sections.Where(data => data.CategoryID == Convert.ToInt32(CategoryID));

            Dictionary<int, string> sectionList = newSections.ToDictionary(items => items.SectionID, items => items.SectionName);

            riskTemplateModel.SectionList = sectionList;


            return PartialView("ComboBoxSection", riskTemplateModel);
        }

        #endregion

    }
 }