namespace SimpleInjector.Extensions.Test
{
    internal static class ContainerFactory
    {
        internal static Container New()
        {
            var container = new Container();

            container.Options.EnableDynamicAssemblyCompilation = true;

            return container;
        }
    }
}
