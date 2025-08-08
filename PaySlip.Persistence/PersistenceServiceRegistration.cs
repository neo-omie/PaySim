using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaySlip.Application.Contracts.Persistence;
using PaySlip.Persistence.Context;
using PaySlip.Persistence.Repositories;

namespace PaySlip.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("PaySimConnStr")));
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            return services;
        }
    }
}
