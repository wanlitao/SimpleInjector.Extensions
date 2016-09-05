using System;
using System.Reflection;
using System.Reflection.Emit;
using FCP.Util;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal class CompositionRootExecutor : ICompositionRootExecutor
    {
        internal CompositionRootExecutor(Container container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            Container = container;
            AssemblyScanner = new AssemblyScanner();
        }

        #region Properties
        protected Container Container { get; private set; }

        protected IAssemblyScanner AssemblyScanner { get; private set; }
        #endregion

        #region Execute
        public void ExecuteCompositionRoot(Assembly assembly)
        {
            var compositionRootTypes = AssemblyScanner.GetCompositionRootTypes(assembly);

            if (compositionRootTypes.isEmpty())
                return;

            foreach (var compositionRootType in compositionRootTypes)
            {
                ExecuteCompositionRoot(compositionRootType);
            }
        }

        public void ExecuteCompositionRoot(Type compositionRootType)
        {
            if (compositionRootType == null)
                throw new ArgumentNullException(nameof(compositionRootType));

            var composeAction = CreateComposeMethodDelegate(compositionRootType);

            composeAction(Container);
        }

        private Action<Container> CreateComposeMethodDelegate(Type compositionRootType)
        {
            var constructor = compositionRootType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
                throw new ArgumentException($"Type: {compositionRootType.FullName} not have non-parameter constructor");

            var composeMethod = compositionRootType.GetMethod(AssemblyScanConstants.CompositionRootInvokeMethodName);
            if (composeMethod == null)
                throw new ArgumentException($"Type: {compositionRootType.FullName} not have {AssemblyScanConstants.CompositionRootInvokeMethodName} method");

            // validate compose method signature
            var composeMethodParameters = composeMethod.GetParameters();
            if (composeMethod.ReturnType != typeof(void) || composeMethodParameters.Length != 1
                || composeMethodParameters[0].ParameterType != typeof(Container))
                throw new ArgumentException($"Type: {compositionRootType.FullName} {AssemblyScanConstants.CompositionRootInvokeMethodName} method signature is not valid");

            var dynamicMethod = new DynamicMethod(string.Empty, typeof(void), new[] { typeof(Container) }, compositionRootType);
            var ilGenerator = dynamicMethod.GetILGenerator();

            // create the CompositionRootType instance and store it in a local variable
            ilGenerator.DeclareLocal(compositionRootType);
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Stloc_0);

            // load CompositionRootType instance (prepare for call later)
            ilGenerator.Emit(OpCodes.Ldloc_0);
            // load Container Argument
            ilGenerator.Emit(OpCodes.Ldarg_0);
            // Call CompositionRootType.Compose(Container)
            ilGenerator.EmitCall(OpCodes.Callvirt, composeMethod, null);
            
            ilGenerator.Emit(OpCodes.Ret);

            return (Action<Container>)dynamicMethod.CreateDelegate(typeof(Action<Container>));
        }
        #endregion
    }
}
