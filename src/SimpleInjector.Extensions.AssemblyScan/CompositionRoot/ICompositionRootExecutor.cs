using System;
using System.Reflection;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal interface ICompositionRootExecutor
    {
        void ExecuteCompositionRoot(Assembly assembly);

        void ExecuteCompositionRoot(Type compositionRootType);
    }
}
