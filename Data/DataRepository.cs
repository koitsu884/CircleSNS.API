using System.Threading.Tasks;

namespace CircleSNS.API.Data
{
    public class DataRepository : IDataRepository
    {
        private readonly ApplicationDbContext _context;
        public DataRepository(ApplicationDbContext context)
        {
            this._context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}