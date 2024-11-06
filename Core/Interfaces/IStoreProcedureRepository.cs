namespace Core.Interfaces
{
    using System.Threading.Tasks;

    public interface IStoreProcedureRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Executes a non-query stored procedure asynchronously.
        /// </summary>
        /// <param name="storeProcedure">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<TEntity> ExecuteStoreProcedureNonQueryAsync(string storeProcedure, string parameters = null);

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves result.
        /// </summary>
        /// <param name="storeProcedure">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation, containing the retrieved result.</returns>
        public Task<TEntity> ExecuteStoreProcedureAsync(string storeProcedure, string parameters = null);

        /// <summary>
        /// Executes a non-query function asynchronously with parameters.
        /// </summary>
        /// <param name="Query">The name of the stored procedure to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public Task<TEntity> ExecuteFunctionNonQueryAsync(string Query, string parameters = null);

        /// <summary>
        /// Executes a non-query function asynchronously with parameters.
        /// </summary>
        /// <param name="storeProcedure">The name of the stored procedure to execute.</param>
        /// <param name="entity">Optional parameters to pass to the stored procedure.</param>
        /// <returns></returns>
        public Task<object> ExecuteFunctionNonQueryAsync<T>(string storeProcedure, T entity);
    }
}

