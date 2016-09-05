using System;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal interface ITypeResolver
    {
        EventHandler<UnregisteredTypeEventArgs> BuildUnregisteredTypeResolveEventHandler();
    }
}
