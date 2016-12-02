using SimpleInjector;
using SimpleInjector.Extensions.AssemblyScan;
using SimpleInjector.Integration.WebApi;
using System;
using System.Web.Http;

namespace FCP.SimpleInjector.WebApi
{
    public static class HttpConfigurationDependencyResolverExtensions
    {
        /// <summary>
        /// Use SimpleInjector DependencyResolver
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="compositionSearchPattern">composition dll search pattern</param>
        /// <returns></returns>
        public static HttpConfiguration UseSimpleInjector(this HttpConfiguration configuration, string compositionSearchPattern)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));            

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();

            container.RegisterCompositionRoots(compositionSearchPattern);
            container.ResolveUnregisteredTypeByScanDefineAssembly(Lifestyle.Scoped);

            container.RegisterWebApiControllers(configuration);

            container.Verify();

            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            return configuration;
        }
    }
}
