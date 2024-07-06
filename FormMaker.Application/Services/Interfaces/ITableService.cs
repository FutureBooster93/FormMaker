using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Application.Services.Interfaces
{
    public interface ITableService
    {
        int CheckAndDropColumn(string schemaName, string tableName, string columnName);
        int CheckCreateOrAlterTable(string schemaName, string tableName, string columnDefinition);
        int DropTable(string schemaName, string tableName);
        int InsertToTableDynamic(string tableName, string fields, string values);
    }
}
