using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Threading.Tasks;
using TheBookClub.Common;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string, (int requestCount, DateTime resetTime)> _clients = new();
    private readonly int _maxRequests = 5; 
    private readonly TimeSpan _timeWindow = TimeSpan.FromMinutes(1); 

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        if (clientIp == null)
        {
            await _next(context);
            return;
        }

        var now = DateTime.UtcNow;

        var (requestCount, resetTime) = _clients.GetOrAdd(clientIp, _ => (0, now.Add(_timeWindow)));

        if (resetTime < now)
        {
            _clients[clientIp] = (1, now.Add(_timeWindow));
        }
        else if (requestCount >= _maxRequests)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";
            var response = new { message = "Too many request. Try again later." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            return;
        }
        else
        {
            _clients[clientIp] = (requestCount + 1, resetTime);
        }

        await _next(context);
    }
}