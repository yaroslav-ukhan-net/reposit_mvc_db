using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: Student/Students
        [HttpGet]
        public ViewResult Students()
        {
            IEnumerable<StudentDto> model = _studentService.GetAllStudents()
                .Select(student => StudentDto.FromModel(student));
            return View(model);
        }

        // GET Student/Edit/22
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var student = _studentService.GetStudentById(id);

            if(student == null)
            {
                return NotFound();
            }

            var model = StudentDto.FromModel(student);
            return View(model);
        }
        
        
        [HttpPost]
        public IActionResult Edit(StudentDto studentDto)
        {
            
            if (studentDto == null)
            {
                return BadRequest();
            }
            _studentService.UpdateStudent(studentDto.ToModel());
            return RedirectToAction("Students");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]

        public ActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return RedirectToAction("Students");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            var model = new StudentDto() { Name = "New Name" };
            return View("Edit", model);
        }

        [HttpPost]
        public IActionResult Create(StudentDto studentDto)
        {
            if(studentDto == null)
            {
                return BadRequest();
            }

            _studentService.CreateStudent(studentDto.ToModel());
            return RedirectToAction("Students");
        }

    }
}
