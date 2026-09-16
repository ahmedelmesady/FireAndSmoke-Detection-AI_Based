using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Hubs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlertsController : ControllerBase
    {
        FireDbContext FireDbContext;
        IHubContext<AlertHub> Hub;
        public AlertsController(FireDbContext context, IHubContext<AlertHub> hub)
        {
            FireDbContext = context;
            Hub = hub;
        }

        [HttpGet("active-count")]
        public async Task<IActionResult> GetActiveCount()
        {
            var count = await FireDbContext.Alerts
                .CountAsync(a => a.CreatedAt.Date.Equals(DateTime.UtcNow.Date));

            return Ok(new { activeAlerts = count });
        }
    }
}
