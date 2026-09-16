using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplication1.Hubs;
using WebApplication1.Models;
using WebApplication1.Services;
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService sensorService;
        FireDbContext FireDbContext;
        public SensorController(FireDbContext context, ISensorService service)
        {
            sensorService = service;
            FireDbContext = context;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ReceiveData([FromBody] SensorReading sensorReading)
        {

            if (sensorReading == null)
            {
                return BadRequest("Invalid Data");
            }
            try
            {
                await sensorService.ProcessSensorReadingAsync(sensorReading);
                return Ok("Data Processed and Checked");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get")]
        public IActionResult GetData()
        {
            var data = FireDbContext.SensorReadings.ToList();
            return Ok(data);
        }


        [HttpGet("active-count")]
        public async Task<IActionResult> GetActiveCount()
        {
            var activeSensors = await FireDbContext.Sensors
                .CountAsync(s => s.LastSeen >= DateTime.UtcNow.AddMinutes(-1));

            return Ok(new { ActiveSensorCount = activeSensors });
        }
    }
}
