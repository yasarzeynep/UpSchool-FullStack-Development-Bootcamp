using System.Linq.Expressions;
using SQLite;
using UpSchool.Domain.Entities;

namespace UpSchool.Domain.Data
{
    public class UserRepository:IUserRepository
    {
        private readonly SQLiteAsyncConnection _database;

        public UserRepository()
        {
            var dbPath = Path.Combine("C:\\Users\\zy_ya_000\\Desktop", "upschool.db"); // Direk masaüstünü verdi. sqlite:küçük dbler

            _database = new SQLiteAsyncConnection(dbPath); //Oluşturulan dbPath

            _database.CreateTableAsync<User>().Wait(); // Tablonun oluştuğunu kontrol etmek için
        }

        public Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _database.Table<User>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<int> AddAsync(User user, CancellationToken cancellationToken)
        {
            return _database.InsertAsync(user);
        }

        public Task<int> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return _database.UpdateAsync(user);
        }

        public Task<int> DeleteAsync(Expression<Func<User,bool>> predicate, CancellationToken cancellationToken)
        {
            return _database.Table<User>().DeleteAsync(predicate);
        }
    }
}
