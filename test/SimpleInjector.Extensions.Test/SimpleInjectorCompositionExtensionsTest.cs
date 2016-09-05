using SimpleInjector.Extensions.AssemblyScan;
using Xunit;

namespace SimpleInjector.Extensions.Test
{
    public class SimpleInjectorCompositionExtensionsTest
    {
        [Fact]
        public void RegisterCompositionRoots_Test()
        {
            var container = ContainerFactory.New();

            container.RegisterCompositionRoots();

            var instance = container.GetInstance(typeof(IDbContext));
            Assert.Equal(typeof(DbContext), instance.GetType());

            instance = container.GetInstance(typeof(IUserService));
            Assert.Equal(typeof(UserService), instance.GetType());
        }
    }

    public class DbContextCompositionRoot
    {
        public void Compose(Container container)
        {
            container.Register<IDbContext, DbContext>();
        }
    }

    public class RepositoryCompositionRoot
    {
        public void Compose(Container container)
        {
            container.Register(typeof(IRepository<>), typeof(Repository<>));
        }
    }

    public class ServiceCompositionRoot
    {
        public void Compose(Container container)
        {
            container.Register<IUserService, UserService>();
        }
    }
}
