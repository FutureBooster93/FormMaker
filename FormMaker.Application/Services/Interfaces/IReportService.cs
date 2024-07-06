using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Application.Services.Interfaces
{
    public interface IReportService
    {
        DataTable GetDynamicTableData(string tableName);
    }
}
