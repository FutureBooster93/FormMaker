using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormMaker.Application.Interfaces;

namespace FormMaker.Infrastructure.Repository
{
    public class TableRepository:ITableRepository
    {
        private readonly IAdoDbContext db;

        public TableRepository(IAdoDbContext _db)
        {
            db = _db;
        }
        public async Task<int> CheckAndDropColumnAsync(string schemaName, string tableName, string columnName)
        {
            using (var connection = db.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_CheckAndDropColumnOrTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SchemaName", schemaName);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@ColumnName", columnName);

                    var responseParameter = new SqlParameter
                    {
                        ParameterName = "@Response",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(responseParameter);

                    await command.ExecuteNonQueryAsync();

                    return (int)responseParameter.Value;
                }
            }
        }
        public async Task<int> CheckCreateOrAlterTableAsync(string schemaName, string tableName, string columnDefinition)
        {
            using (var connection = db.CreateConnection())
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("sp_CheckCreateOrAlterTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SchemaName", schemaName);
                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@ColumnDefinition", columnDefinition);

                    var responseParameter = new SqlParameter
                    {
                        ParameterName = "@Response",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(responseParameter);

                    await command.ExecuteNonQueryAsync();

                    return (int)responseParameter.Value;
                }
            }
        }
        public async Task<int> DropTableAsync(string schemaName, string tableName)
        {
            using (var connection = db.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_DropTable", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@SchemaName", schemaName);
                    command.Parameters.AddWithValue("@TableName", tableName);

                    var responseParameter = new SqlParameter
                    {
                        ParameterName = "@Response",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(responseParameter);

                    await command.ExecuteNonQueryAsync();

                     
                    return (int)responseParameter.Value; 
                }
            }
        }
        public async Task<int> InsertToTableDynamicAsync(string tableName, string fields, string values)
        {
            using (var connection = db.CreateConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_InsertToTableDynamic", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TableName", tableName);
                    command.Parameters.AddWithValue("@Fields", fields);
                    command.Parameters.AddWithValue("@Values", values);

                    var responseParameter = new SqlParameter
                    {
                        ParameterName = "@Response",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(responseParameter);

                    await command.ExecuteNonQueryAsync();

                    return (int)responseParameter.Value;
                }
            }
        }
    }
}
