using System;

namespace SimpleInjector.Extensions.AssemblyScan
{
    internal abstract class TypeResolverBase : ITypeResolver
    {
        private readonly Lifestyle _lifestyle;

        protected TypeResolverBase(Container container, Lifestyle lifestyle)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            Container = container;
            _lifestyle = lifestyle;
        }

        #region Properties
        protected Container Container { get; private set; }
        #endregion

        #region Type Predicate
        protected virtual Predicate<Type> ResolveTypePredicate
        {
            get
            {
                return (type) => CheckResolveType(type);
            }
        }

        protected static bool CheckResolveType(Type type)
        {
            return type.IsInterface && !(type.IsGenericType || type.FullName.StartsWith("System"));
        }
        #endregion

        #region Type Resolve
        protected abstract Type GetImplementationOfType(Type resolveType);
        #endregion

        #region Select Lifestyle
        private Lifestyle SelectLifestyle(Type serviceType, Type implementationType)
        {
            return _lifestyle ?? Container.Options.LifestyleSelectionBehavior.SelectLifestyle(serviceType, implementationType);
        }
        #endregion

        public EventHandler<UnregisteredTypeEventArgs> BuildUnregisteredTypeResolveEventHandler()
        {
            return (sender, e) =>
            {
                var resolveType = e.UnregisteredServiceType;
                if (!ResolveTypePredicate(resolveType))
                    return;

                var implementationType = GetImplementationOfType(resolveType);

                if (implementationType != null)
                {
                    var registration = SelectLifestyle(resolveType, implementationType).CreateRegistration(resolveType, implementationType, Container);
                    e.Register(registration);
                }
            };
        }
    }
}
