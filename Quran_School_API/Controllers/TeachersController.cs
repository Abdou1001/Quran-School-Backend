using Microsoft.AspNetCore.Mvc;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachers = await _teacherService.GetAllTeachers();
            return Ok(teachers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var teacher = await _teacherService.GetTeacherById(id);

            if (teacher == null)
                return NotFound();

            return Ok(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            var newTeacher = await _teacherService.AddTeacher(teacher);
            return Ok(newTeacher);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Teacher teacher)
        {
            var result = await _teacherService.UpdateTeacher(id, teacher);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.DeleteTeacher(id);

            if (!result)
                return NotFound();

            return Ok("Teacher deleted successfully");
        }
    }
}