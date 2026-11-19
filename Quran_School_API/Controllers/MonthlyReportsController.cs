using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonthlyReportsController : ControllerBase
    {
        private readonly IMonthlyReportService _service;

        public MonthlyReportsController(IMonthlyReportService service)
        {
            _service = service;
        }

        // جميع التقارير
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        // تقرير محدد
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var report = await _service.GetById(id);

            if (report == null)
                return NotFound(new { message = "التقرير غير موجود." });

            return Ok(report);
        }

        // تقارير حلقة محددة
        [HttpGet("halaqa/{halaqaId}")]
        public async Task<IActionResult> GetByHalaqa(int halaqaId)
        {
            return Ok(await _service.GetByHalaqa(halaqaId));
        }

        // إنشاء تقرير
        [HttpPost]
        public async Task<IActionResult> Create(MonthlyReport report)
        {
            try
            {
                var result = await _service.Create(report);
                return CreatedAtAction(nameof(GetById),
                    new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "يوجد تقرير لهذه الحلقة في الفترة المحددة."
                });
            }
        }

        // تعديل تقرير
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MonthlyReport report)
        {
            try
            {
                var result = await _service.Update(id, report);

                if (result == null)
                    return NotFound(new { message = "التقرير غير موجود." });

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "تعذر تعديل التقرير بسبب تعارض في البيانات." });
            }
        }

        // حذف تقرير
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _service.Delete(id);

                if (!deleted)
                    return NotFound(new { message = "التقرير غير موجود." });

                return Ok(new { message = "تم حذف التقرير بنجاح." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // إرسال التقرير للإدارة
        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id)
        {
            try
            {
                bool submitted = await _service.Submit(id);

                if (!submitted)
                    return NotFound(new { message = "التقرير غير موجود." });

                return Ok(new { message = "تم إرسال التقرير للإدارة." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // اعتماد التقرير
        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id, [FromBody] AdminReviewRequest request)
        {
            try
            {
                bool approved = await _service.Approve(id, request.AdminNotes);

                if (!approved)
                    return NotFound(new { message = "التقرير غير موجود." });

                return Ok(new { message = "تم اعتماد التقرير بنجاح." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // إعادة التقرير للمعلم
        [HttpPost("{id}/return")]
        public async Task<IActionResult> ReturnToTeacher(int id, [FromBody] AdminReviewRequest request)
        {
            try
            {
                bool returned = await _service.ReturnToTeacher(id, request.AdminNotes);

                if (!returned)
                    return NotFound(new { message = "التقرير غير موجود." });

                return Ok(new { message = "تمت إعادة التقرير للمعلم." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class AdminReviewRequest
    {
        public string AdminNotes { get; set; } = string.Empty;
    }
}