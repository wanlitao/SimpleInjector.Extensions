using System;
using System.Linq;

namespace SimpleInjector.Extensions.AssemblyScan
{
    /// <summary>
    /// Resolve Type By Scaning Define Assembly
    /// </summary>
    internal class DefineAssemblyTypeResolver : TypeResolverBase
    {
        internal DefineAssemblyTypeResolver(Container container, Lifestyle lifestyle)
            : base(container, lifestyle)
        {
            AssemblyScanner = new AssemblyScanner();
        }

        protected IAssemblyScanner AssemblyScanner { get; private set; }

        protected override Type GetImplementationOfType(Type resolveType)
        {
            return AssemblyScanner.GetImplementationsOfType(resolveType.Assembly, resolveType).SingleOrDefault();
        }
    }
}
