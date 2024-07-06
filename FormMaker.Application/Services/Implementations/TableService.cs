using FormMaker.Application.Interfaces;
using FormMaker.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Application.Services.Implementations
{
    public class TableService :ITableService
    {
        private readonly ITableRepository tableRepo;

        public TableService(ITableRepository _tableRepo)
        {
            tableRepo = _tableRepo;
        }
        public int CheckAndDropColumn(string schemaName, string tableName, string columnName)
        {
            var response = tableRepo.CheckAndDropColumnAsync(schemaName, tableName, columnName);
            return response.Result;
        }
        public int CheckCreateOrAlterTable(string schemaName, string tableName, string columnDefinition)
        {
            var response=tableRepo.CheckCreateOrAlterTableAsync(schemaName, tableName, columnDefinition);
            return response.Result;
        }
        public int DropTable(string schemaName, string tableName)
        {
            var response=tableRepo.DropTableAsync(schemaName, tableName);
            return response.Result;
        }

        public int InsertToTableDynamic(string tableName, string fields, string values)
        {
            var response=tableRepo.InsertToTableDynamicAsync(tableName, fields, values);
            return response.Result;
        }
    }
}
