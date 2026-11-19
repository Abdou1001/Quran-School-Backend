using Microsoft.AspNetCore.Mvc;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HalaqasController : ControllerBase
    {
        private readonly IHalaqaService _halaqaService;

        public HalaqasController(IHalaqaService halaqaService)
        {
            _halaqaService = halaqaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var halaqas = await _halaqaService.GetAllHalaqas();
            return Ok(halaqas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var halaqa = await _halaqaService.GetHalaqaById(id);

            if (halaqa == null)
                return NotFound();

            return Ok(halaqa);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Halaqa halaqa)
        {
            var newHalaqa = await _halaqaService.AddHalaqa(halaqa);
            return Ok(newHalaqa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Halaqa halaqa)
        {
            var result = await _halaqaService.UpdateHalaqa(id, halaqa);

            if (!result)
                return NotFound();

            return Ok("Halaqa updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _halaqaService.DeleteHalaqa(id);

            if (!result)
                return NotFound();

            return Ok("Halaqa deleted successfully");
        }
    }
}