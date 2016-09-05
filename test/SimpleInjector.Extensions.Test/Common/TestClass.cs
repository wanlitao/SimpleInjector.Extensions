using System;

namespace SimpleInjector.Extensions.Test
{
    public interface IDbContext
    {
    }

    public class DbContext : IDbContext
    {
    }

    public interface IRepository<TEntity> where TEntity : class
    {
        IDbContext DbContext { get; }
    }

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IDbContext _dbContext;

        public Repository(IDbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _dbContext = dbContext;
        }

        public IDbContext DbContext { get { return _dbContext; } }
    }

    public interface IService<TEntity> where TEntity : class
    {
        IRepository<TEntity> Repository { get; }
    }

    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private readonly IRepository<TEntity> _repository;

        public Service(IRepository<TEntity> repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            _repository = repository;
        }

        public IRepository<TEntity> Repository { get { return _repository; } }
    }

    public interface IUserService
    {
        UserInfo GetUserByKey(string id);

        int DeleteUserByKey(string id);
    }

    public class UserService : Service<UserInfo>, IUserService
    {
        public UserService(IRepository<UserInfo> repository)
            : base(repository)
        { }

        public UserInfo GetUserByKey(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            return new UserInfo { Id = Guid.NewGuid().ToString("N"), UserName = "SampleTest", MobilePhone = "15645731854" };
        }

        public int DeleteUserByKey(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));

            return 1;
        }
    }

    #region Models
    public class UserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }

        public int Sex { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public bool IsDelete { get; set; }
    }
    #endregion
}
