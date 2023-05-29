namespace LibraryData.Gateways.Abstract;

using Models;

public interface IGateway<TEntity> where TEntity : Entity
{
    public IEnumerable<TEntity> GetAll();
    public IEnumerable<TEntity>? GetByPage(int page, int size);
    public TEntity? GetById(int id);

    public TEntity Insert(TEntity entity);
    public IEnumerable<TEntity>? InsertMulti(IEnumerable<TEntity>? entities);

    public TEntity Update(TEntity entity);
    public TEntity Delete(int id);
}