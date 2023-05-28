namespace Services.Validators.Shared
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface ICommonValidators : IDisposable
    {
        bool IsExistingEntityRow<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;

        Task<TEntity> GetFirstOrDefaultEntityRowAsync<TEntity>(Expression<Func<TEntity, bool>> filter) where TEntity : class;
    }
}
