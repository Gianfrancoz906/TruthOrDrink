using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TruthOrDrink
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Question>().Wait();
        }

        public Task<List<Question>> GetQuestionsAsync()
        {
            return _database.Table<Question>().ToListAsync();
        }

        public Task<int> AddQuestionAsync(Question question)
        {
            return _database.InsertAsync(question);
        }

        public Task<int> UpdateQuestionAsync(Question question)
        {
            return _database.UpdateAsync(question);
        }

        public Task<int> DeleteQuestionAsync(Question question)
        {
            return _database.DeleteAsync(question);
        }
    }
}
