namespace Bookstore.Models.Repositories
{
    public interface IBookstoreRepository<TEntity> where TEntity : class
    {
        IList<TEntity> GetAll();
        TEntity? Find(int id);   // قد يرجع null إن لم يوجد

        IList<TEntity> Search(string? term);
        void Add(TEntity entity);
        void Update(int id, TEntity entity);
        void Delete(int id);


    }
}
