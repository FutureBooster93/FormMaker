using FormMaker.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker.Infrastructure.Data
{
    public class AdoDbContext:IAdoDbContext
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;
        public AdoDbContext(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = configuration["AdoConnectionString:Connection"];
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
