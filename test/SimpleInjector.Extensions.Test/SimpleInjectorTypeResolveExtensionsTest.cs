using Xunit;
using SimpleInjector.Extensions.AssemblyScan;

namespace SimpleInjector.Extensions.Test
{
    public class SimpleInjectorTypeResolveExtensionsTest
    {
        [Fact]
        public void ResolveUnregisteredType_Test()
        {
            var container = ContainerFactory.New();

            container.ResolveUnregisteredTypeByScanDefineAssembly();

            var instance = container.GetInstance(typeof(IDbContext));
            Assert.Equal(typeof(DbContext), instance.GetType());
        }

        [Fact]
        public void ResolveUnregisteredType_WithGeneric_Test()
        {
            var container = ContainerFactory.New();

            container.Register(typeof(IRepository<>), typeof(Repository<>));
            container.ResolveUnregisteredTypeByScanDefineAssembly();

            var instance = container.GetInstance(typeof(IDbContext));
            Assert.Equal(typeof(DbContext), instance.GetType());

            instance = container.GetInstance(typeof(IUserService));
            Assert.Equal(typeof(UserService), instance.GetType());
        }
    }
}
