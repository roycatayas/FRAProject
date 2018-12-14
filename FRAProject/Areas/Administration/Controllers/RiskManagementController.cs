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
    [Authorize(Roles = "Administrator, User")]
    public class RiskManagementController : Controller
    {
        private readonly IRiskAssessmentRepository _riskAssessmentRepository;
        private readonly ITableDataRepository<Catergory> _categoryRepository;

        public RiskManagementController(IRiskAssessmentRepository riskAssessmentRepository, ITableDataRepository<Catergory> categoryRepository)
        {
            _riskAssessmentRepository = riskAssessmentRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public ViewResult Index() => View();

        [HttpGet]
        public ActionResult AddRisk()
        {
            RiskAssessmentView model = new RiskAssessmentView();
            
            return PartialView("AddNewRisk", model);
        }

        [HttpPost]
        public JsonResult AddRisk([FromBody]RiskAssessmentView model)
        {
            _riskAssessmentRepository.AddRiskAsync(model);
            return Json("success");
        }

        [HttpGet]
        public async Task<IActionResult> EditRisk(string RiskAssessmentID)
        {
            if (string.IsNullOrEmpty(RiskAssessmentID)) return PartialView("GetRiskAssessment");

            RiskAssessmentView riskAssessment = await _riskAssessmentRepository.FindByIdAsync(RiskAssessmentID);
            if (riskAssessment != null)
            {                
                riskAssessment.SiteCountry = riskAssessment.Country;
                riskAssessment.SiteAdress = riskAssessment.Address;
                riskAssessment.SiteStateProvince = riskAssessment.ProvinceState;
                riskAssessment.SurveyorTelephone = riskAssessment.SurveyorNumber;
                riskAssessment.ContactPersonName = riskAssessment.PrimaryContactName;
                riskAssessment.ContactPersonTelephone = riskAssessment.PhoneNumber;
                riskAssessment.ContactPersonFaxNumber = riskAssessment.FaxNumber;
                riskAssessment.ContactPersonEmail = riskAssessment.EmailAdress;
                riskAssessment.ContactPersonWebsiteUrl = riskAssessment.URLAdress;
                return PartialView("EditRisk", riskAssessment);
            }
            return PartialView("GetRiskAssessment");
        }

        [HttpPost]
        public JsonResult EditRisk([FromBody]RiskAssessmentView model)
        {
            if (string.IsNullOrEmpty(model.RiskAssessmentID.ToString())) return Json("Fail");

            _riskAssessmentRepository.EditRiskAsync(model);
            return Json("success");
        }

        [HttpPost]
        [ActionName("GetRiskAssessment")]
        public async Task<ActionResult> GetAllRiskAssessmentList()
        {            
            RiskAssessmentView[] riskListViews = (await _riskAssessmentRepository.GetRiskAssessmentAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();
            Catergory[] categoryList = (await _categoryRepository.GetDataRocordsAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();

            RiskAssessmentListView model = new RiskAssessmentListView { ListRiskAssessment = riskListViews, ListCategory = categoryList };

            foreach (RiskAssessmentView riskListView in riskListViews)
            {
                RiskDetailScoreView[] riskDetailScore = (await _riskAssessmentRepository.GetRiskDetailScoreRecordsByRiskId(riskListView.RiskAssessmentID)).ToArray();
                List<string> currentCategory = new List<string>();

                foreach (var iDetailScore in riskDetailScore)
                {
                    List<int> ids = new List<int>();
                    
                    RiskSectionScoreView[] riskSectionScore = (await _riskAssessmentRepository.GetRiskSectionScoreRecordsByRiskId(riskListView.RiskAssessmentID, iDetailScore.RiskDetailsID)).ToArray();
                    iDetailScore.ListRiskSectionScore = riskSectionScore;
                    iDetailScore.SectionDataCount = riskSectionScore.Length;                    

                    iDetailScore.MinRecordNo = 0;
                    iDetailScore.MaxRecordNo = 0;

                    if (riskSectionScore.Any())
                    {
                        ids.AddRange(riskSectionScore.Select(iData => iData.RiskSectionScoreID));

                        iDetailScore.MinRecordNo = ids.Min();
                        iDetailScore.MaxRecordNo = ids.Max();
                    }
                }

                currentCategory.AddRange(riskDetailScore.Select(iData => iData.CategoryName));
                riskListView.ListRiskDetailScoreViews = riskDetailScore;
            }            

            return PartialView("GetRiskAssessment", model);
        }

        [HttpPost]
        public JsonResult AddRiskDeitailScore([FromBody]RiskDetailScoreView model)
        {
            _riskAssessmentRepository.AddRiskDetailScoreAsync(model);           
            return Json("success");
        }

        [HttpPost]
        [ActionName("GetRiskDetailScore")]
        public async Task<ActionResult> GetAllRiskScoreList([FromBody]RiskDetailScoreView model)
        {
            RiskDetailScoreView[] riskDetailScore = (await _riskAssessmentRepository.GetRiskDetailScoreRecordsByRiskId(model.RiskAssessmentID)).ToArray();

            foreach (var iDetailScore in riskDetailScore)
            {
                List<int> ids = new List<int>();

                RiskSectionScoreView[] riskSectionScore = (await _riskAssessmentRepository.GetRiskSectionScoreRecordsByRiskId(model.RiskAssessmentID, iDetailScore.RiskDetailsID)).ToArray();
                iDetailScore.ListRiskSectionScore = riskSectionScore;
                iDetailScore.SectionDataCount = riskSectionScore.Length;

                iDetailScore.MinRecordNo = 0;
                iDetailScore.MaxRecordNo = 0;

                if (riskSectionScore.Any())
                {
                    ids.AddRange(riskSectionScore.Select(iData => iData.RiskSectionScoreID));

                    iDetailScore.MinRecordNo = ids.Min();
                    iDetailScore.MaxRecordNo = ids.Max();
                }
            }

            RiskDetailSectionScoreListView data = new RiskDetailSectionScoreListView {ListRiskDetailScoreViews = riskDetailScore};

            return PartialView("GetDetailSectionScore", data);
        }

        [HttpPost]
        [ActionName("GetRiskGuidelinesScoreEdit")]
        public async Task<ActionResult> GetRiskGuidelinesScoreList([FromBody]RiskDetailScoreView model)
        {
            RiskDetailSectionScoreListView data = new RiskDetailSectionScoreListView();

            RiskDetailScoreView[] riskDetailScore = (await _riskAssessmentRepository.GetRiskDetailScoreRecordsByRiskId(model.RiskAssessmentID)).ToArray();

            foreach (var iDetailScore in riskDetailScore.Where(a => a.RiskDetailsID == model.RiskDetailsID))
            {
                RiskSectionScoreView[] riskSectionScore = (await _riskAssessmentRepository.GetRiskSectionScoreRecordsByRiskId(model.RiskAssessmentID, iDetailScore.RiskDetailsID)).ToArray();
                data.ListRiskSectionScoreViews = riskSectionScore;
                data.ParticipantsNo = iDetailScore.ParticipantsNo;

                foreach (RiskSectionScoreView scoreView in data.ListRiskSectionScoreViews)
                {
                    RiskGuidelinesScoreView[] riskGuidelinesScore = (await _riskAssessmentRepository.GetRiskGuidelinesScoreViewByRiskId(model.RiskAssessmentID, iDetailScore.RiskDetailsID, scoreView.RiskSectionScoreID)).ToArray();
                    scoreView.ListRiskGuidelinesScore = riskGuidelinesScore;
                    scoreView.TotalRecordNo = riskGuidelinesScore.Length;
                    List<int> ids = new List<int>();
                    
                    foreach (RiskGuidelinesScoreView guidelinesScoreView in riskGuidelinesScore)
                    {
                        RiskParticipantsScoreView[] riskParticipantsScore = (await _riskAssessmentRepository.GetRiskParticipantsScoreRecordsById(guidelinesScoreView.RiskGuidelinesScoreID,scoreView.RiskSectionScoreID, iDetailScore.RiskDetailsID)).ToArray();
                        guidelinesScoreView.ListRiskParticipantsScore = riskParticipantsScore;                        
                    }

                    scoreView.MinRecordNo = 0;
                    scoreView.MaxRecordNo = 0;

                    if (riskGuidelinesScore.Any())
                    {
                        ids.AddRange(riskGuidelinesScore.Select(iData => iData.RiskGuidelinesScoreID));

                        scoreView.MinRecordNo = ids.Min();
                        scoreView.MaxRecordNo = ids.Max();
                    }                    
                }
            }
            
            return PartialView("EditRiskGuidelinesScore", data);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRiskSectionScore([FromBody]RiskSectionScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskSectionScoreID.ToString())) return Json("Fail");
            
            var dataResult = await _riskAssessmentRepository.UpdateRiskSectionScoreAsync(model);            

            return Json("success");
        }        

        [HttpPost]
        public async Task<JsonResult> UpdateRiskParticipantsScore([FromBody]RiskParticipantsScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskParticipantsID)) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskParticipantsScoreAsync(model);

            return Json("success");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRiskGuidelinesScore([FromBody]RiskGuidelinesScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskGuidelinesScoreID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskGuidelinesScoreAsync(model);

            return Json("success");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRiskGuidelinesComment([FromBody]RiskGuidelinesScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskGuidelinesScoreID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskGuidelinesScoreAsync(model.RiskGuidelinesScoreID.ToString(),model.Comments);

            return Json("success");
        }

        [HttpPost]
        public async Task<JsonResult> UpdateRiskDetailScore([FromBody]RiskDetailScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskDetailsID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskDetailScoreAsync(model);

            return Json("success");
        }
    }
}