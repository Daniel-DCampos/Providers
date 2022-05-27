using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Prov.App.Configurations
{
    public static class GlobalizationConfig
    {
        public static IApplicationBuilder UseGlobalizationConfiguration(this IApplicationBuilder app)
        {

            var defaultCulture = new CultureInfo("pt-BR");
            var localization = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo>() { defaultCulture },
                SupportedUICultures = new List<CultureInfo>() { defaultCulture }
            };

            app.UseRequestLocalization(localization);

            return app;
        }
    }
}
