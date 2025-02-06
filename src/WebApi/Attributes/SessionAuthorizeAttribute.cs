using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Application._Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace WebApi.Attributes
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userSessionService = context.HttpContext.RequestServices.GetService<IUserSessionService>();

            if (!context.RouteData.Values.TryGetValue("sessionId", out var sessionIdValue) ||
                string.IsNullOrEmpty(sessionIdValue?.ToString()))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var activeSession = userSessionService.ExistsActiveSession(sessionIdValue.ToString());

            if (!activeSession)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}