using System.Threading.Tasks;

namespace CircleSNS.API.Data
{
    public interface IDataRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
    }
}