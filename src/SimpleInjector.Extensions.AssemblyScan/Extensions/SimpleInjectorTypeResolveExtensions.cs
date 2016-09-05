namespace SimpleInjector.Extensions.AssemblyScan
{
    public static class SimpleInjectorTypeResolveExtensions
    {
        public static void ResolveUnregisteredTypeByScanDefineAssembly(this Container container)
        {
            container.ResolveUnregisteredTypeByScanDefineAssembly(null);
        }

        public static void ResolveUnregisteredTypeByScanDefineAssembly(this Container container, Lifestyle lifestyle)
        {
            var typeResolver = new DefineAssemblyTypeResolver(container, lifestyle);

            container.ResolveUnregisteredType += typeResolver.BuildUnregisteredTypeResolveEventHandler();
        }
    }
}
