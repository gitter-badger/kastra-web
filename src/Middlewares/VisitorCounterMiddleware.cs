using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Kastra.Core.Business;
using Kastra.Core.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Kastra.Web.Middlewares
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitorCounterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStatisticsManager statisticsManager)
        {
            if (statisticsManager == null)
            {
                throw new ArgumentNullException(nameof(statisticsManager));
            }

            string visitorId = context.Request.Cookies["VisitorId"];
            
            if (context.Request != null && visitorId == null)
            {
                VisitorInfo visitor = new VisitorInfo();
                visitor.Id = Guid.NewGuid();
                visitor.LastVisitAt = DateTime.Now;
                visitor.UserAgent = context.Request.Headers[HeaderNames.UserAgent];
                visitor.IpAddress = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                visitor.UserId = context.Request.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                // Save in database
                statisticsManager.SaveVisitor(visitor);

                // Save in cookies
                context.Response.Cookies.Append("VisitorId", visitor.Id.ToString(), new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                });
            }

            await _next(context);
        }
    }
}
