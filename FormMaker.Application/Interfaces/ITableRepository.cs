using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Application.Interfaces
{
    public interface ITableRepository
    {
        Task<int> CheckAndDropColumnAsync(string schemaName, string tableName, string columnName);
        Task<int> CheckCreateOrAlterTableAsync(string schemaName, string tableName, string columnDefinition);
        Task<int> DropTableAsync(string schemaName, string tableName);
        Task<int> InsertToTableDynamicAsync(string tableName, string fields, string values);
    }
}
