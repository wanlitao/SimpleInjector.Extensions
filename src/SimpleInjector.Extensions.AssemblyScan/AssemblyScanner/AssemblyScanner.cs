using FCP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal class AssemblyScanner : IAssemblyScanner
    {
        #region CompositionRoot
        public IEnumerable<Type> GetCompositionRootTypes(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return assembly.GetExportedTypes().Where(CheckCompositionRootType);
        }

        private static bool CheckCompositionRootType(Type type)
        {
            return type.IsClass && !type.IsAbstract
                && type.FullName.EndsWith(AssemblyScanConstants.CompositionRootTypeSuffix)
                && type.GetMethod(AssemblyScanConstants.CompositionRootInvokeMethodName) != null;
        }
        #endregion

        public IEnumerable<Type> GetImplementationsOfType(Assembly assembly, Type resolveType)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (resolveType == null)
                throw new ArgumentNullException(nameof(resolveType));

            return assembly.GetExportedTypes()
                .Where(m => !m.IsAbstract && m != resolveType && m.Is(resolveType));
        }
    }
}
