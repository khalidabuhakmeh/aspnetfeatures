using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace aspnetfeatures.Middlewares
{
    public class ClockMiddleware : IMiddleware
    {
        public const string TimeZoneKey = "clock.timezone";
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var clock = new Clock();
            if (context.Request.Cookies.TryGetValue(TimeZoneKey, out var timezone))
            {
                clock.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone)
                    ?? TimeZoneInfo.Utc;
            }
            
            // set the feature
            context.Features.Set(clock);
            await next(context);
        }

        public static void SetTimeZone(HttpResponse response, string timeZoneId)
        {
            response.Cookies.Append(TimeZoneKey, timeZoneId);
        }
    }

    public class Clock
    {
        public DateTimeOffset DateTime { get; set; } 
            = DateTimeOffset.UtcNow;
        
        public TimeZoneInfo TimeZone { get; set; }
            = TimeZoneInfo.Utc;

        public DateTimeOffset Local =>
            TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime, TimeZone?.Id ?? TimeZoneInfo.Utc.Id);
    }
}