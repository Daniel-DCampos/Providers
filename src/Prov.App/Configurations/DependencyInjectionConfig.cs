using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Prov.App.Extensions;
using Prov.Business.Interfaces;
using Prov.Business.Notificacoes;
using Prov.Business.Services;
using Prov.Business.Services_Interfaces;
using Prov.Data.Context;
using Prov.Data.Repository;

namespace Prov.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependences(this IServiceCollection services)
        {
            //Repository

            services.AddScoped<ProvidersDbContext>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            //Service

            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IFornecedorService, FornecedorService>();


            //Extras
            services.AddScoped<INotificador, Notificador>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();

            return services;
        }
    }
}
