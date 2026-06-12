using LibraryService.WebAPI.Data;
using LibraryService.WebAPI.DTO;
using LibraryService.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly IFraudService _fraudService;

        public FraudController(IFraudService fraudService)
        {
            _fraudService = fraudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var frauds = await _fraudService.GetAllAsync();
            return Ok(frauds);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FraudForm form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        e => e.Key,
                        e => e.Value!.Errors.Select(x => x.ErrorMessage).ToArray());

                return BadRequest(new { message = "Datos inválidos.", errors });
            }

            if (string.IsNullOrWhiteSpace(form.ImpostorDetails))
            {
                return BadRequest(new { message = "ImpostorDetails es obligatorio." });
            }

            if (string.IsNullOrWhiteSpace(form.ContactInfo))
            {
                return BadRequest(new { message = "ContactInfo es obligatorio." });
            }

            var fraud = new Fraud
            {
                ImpostorDetails = form.ImpostorDetails.Trim(),
                ContactInfo = form.ContactInfo.Trim(),
                Comments = form.Comments?.Trim() ?? string.Empty,
            };

            var created = await _fraudService.AddAsync(fraud);
            return StatusCode(StatusCodes.Status201Created, created);
        }
    }
}
