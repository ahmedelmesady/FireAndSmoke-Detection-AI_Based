using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private readonly CameraService cameraService;
        private readonly IConfiguration _configuration;
        public CameraController(CameraService cameraService, IConfiguration configuration)
        {
            this.cameraService = cameraService;
            _configuration = configuration;
        }
        [HttpPost("start")]
        public async Task<IActionResult> StartCamera()
        {
            try
            {
                await Task.Run(() => cameraService.StartCameraAsync());
                return Ok("Camera started successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error starting camera: {ex.Message}");
            }
        }

        [HttpGet("Stream")]
        public IActionResult StreamCamera()
        {
            var streamUrl = _configuration["CameraSettings:RtspUrl"];

            return Ok(new {streamUrl });
        }
    }
}
