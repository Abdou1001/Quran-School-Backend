using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecitationProgressController : ControllerBase
    {
        private readonly IRecitationProgressService _progressService;

        public RecitationProgressController(
            IRecitationProgressService progressService)
        {
            _progressService = progressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var progress = await _progressService.GetAllProgress();

            return Ok(progress);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var progress = await _progressService.GetProgressById(id);

            if (progress == null)
                return NotFound(new { message = "السجل غير موجود." });

            return Ok(progress);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecitationProgress progress)
        {
            try
            {
                var newProgress = await _progressService.AddProgress(progress);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = newProgress.Id },
                    newProgress);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "تعذر حفظ السجل. ربما يوجد تسجيل مسبق لهذا الطالب في اليوم نفسه."
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            RecitationProgress progress)
        {
            try
            {
                var result = await _progressService.UpdateProgress(id, progress);

                if (!result)
                    return NotFound(new { message = "السجل غير موجود." });

                return Ok(new
                {
                    message = "تم تحديث سجل الحفظ والمراجعة بنجاح."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "تعذر تحديث السجل بسبب تعارض مع تسجيل موجود."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _progressService.DeleteProgress(id);

            if (!result)
                return NotFound(new { message = "السجل غير موجود." });

            return Ok(new
            {
                message = "تم حذف السجل بنجاح."
            });
        }
    }
}