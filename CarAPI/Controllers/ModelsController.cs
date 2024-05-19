using CarAPI.Data.DTO;
using CarAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly CarMakeService _carMakeService;

        // For a simple project like this, you could potentially forgo the interface to keep things straightforward and I think this project don't have extinsion in future 
        // I don't add Authontication & Authorization level because not ask it in email , if you want added them please refer to me in email 
        public ModelsController(CarMakeService carMakeService)
        {
            _carMakeService = carMakeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetModels([FromQuery] string make, [FromQuery] int modelYear)
        {
            try
            {
                var models = await _carMakeService.GetModelsAsync(make, modelYear);
                var response = new ModelsResponse { Models = models };
                return Ok(response);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
