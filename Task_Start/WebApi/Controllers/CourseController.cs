using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using Models.Models;
using WebApi.Dto;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly StudentService _studentService;

        public CourseController(CourseService courseService, StudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
        }
        // GET: Course/Courses
        [HttpGet]
        public IActionResult Courses()
        {
            IEnumerable<CourseDto> model = _courseService.GetAllCourses()
                .Select(course => CourseDto.FromModel(course));
            return View(model);
        }

        // GET Course/Edit/22
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound(); 
            }

            var model =  CourseDto.FromModel(course);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CourseDto courseDto)
        {
            if(courseDto == null)
            {
                return BadRequest();
            }
            _courseService.UpdateCourse(courseDto.ToModel());
            return RedirectToAction("Courses");
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            _courseService.DeleteCourse(id);
            return RedirectToAction("Courses");
        }

        // GET Course/Create/22
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            var model = new CourseDto() { StartDate = System.DateTime.Now, EndDate = System.DateTime.Now };
            return View("Edit",model);
        }

        [HttpPost]
        public IActionResult Create(CourseDto courseDto)
        {

            if (courseDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", courseDto);
            }

            if (courseDto.StartDate > courseDto.EndDate)
            {
                ModelState.AddModelError("EndDate", "Start date cannot be after end date");
            }

            _courseService.CreateCourse(courseDto.ToModel());
            return RedirectToAction("Courses");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AssignStudents(int id)
        {
            var allstudents = _studentService.GetAllStudents();
            var course = _courseService.GetCourseById(id);
            if(course == null)
            {
                return BadRequest();
            }

            Student_Course_Assignmed model = new Student_Course_Assignmed();

            model.Id = id;
            model.Name = course.Name;
            model.PassCredits = course.PassCredits;
            model.StartDate = course.StartDate;
            model.EndDate = course.EndDate;
            model.Students = new List<AssignmedStudent_Viewmodel>();

            foreach(var student in allstudents)
            {
                bool isAssigned = course.Students.Any(p => p.Id == student.Id);
                model.Students.Add(new AssignmedStudent_Viewmodel() { StudentId = student.Id, StudentName = student.Name, IsAssigned = isAssigned });
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AssignStudent(Student_Course_Assignmed _Assignmed)
        {
            _courseService.SetStudentsToCourse(_Assignmed.Id, _Assignmed.Students.Where(p => p.IsAssigned).Select(stud => stud.StudentId));

            return RedirectToAction("Courses");
        }
    }
}
