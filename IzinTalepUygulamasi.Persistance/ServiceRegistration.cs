using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Reflection;
public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        services.AddDbContext<IzinTalepAPIContext>(options => options.UseSqlServer(Configuration.ConnectionString));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies([Assembly.GetExecutingAssembly(), Assembly.Load("IzinTalepUygulamasi.Application")]));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        #region Repositories
        services.AddScoped<ILeaveRequestReadRepository, LeaveRequestReadRepository>();
        services.AddScoped<ILeaveRequestWriteRepository, LeaveRequestWriteRepository>();

        services.AddScoped<IADUserReadRepository, ADUserReadRepository>();

        services.AddScoped<ICumulativeLeaveReadRepository, CumulativeLeaveReadRepository>();
        services.AddScoped<ICumulativeLeaveWriteRepository, CumulativeLeaveWriteRepository>();

        services.AddScoped<INotificationReadRepository, NotificationReadRepository>();
        services.AddScoped<INotificationWriteRepository, NotificationWriteRepository>();
        #endregion

        #region AutoMapper
        services.AddAutoMapper(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            cfg.AddProfile<LeaveRequestProfile>();
            cfg.AddProfile<ADUserProfile>();
            cfg.AddProfile<CumulativeLeaveRequestProfile>();
        }, typeof(ServiceRegistration).Assembly);
        #endregion

        #region Json Configurations
        JsonConvert.DefaultSettings = () =>
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            settings.Converters.Add(new StringEnumConverter());
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return settings;
        };
        #endregion
    }
}
