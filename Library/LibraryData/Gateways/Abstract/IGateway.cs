namespace LibraryData.Gateways.Abstract;

using Models;

public interface IGateway<TEntity> where TEntity : Entity
{
    IEnumerable<TEntity> GetAll();
    IEnumerable<TEntity>? GetAllEntities();
    IEnumerable<TEntity>? GetEntitiesByPage(int page, int size);

    IEnumerable<TEntity>? GetByIds(IEnumerable<int> ids);
    TEntity? GetById(int id);
    TEntity Insert(TEntity entity);
    IEnumerable<TEntity>? InsertMulti(IEnumerable<TEntity>? entities);
    TEntity Update(TEntity entity);
    TEntity Delete(int id);
}