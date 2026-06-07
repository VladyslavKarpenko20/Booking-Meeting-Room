using Booking_Meeting_Rooms.Exceptions;

namespace Booking_Meeting_Rooms.Middleware
{
    public class MyExceptionsMiddleware
    {
        private readonly RequestDelegate _next;


        public MyExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundExceptions ex)
            {
                context.Response.StatusCode = 404;

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    exceptions = ex.Message
                });
            }
            catch (BadRequestExceptions ex)
            {
                context.Response.StatusCode = 400;

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    exceptions = ex.Message
                });
            }
            catch (UnAuthorizeExceptions ex)
            {
                context.Response.StatusCode = 409;

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    exceptions = ex.Message
                });
            }
            catch (ConflictExceptions ex)
            {
                context.Response.StatusCode = 401;

                await context.Response.WriteAsJsonAsync(new
                {
                    statusCode = context.Response.StatusCode,
                    exceptions = ex.Message
                });
            }
        }
    }
}
