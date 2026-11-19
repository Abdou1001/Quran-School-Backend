using Microsoft.AspNetCore.Mvc;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendancesController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var attendances = await _attendanceService.GetAllAttendances();
            return Ok(attendances);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var attendance = await _attendanceService.GetAttendanceById(id);

            if (attendance == null)
                return NotFound();

            return Ok(attendance);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Attendance attendance)
        {
            try
            {
                var newAttendance = await _attendanceService.AddAttendance(attendance);
                return Ok(newAttendance);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "سجل الحضور موجود مسبقًا لهذا الطالب في هذا اليوم." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Attendance attendance)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendance(id, attendance);

                if (!result)
                    return NotFound();

                return Ok("Attendance updated successfully");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "سجل الحضور موجود مسبقًا لهذا الطالب في هذا اليوم." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _attendanceService.DeleteAttendance(id);

            if (!result)
                return NotFound();

            return Ok("Attendance deleted successfully");
        }
    }
}