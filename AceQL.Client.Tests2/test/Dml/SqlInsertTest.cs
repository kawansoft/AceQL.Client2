using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Dml
{
    public class SqlInsertTest
    {
        private AceQLConnection connection;

        public SqlInsertTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> InsertCustomer(int customerId)
        {
            string sql = "insert into customer values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8)";

            AceQLCommand command = new AceQLCommand(sql, connection);

            command.Parameters.AddWithValue("@parm1", customerId);
            command.Parameters.AddWithValue("@parm2", ""); // HACK NDP
            command.Parameters.AddWithValue("@parm3", "André_" + customerId);
            command.Parameters.Add(new AceQLParameter("@parm4", "Name_" + customerId));
            command.Parameters.AddWithValue("@parm5", customerId + ", road 66");
            command.Parameters.AddWithValue("@parm6", "Town_" + customerId);
            command.Parameters.AddWithValue("@parm7", customerId + "1111");
            command.Parameters.Add(new AceQLParameter("@parm8", new AceQLNullValue(AceQLNullType.VARCHAR))); //null value for NULL SQL insert.

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            int rows = await command.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            command.Dispose();
            return rows;

        }

    }
}
