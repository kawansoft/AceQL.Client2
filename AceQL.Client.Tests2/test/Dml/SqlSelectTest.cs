using AceQL.Client.Api;
using AceQL.Client.Test.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Dml
{
    public class SqlSelectTest
    {
        private AceQLConnection connection;

        public SqlSelectTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        public async Task SelectCustomerExecute()
        {
            string sql = "select * from customer where customer_id > @parm1";
            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@parm1", 1);

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("" + DateTime.Now);
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i));
                }
            }
        }

        public async Task SelectOneCustomer()
        {
            string sql = "select * from customer where customer_id >=1 limit 1";
            AceQLCommand command = new AceQLCommand(sql, connection);

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("" + DateTime.Now);
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i));
                }
            }
        }

        public async Task SelectCustomers()
        {
            string sql = "select * from customer where customer_id > @parm1";
            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@parm1", 1);

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("" + DateTime.Now);
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i));
                }
            }
        }

        public async Task<int> SelectMaxCustomers()
        {
            string sql = "select max(customer_id) from customer";
            AceQLCommand command = new AceQLCommand(sql, connection);
            int maxCustomerId = (Int32)await command.ExecuteScalar();
            return maxCustomerId;
        }

    }
}
