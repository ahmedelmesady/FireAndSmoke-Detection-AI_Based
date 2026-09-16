using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationSettingsController : ControllerBase
    {

        private readonly FireDbContext _context;
        public NotificationSettingsController(FireDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Save(NotificationSettings nts)
        {
            var settings = new NotificationSettings
            {
                AccountSid = nts.AccountSid,
                AuthToken = nts.AuthToken,
                TwilioPhoneNumber = nts.TwilioPhoneNumber,
                TwilioWhatsappNumber = nts.TwilioWhatsappNumber,
                ReceiverPhone = nts.ReceiverPhone,
                SmsEnabled = nts.SmsEnabled,
                WhatsappEnabled = nts.WhatsappEnabled,
                EmailEnabled = nts.EmailEnabled
            };

            _context.NotificationSettings.Add(settings);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var settings = await _context.NotificationSettings.ToListAsync();
            if (settings == null)
            {
                return NotFound();
            }
            return Ok(settings);
        }
    }
}
