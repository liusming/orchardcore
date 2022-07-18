using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Localization.Drivers;
using OrchardCore.Localization.Models;
using OrchardCore.Localization.Services;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;
using OrchardCore.Settings.Deployment;

namespace OrchardCore.Localization
{
    /// <summary>
    /// Represents a localization module entry point.
    /// </summary>
    public class Startup : StartupBase
    {
        public override int ConfigureOrder => -100;

        /// <inheritdocs />
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDisplayDriver<ISite>, LocalizationSettingsDisplayDriver>();
            services.AddScoped<INavigationProvider, AdminMenu>();
            services.AddScoped<IPermissionProvider, Permissions>();
            services.AddScoped<ILocalizationService, LocalizationService>();

            services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization").
                AddDataAnnotationsPortableObjectLocalization();

            services.Replace(ServiceDescriptor.Singleton<ILocalizationFileLocationProvider, ModularPoFileLocationProvider>());
        }

        /// <inheritdocs />
        public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var localizationService = serviceProvider.GetService<ILocalizationService>();

            var defaultCulture = localizationService.GetDefaultCultureAsync().GetAwaiter().GetResult();
            var supportedCultures = localizationService.GetSupportedCulturesAsync().GetAwaiter().GetResult();

            var options = serviceProvider.GetService<IOptions<RequestLocalizationOptions>>().Value;
            options.SetDefaultCulture(defaultCulture);
            options
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures)
                ;

            app.UseRequestLocalization(options);
        }
    }

    [RequireFeatures("OrchardCore.Deployment")]
    public class LocalizationDeploymentStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSiteSettingsPropertyDeploymentStep<LocalizationSettings, LocalizationDeploymentStartup>(S => S["Culture settings"], S => S["Exports the culture settings."]);
        }
    }

    [Feature("OrchardCore.Localization.ContentLanguageHeader")]
    public class ContentLanguageHeaderStartup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options => options.ApplyCurrentCultureToResponseHeaders = true);
        }
    }

    [Feature("OrchardCore.Localization.CulturePicker")]
    public class CulturePickerStartup : StartupBase
    {
        internal static readonly string AdminSiteCookieName = ".OrchardCore.AdminSiteCulture";

        private static readonly Task<ProviderCultureResult> NullProviderCultureResult = Task.FromResult(default(ProviderCultureResult));

        private readonly AdminOptions _adminOptions;

        public CulturePickerStartup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RequestLocalizationOptions>(options => options.AddInitialRequestCultureProvider(new CustomRequestCultureProvider(context =>
            {
                if (context.Request.Path.Value[1..].StartsWith(_adminOptions.AdminUrlPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var cookie = context.Request.Cookies[AdminSiteCookieName];

                    if (String.IsNullOrEmpty(cookie))
                    {
                        return NullProviderCultureResult;
                    }

                    var providerResultCulture = CookieRequestCultureProvider.ParseCookieValue(cookie);

                    return Task.FromResult(providerResultCulture);
                }
                else
                {
                    return NullProviderCultureResult;
                }
            })));
        }
    }
}
