using System.Reflection;
using Application._Common.Behaviours;
using Application._Common.Interfaces;
using Application._Common.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            services.AddSingleton<IEventLogger, EventLogger>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddSingleton<WebSocketManager>();
            services.AddSingleton<ICurrentDateTime, CurrentDateTime>();
            return services;
        }
    }
}