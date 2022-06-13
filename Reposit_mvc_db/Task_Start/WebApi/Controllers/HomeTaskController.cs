using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dto;

namespace WebApi.Controllers
{
    public class HomeTaskController : Controller
    { 
        private readonly HomeTaskService homeTaskService;

        public HomeTaskController(HomeTaskService homeTaskService)
        {
            this.homeTaskService = homeTaskService;
        }


        // GET: HomeTask/HomeTasks
        [HttpGet]
        public IActionResult HomeTasks()
        {
            IEnumerable<HomeTaskDto> model = homeTaskService.GetAllHomeTasks().
                Select(task => HomeTaskDto.FromModel(task));

            return View(model);
        }

        // GET HomeTask/Edit/4
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var hometask = homeTaskService.GetHomeTaskById(id);
            if(hometask == null)
            {
                return NotFound();
            }
            var model = HomeTaskDto.FromModel(hometask);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit (HomeTaskDto homeTaskDto)
        {
            if(homeTaskDto == null)
            {
                return BadRequest();
            }
            homeTaskService.UpdateHomeTask(homeTaskDto.ToModel());
            return RedirectToAction("HomeTasks");

        }

        // create HomeTaskController>/5
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Action = "Create";

            var model = new HomeTaskDto();
            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Create(HomeTaskDto homeTaskDto)
        {
            if(homeTaskDto == null)
            {
                return BadRequest();
            }
            homeTaskService.CreateHomeTask(homeTaskDto.ToModel());
            return RedirectToAction("HomeTasks");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            homeTaskService.DeleteHomeTask(id);
            return RedirectToAction("HomeTasks");
        }
    }
}
