using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using VakantieProject.Dtos;

namespace VakantieProject.Data
{
    public class SqlPriceRangeDal
    {
        private readonly string ConnectionString;

        public SqlPriceRangeDal(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public async Task<Task> SavePriceRange(PriceRange priceRange)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("max_amount", priceRange.MaxAmount);
                parameters.Add("min_amount", priceRange.MinAmount);
                parameters.Add("percentage", priceRange.Percentage);
                parameters.Add("boundary", priceRange.Boundary);
                parameters.Add("created_at", priceRange.CreatedAt);
                parameters.Add("@success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                try
                {
                    await sqlConnection.OpenAsync();

                    var result = await sqlConnection.QueryAsync("dbo.savepricerange", parameters, commandType: CommandType.StoredProcedure);

                    if (!parameters.Get<bool>("@success"))
                    {

                        throw new Exception("Savepricerange stored procedure failed.");
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    await sqlConnection.CloseAsync();
                }
            }


            return Task.CompletedTask;
            
        }
    }
}
