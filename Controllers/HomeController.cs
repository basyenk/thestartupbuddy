using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheStartupBuddy.Entities;
using TheStartupBuddy.Models;
using TheStartupBuddy.Utils;

namespace TheStartupBuddy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<AdminUserEntity> _adminUserRepository;

        public HomeController(ILogger<HomeController> logger, IRepository<AdminUserEntity> adminUserRepository)
        {
            _logger = logger;
            _adminUserRepository = adminUserRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View("/Views/Home/Form.cshtml", new AdminUserEntity());
        }

        public IActionResult Edit(int id)
        {
            AdminUserEntity data = _adminUserRepository.GetById(id);
            return View("/Views/Home/Form.cshtml", data);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            AdminUserEntity data = _adminUserRepository.GetById(id);
            _adminUserRepository.Delete(data);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HandleSave(AdminUserEntity request)
        {
            if (request != null && ModelState.IsValid)
            {
                if (request.Id > 0)
                {
                    AdminUserEntity adminUserEntity = _adminUserRepository.GetById(request.Id);
                    adminUserEntity.Name = request.Name;
                    adminUserEntity.Email = request.Email;
                    adminUserEntity.ProfilePhoto = request.ProfilePhoto;
                    adminUserEntity.Gender = request.Gender;
                    adminUserEntity.Age = request.Age;
                    adminUserEntity.Address = request.Address;
                    adminUserEntity.Position = request.Position;
                    _adminUserRepository.Update(adminUserEntity);
                }
                else
                {
                    _adminUserRepository.Insert(request);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult HandleRead()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var customerData = _adminUserRepository.Table;
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Name.Contains(searchValue)
                                            || m.Email.Contains(searchValue)
                                            || m.ProfilePhoto.Contains(searchValue)
                                            || m.Gender.Contains(searchValue)
                                            || m.Age.ToString().Contains(searchValue)
                                            || m.Address.Contains(searchValue)
                                            || m.Position.Contains(searchValue));
            }
            recordsTotal = customerData.Count();
            var data = customerData.Skip(skip).Take(pageSize).ToList();
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
    }
}
