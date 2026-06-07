using System.Diagnostics;
using System.Security.Claims;

namespace Booking_Meeting_Rooms.Middleware
{
    public class MyLoggMiddleware
    {
        private readonly RequestDelegate _next;


        public MyLoggMiddleware(RequestDelegate next) 
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            Console.WriteLine($"[Start] {DateTime.Now} {context.Request.Method} {context.Request.Path} {context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonim"} ");

            await _next(context);

            watch.Stop();

            Console.WriteLine($"[End] {DateTime.Now} {context.Request.Method} {context.Request.Path} {context.User.FindFirst(ClaimTypes.Name)?.Value ?? "Anonymous"} {watch.ElapsedMilliseconds} ");

        }

    }
}
