using System.Data.Common;
using System.Data;
using Dapper; 
namespace AppModels
{ 
    public interface IDapperContext
    { 
        Task<dynamic> ExecuteStoredProcedure(string storedProcedureName, object parameters = null);
        Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, object parameters = null);
        Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string storedProcedureName, object parameters = null);
    }
     
    public class DapperContext : IDapperContext
    {
        
        #region Constructors 
        private readonly DbConnection _dbConnection;
        public DapperContext(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        #endregion


        public async Task<dynamic> ExecuteStoredProcedure(string storedProcedureName, object parameters = null)
        {
            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    await _dbConnection.OpenAsync();

                return await _dbConnection.QueryAsync(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { return null; }
            finally
            {
                if (_dbConnection.State == ConnectionState.Open)
                    await _dbConnection.CloseAsync();
            }
        }

        public async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedureName));

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    await _dbConnection.OpenAsync();

                return await _dbConnection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) { throw; }
            finally
            {
                if (_dbConnection.State == ConnectionState.Open)
                    await _dbConnection.CloseAsync();
            }
        }

        public async Task<T> ExecuteQueryFirstOrDefaultAsync<T>(string storedProcedureName, object parameters = null)
        {
            if (string.IsNullOrWhiteSpace(storedProcedureName))
                throw new ArgumentException("Stored procedure name cannot be null or empty.", nameof(storedProcedureName));

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    await _dbConnection.OpenAsync();

                return await _dbConnection.QueryFirstOrDefaultAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure );
            }
            catch (Exception ex) { throw; }
            finally
            {
                if (_dbConnection.State == ConnectionState.Open)
                    await _dbConnection.CloseAsync();
            }
        }

    }
}
