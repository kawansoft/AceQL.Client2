using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Dml
{
    public class SqlDeleteTest
    {
        private AceQLConnection connection;

        public SqlDeleteTest(AceQLConnection connection)
        {
            this.connection = connection;

        }

        public async Task<int>  DeleteCustomerAll()
        {
            string sql = "delete from customer";

            AceQLCommand command = new AceQLCommand
            {
                CommandText = sql,
                Connection = connection
            };
            command.Prepare();

            int rows = await command.ExecuteNonQueryAsync();
            return rows;
        }


        public async Task<int> DeleteOrderlogAll()
        {
            string sql = "delete from orderlog";

            AceQLCommand command = new AceQLCommand
            {
                CommandText = sql,
                Connection = connection
            };
            command.Prepare();

            int rows = await command.ExecuteNonQueryAsync();
            return rows;
        }
    }
}
