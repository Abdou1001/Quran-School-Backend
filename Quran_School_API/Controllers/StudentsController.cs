
using Microsoft.AspNetCore.Mvc;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentService.GetStudentById(id);

            if (student == null)
                return NotFound("الطالب غير موجود");

            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FullName))
                return BadRequest("اسم الطالب مطلوب");

            var newStudent = await _studentService.AddStudent(student);

            return CreatedAtAction(
                nameof(GetById),
                new { id = newStudent.Id },
                newStudent
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FullName))
                return BadRequest("اسم الطالب مطلوب");

            var result = await _studentService.UpdateStudent(id, student);

            if (!result)
                return NotFound("الطالب غير موجود");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.DeleteStudent(id);

            if (!result)
                return NotFound("الطالب غير موجود");

            return Ok("تم حذف الطالب بنجاح");
        }
    }
}
