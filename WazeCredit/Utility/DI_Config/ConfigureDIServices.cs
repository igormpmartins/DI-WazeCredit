using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WazeCredit.Data.Repository;
using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Service;
using WazeCredit.Service.LifeTimeExample;
using WazeCredit.Utility.AppSettingsClasses;

namespace WazeCredit.Utility.DI_Config
{
    public static class ConfigureDIServices
    {

        public static IServiceCollection AddAllServices(this IServiceCollection services)
        {

            services.AddTransient<IMarketForecaster, MarketForecasterV2>();
            //if it already exists, it just doesn't add the new one!
            //services.TryAddTransient<IMarketForecaster, MarketForecaster>();

            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();

            services.AddScoped<IValidationChecker, AddressValidationChecker>();
            services.AddScoped<IValidationChecker, CreditValidationChecker>();

            /*for enumerable injection!
            services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());

            //multiple injections!
            services.TryAddEnumerable(new ServiceDescriptor[]
            {
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>()
            });*/


            services.AddScoped<ICreditValidator, CreditValidator>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<CreditApprovedHigh>();
            services.AddScoped<CreditApprovedLow>();

            services.AddScoped<Func<CreditApprovedEnum, ICreditApproved>>(
                ServiceProvider => range =>
                {
                    switch (range)
                    {
                        case CreditApprovedEnum.High:
                            return ServiceProvider.GetService<CreditApprovedHigh>();
                        case CreditApprovedEnum.Low:
                            return ServiceProvider.GetService<CreditApprovedLow>();
                        default:
                            return ServiceProvider.GetService<CreditApprovedLow>();
                    }
                });

            //removal/replace
            //services.RemoveAll<IMarketForecaster>();
            //services.Replace(ServiceDescriptor.Transient<IMarketForecaster, MarketForecasterV2>());

            //possible ways of adding service:

            //interface + concrete class
            //services.AddTransient<IMarketForecaster, MarketForecasterV2>();

            //interface + instance (only singleton!)
            //services.AddSingleton<IMarketForecaster>(new MarketForecasterV2());
            //instance (only singleton!)
            //services.AddSingleton(new MarketForecasterV2());

            //type of
            //services.AddTransient(typeof(IMarketForecaster), typeof(MarketForecasterV2));
            //services.AddTransient(typeof(MarketForecasterV2));

            return services;
        }

    }
}
