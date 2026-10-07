namespace Hirealdoor.Extentions
{
    public static class HttpContextExtensions
    {
        public static string? GetRealClientIpAddress(this HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(forwardedFor)) return context.Connection.RemoteIpAddress?.ToString();
            // The "X-Forwarded-For" header may contain a comma-separated list of IP addresses
            // The client's IP address should be the first one in the list
            var addresses = forwardedFor.Split(',');
            return addresses[0].Trim();
        }
    }
}