using System;
using System.Collections.Generic;
using System.Reflection;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal interface IAssemblyScanner
    {
        IEnumerable<Type> GetCompositionRootTypes(Assembly assembly);

        IEnumerable<Type> GetImplementationsOfType(Assembly assembly, Type resolveType);
    }
}
