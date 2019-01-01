using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentFTP;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Data.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Administrator, User")]
    public class RiskManagementController : Controller
    {
        private readonly IRiskAssessmentRepository _riskAssessmentRepository;
        private readonly ITableDataRepository<Catergory> _categoryRepository;
        private readonly IContactPersonRepository _contactPersonRepository;
        //private readonly IHostingEnvironment _environment;
        private readonly IDocumentRepository _documentRepository;

        public RiskManagementController(IRiskAssessmentRepository riskAssessmentRepository, ITableDataRepository<Catergory> categoryRepository, IContactPersonRepository contactPersonRepositor, IDocumentRepository documentRepository)
        {
            _riskAssessmentRepository = riskAssessmentRepository;
            _categoryRepository = categoryRepository;
            _contactPersonRepository = contactPersonRepositor;
            //_environment = environment;
            _documentRepository = documentRepository;
        }

        [HttpGet]
        public ViewResult Index() => View();

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult AddRisk()
        {
            RiskAssessmentView model = new RiskAssessmentView();
            
            return PartialView("AddNewRisk", model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public JsonResult AddRisk([FromBody]RiskAssessmentView model)
        {
            _riskAssessmentRepository.AddRiskAsync(model);
            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public JsonResult EditRisk([FromBody]RiskAssessmentView model)
        {
            if (string.IsNullOrEmpty(model.RiskAssessmentID.ToString())) return Json("Fail");

            _riskAssessmentRepository.EditRiskAsync(model);
            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> DeleteRisk(string RiskAssessmentID)
        {
            if (!string.IsNullOrEmpty(RiskAssessmentID))
            {
                RiskAssessmentView riskAssessment = await _riskAssessmentRepository.FindByIdAsync(RiskAssessmentID);
                if (riskAssessment != null)
                {
                    return PartialView("DeleteRiskAssessment", riskAssessment);
                }
            }
            return PartialView("DeleteRiskAssessment");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteRisk([FromBody]RiskAssessmentView model)
        {
            if (!string.IsNullOrEmpty(model.RiskAssessmentID.ToString()))
            {
                RiskAssessmentView riskAssessment = await _riskAssessmentRepository.FindByIdAsync(model.RiskAssessmentID.ToString());
                if (riskAssessment != null)
                {
                    OperationResult result = await _riskAssessmentRepository.DeleteRiskAssessmentAsync(riskAssessment);
                    if (result.Succeeded)
                    {
                        return Json("Success");
                    }
                }
            }
            return Json("Failed");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ActionName("GetRiskAssessment")]
        public async Task<ActionResult> GetAllRiskAssessmentList()
        {            
            RiskAssessmentView[] riskListViews = (await _riskAssessmentRepository.GetRiskAssessmentAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();
            Catergory[] categoryList = (await _categoryRepository.GetDataRocordsAsync(1, 100, 0, SortDirection.Ascending, string.Empty)).ToArray();

            RiskAssessmentListView model = new RiskAssessmentListView { ListRiskAssessment = riskListViews, ListCategory = categoryList };

            foreach (RiskAssessmentView riskListView in riskListViews)
            {
                RiskDetailScoreView[] riskDetailScore = (await _riskAssessmentRepository.GetRiskDetailScoreRecordsByRiskId(riskListView.RiskAssessmentID)).ToArray();
                ContactPerson[] contactPerson = (await _contactPersonRepository.GetContactPersonById(riskListView.RiskAssessmentID.ToString())).ToArray();
                Document[] fileDocument = (await _documentRepository.GetDocumentById(riskListView.RiskAssessmentID.ToString())).ToArray();

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
                riskListView.ListContactPersons = contactPerson;
                riskListView.ListDocuments = fileDocument;
            }            

            return PartialView("GetRiskAssessment", model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public JsonResult AddRiskDeitailScore([FromBody]RiskDetailScoreView model)
        {
            _riskAssessmentRepository.AddRiskDetailScoreAsync(model);           
            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> DeleteRiskDeitailScore(string RiskDetailsID, string RiskAssessmentID)
        {
            if (string.IsNullOrEmpty(RiskAssessmentID)) return PartialView("DeleteRiskDetailScore");

            RiskDetailScoreView riskDetailScore = await _riskAssessmentRepository.FindByIdRiskDetailScoreAsync(RiskDetailsID, RiskAssessmentID);
            if (riskDetailScore != null)
            {
                return PartialView("DeleteRiskDetailScore", riskDetailScore);
            }
            return PartialView("DeleteRiskDetailScore");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteRiskDeitailScore([FromBody]RiskDetailScoreView model)
        {
            if (!string.IsNullOrEmpty(model.RiskDetailsID.ToString()))
            {
                RiskDetailScoreView riskDetailScore = await _riskAssessmentRepository.FindByIdRiskDetailScoreAsync(model.RiskDetailsID.ToString(), model.RiskAssessmentID.ToString());
                if (riskDetailScore != null)
                {
                    OperationResult result = await _riskAssessmentRepository.DeleteRiskDetailScoreAsync(riskDetailScore);
                    if (result.Succeeded)
                    {
                        return Json("Success");
                    }
                }
            }
            return Json("Failed");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ActionName("GetRiskDetailScore")]
        public async Task<ActionResult> GetAllRiskScoreList([FromBody]RiskDetailScoreView model)
        {
            RiskDetailScoreView[] riskDetailScore = (await _riskAssessmentRepository.GetRiskDetailScoreRecordsByRiskId(model.RiskAssessmentID)).ToArray();
            ContactPerson[] contactPerson = (await _contactPersonRepository.GetContactPersonById(model.RiskAssessmentID.ToString())).ToArray();
            Document[] fileDocument = (await _documentRepository.GetDocumentById(model.RiskAssessmentID.ToString())).ToArray();

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

            RiskDetailSectionScoreListView data = new RiskDetailSectionScoreListView
            {
                ListRiskDetailScoreViews = riskDetailScore,
                ListContactPersons = contactPerson,
                ListDocuments = fileDocument
            };

            return PartialView("GetDetailSectionScore", data);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> UpdateRiskSectionScore([FromBody]RiskSectionScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskSectionScoreID.ToString())) return Json("Fail");
            
            var dataResult = await _riskAssessmentRepository.UpdateRiskSectionScoreAsync(model);            

            return Json("success");
        }        

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> UpdateRiskParticipantsScore([FromBody]RiskParticipantsScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskParticipantsID)) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskParticipantsScoreAsync(model);

            return Json("success");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> UpdateRiskGuidelinesScore([FromBody]RiskGuidelinesScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskGuidelinesScoreID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskGuidelinesScoreAsync(model);

            return Json("success");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> UpdateRiskGuidelinesComment([FromBody]RiskGuidelinesScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskGuidelinesScoreID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskGuidelinesScoreAsync(model.RiskGuidelinesScoreID.ToString(),model.Comments);

            return Json("success");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> UpdateRiskDetailScore([FromBody]RiskDetailScoreView model)
        {
            if (string.IsNullOrEmpty(model.RiskDetailsID.ToString())) return Json("Fail");

            var dataResult = await _riskAssessmentRepository.UpdateRiskDetailScoreAsync(model);

            return Json("success");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult AddContactPerson(string RiskAssessmentID)
        {
            ContactPersonView model = new ContactPersonView();
            model.RiskAssessmentID = RiskAssessmentID;

            return PartialView("AddNewContactPerson", model);
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> AddContactPerson([FromBody]ContactPersonView model)
        {
            ContactPerson person = new ContactPerson
            {
                RiskAssessmentID = model.RiskAssessmentID,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber
            };
            await _contactPersonRepository.AddToContactPersonAsync(person);

            return Json("success");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]      
        public async Task<ActionResult> GetContactPersonList([FromBody]RiskDetailScoreView model)
        {            
            ContactPerson[] contactPerson = (await _contactPersonRepository.GetContactPersonById(model.RiskAssessmentID.ToString())).ToArray();            

            RiskDetailSectionScoreListView data = new RiskDetailSectionScoreListView { RiskAssessmentID = model.RiskAssessmentID.ToString(), ListContactPersons = contactPerson };

            return PartialView("GetContactPerson", data);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> DeleteContactPerson(string ContactPersonID, string RiskAssessmentID)
        {
            if (string.IsNullOrEmpty(RiskAssessmentID)) return PartialView("DeleteRiskDetailScore");
            ContactPerson model = new ContactPerson();
            model.RiskAssessmentID = RiskAssessmentID;
            model.ContactPersonID = ContactPersonID;

            ContactPerson contactPerson = await _contactPersonRepository.GetContactPersonById(model);
            if (contactPerson != null)
            {
                return PartialView("DeleteContactPerson", contactPerson);
            }
            return PartialView("DeleteContactPerson");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteContactPerson([FromBody]ContactPerson model)
        {
            if (!string.IsNullOrEmpty(model.ContactPersonID))
            {
                ContactPerson contactPerson = await _contactPersonRepository.GetContactPersonById(model);
                if (contactPerson != null)
                {
                    OperationResult result = await _contactPersonRepository.RemoveFromContactPersonAsync(contactPerson);
                    if (result.Succeeded)
                    {
                        return Json("Success");
                    }
                }
            }
            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Upload(string dataId)
        {
            UploadFileView fileUpload = new UploadFileView();
            fileUpload.DataId = dataId;
            return PartialView("UploadFile", fileUpload);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public async Task<ActionResult> Upload(IFormFile file, string RiskId, string documentName)
        {            
            try
            {
                if (file.Length > 0)
                {
                    string[] detectFileName = file.FileName.Split('\\');
                    //remove space on filename or special characters
                    string fileNameOnly = detectFileName[detectFileName.Length - 1].Replace(" ","");
                    string originalFileName = fileNameOnly;
                    #region clear file name for any special characters
                    fileNameOnly = fileNameOnly.Replace("'", "");
                    fileNameOnly = fileNameOnly.Replace("-", "");
                    fileNameOnly = fileNameOnly.Replace("_", "");
                    fileNameOnly = fileNameOnly.Replace("$", "");
                    fileNameOnly = fileNameOnly.Replace("%", "");
                    fileNameOnly = fileNameOnly.Replace("@", "");
                    fileNameOnly = fileNameOnly.Replace("*", "");
                    fileNameOnly = fileNameOnly.Replace("#", "");
                    #endregion

                    Guid ramdomId = Guid.NewGuid();
                    fileNameOnly = RiskId + "_" + ramdomId + "_" +fileNameOnly;
                    string remoteDirectory = "/document/" + fileNameOnly;

                    using (FtpClient client = new FtpClient())
                    {
                        client.Host = "fraweb.iweb-storage.com";
                        client.Port = 21;
                        client.Credentials = new NetworkCredential("fraweb-admin", "P@$$word123");                        

                        client.UploadFile(file.FileName, remoteDirectory);
                        client.RetryAttempts = 1;
                        client.UploadFile(file.FileName, remoteDirectory, FtpExists.Overwrite, false, FtpVerify.Retry);

                        client.Disconnect();
                    }

                    Document fileDocument = new Document
                    {
                        RiskAssessmentID = RiskId,
                        DocumentName = documentName,
                        FileName = originalFileName,
                        FTPLink = remoteDirectory,
                        DocumentGUID = ramdomId.ToString()
                    };
                    await _documentRepository.AddToDocumentAsync(fileDocument);
                }
            }
            catch (Exception exception)
            {
                return Json(new
                {
                    success = false,
                    response = exception.Message
                });
            }

            return Ok();
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> GetDocumentList([FromBody]RiskDetailScoreView model)
        {
            Document[] fileDocument = (await _documentRepository.GetDocumentById(model.RiskAssessmentID.ToString())).ToArray();

            RiskDetailSectionScoreListView data = new RiskDetailSectionScoreListView { RiskAssessmentID = model.RiskAssessmentID.ToString(), ListDocuments = fileDocument };

            return PartialView("GetDocument", data);
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> DeleteDocument(string DocId, string RiskAssessmentID)
        {
            if (string.IsNullOrEmpty(RiskAssessmentID)) return PartialView("DeleteDocument");
            Document model = new Document
            {
                RiskAssessmentID = RiskAssessmentID,
                DocId = DocId
            };

            Document fileDocument = await _documentRepository.GetDocumentById(model);
            if (fileDocument != null)
            {
                return PartialView("DeleteDocument", fileDocument);
            }
            return PartialView("DeleteDocument");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DeleteDocument([FromBody]Document model)
        {
            if (!string.IsNullOrEmpty(model.DocId))
            {
                Document fileDocument = await _documentRepository.GetDocumentById(model);
                if (fileDocument != null)
                {
                    using (FtpClient client = new FtpClient())
                    {
                        client.Host = "fraweb.iweb-storage.com";
                        client.Port = 21;
                        client.Credentials = new NetworkCredential("fraweb-admin", "P@$$word123");

                        client.DeleteFile(fileDocument.FTPLink);

                        client.Disconnect();
                    }

                    OperationResult result = await _documentRepository.RemoveFromDocumentAsync(fileDocument);
                    if (result.Succeeded)
                    {
                        return Json("Success");
                    }
                }
            }
            return Json("Failed");
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<ActionResult> DownloadFile(string DocId, string RiskAssessmentID)
        {
            if (string.IsNullOrEmpty(RiskAssessmentID)) return PartialView("DownloadFile");
            string downloadPath = @"C:\RiskDocuments";
            //Check if not exists then create the folder else do nothing
            if (!Directory.Exists(downloadPath))
            {
                Directory.CreateDirectory(downloadPath);
            }

            Document model = new Document
            {
                RiskAssessmentID = RiskAssessmentID,
                DocId = DocId                
            };

            Document fileDocument = await _documentRepository.GetDocumentById(model);
            if (fileDocument != null)
            {
                fileDocument.DownloadLocation = downloadPath;
                return PartialView("DownloadFile", fileDocument);
            }
            return PartialView("DownloadFile");            
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<JsonResult> DownloadFile([FromBody]Document model)
        {
            if (!string.IsNullOrEmpty(model.DocId))
            {
                Document fileDocument = await _documentRepository.GetDocumentById(model);
                if (fileDocument != null)
                {
                    string downloadPath = @"C:\RiskDocuments";
                    using (FtpClient client = new FtpClient())
                    {
                        downloadPath = downloadPath + @"\" + fileDocument.FileName;
                        client.Host = "fraweb.iweb-storage.com";
                        client.Port = 21;
                        client.Credentials = new NetworkCredential("fraweb-admin", "P@$$word123");

                        client.DownloadFile(downloadPath, fileDocument.FTPLink);

                        client.Disconnect();
                    }
                }

                return Json("Success");
            }
            return Json("Failed");
        }
    }
}