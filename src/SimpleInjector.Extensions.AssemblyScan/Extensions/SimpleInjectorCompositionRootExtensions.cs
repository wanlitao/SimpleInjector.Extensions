using FCP.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleInjector.Extensions.AssemblyScan
{
    public static class SimpleInjectorCompositionRootExtensions
    {
        #region Assembly Load
        private static IEnumerable<Assembly> GetAssembliesBySearchExecutingPath(string searchPattern)
        {
            if (!searchPattern.isNullOrEmpty())
            {
                string directory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

                if (!directory.isNullOrEmpty())
                {
                    string[] searchPatterns = searchPattern.Split('|');
                    foreach (string file in searchPatterns.SelectMany(sp => Directory.GetFiles(directory, sp)))
                    {
                        yield return Assembly.LoadFrom(file);
                    }
                }
            }
        }
        #endregion

        public static void RegisterCompositionRoots(this Container container)
        {
            container.RegisterCompositionRoots(Assembly.GetCallingAssembly());
        }

        public static void RegisterCompositionRoots(this Container container, string searchPattern)
        {
            container.RegisterCompositionRoots(GetAssembliesBySearchExecutingPath(searchPattern).ToArray());
        }

        public static void RegisterCompositionRoots(this Container container, params Assembly[] assemblies)
        {
            if (assemblies.isEmpty())
                return;

            var compositionRootExecutor = new CompositionRootExecutor(container);

            foreach(var assembly in assemblies)
            {
                compositionRootExecutor.ExecuteCompositionRoot(assembly);
            }
        }
    }
}
