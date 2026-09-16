namespace WebApplication1.Services
{
    public class AlertRateLimiter
    {
        private DateTime _lastAlertTime = DateTime.MinValue;
        private readonly TimeSpan _cooldown = TimeSpan.FromSeconds(30);

        public bool CanSendAlert()
        {
            if (DateTime.UtcNow - _lastAlertTime >= _cooldown)
            {
                _lastAlertTime = DateTime.UtcNow;
                return true;
            }

            return false;
        }
    }
}
