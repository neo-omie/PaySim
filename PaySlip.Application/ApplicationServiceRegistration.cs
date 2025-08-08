using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaySlip.Application.Contracts.Application;
using PaySlip.Application.Contracts.Persistence;
using PaySlip.Application.Services;

namespace PaySlip.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}
