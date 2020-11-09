using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary.DataAccess
{
    public interface IDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionString);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionString);
    }
}