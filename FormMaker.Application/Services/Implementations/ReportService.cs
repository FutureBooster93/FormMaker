using Aspose.Cells;
using FormMaker.Application.Interfaces;
using FormMaker.Application.Services.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Application.Services.Implementations
{
    public class ReportService:IReportService
    {
        
        private readonly IAdoDbContext db;

        public ReportService(IAdoDbContext _db)
        {
            db = _db;
        }
        public DataTable GetDynamicTableData(string tableName)
        {
            using (SqlConnection connection = db.CreateConnection())
            {
                using (SqlCommand command = new SqlCommand("sp_GetDynamicTableData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TableName", tableName);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    return dataTable;
                }
            }
        }
        
    }
}
