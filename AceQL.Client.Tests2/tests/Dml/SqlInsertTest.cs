using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Tests.tests.Dml
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

        public async Task<int> InsertOrderlog(int customerId, string blobPath)
        {
            string sql =
            "insert into orderlog values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8, @parm9)";

            AceQLCommand command = new AceQLCommand(sql, connection);

            //string blobPath = IN_DIRECTORY + "username_koala.jpg";
            Stream stream = new FileStream(blobPath, FileMode.Open, FileAccess.Read);

            //customerId integer NOT NULL,
            //item_id integer NOT NULL,
            //description character varying(64) NOT NULL,
            //cost_price numeric,
            //date_placed date NOT NULL,
            //date_shipped timestamp without time zone,
            //jpeg_image oid,
            //is_delivered numeric,
            //quantity integer NOT NULL,

            command.Parameters.AddWithValue("@parm1", customerId);
            command.Parameters.AddWithValue("@parm2", customerId);
            command.Parameters.AddWithValue("@parm3", "Description_" + customerId);
            //command.Parameters.Add(new AceQLParameter("@parm4", new AceQLNullValue(AceQLNullType.DECIMAL))); //null value for NULL SQL insert.
            command.Parameters.AddWithValue("@parm4", 45.4);
            command.Parameters.AddWithValue("@parm5", DateTime.UtcNow);
            command.Parameters.AddWithValue("@parm6", DateTime.UtcNow);

            // Adds the Blob. (Stream will be closed by AceQLCommand)
            bool useBlob = true;
            if (useBlob)
            {
                command.Parameters.Add(new AceQLParameter("@parm7", stream));
            }
            else
            {
                command.Parameters.Add(new AceQLParameter("@parm7", new AceQLNullValue(AceQLNullType.BLOB)));
            }

            command.Parameters.AddWithValue("@parm8", 1);
            command.Parameters.AddWithValue("@parm9", customerId * 2000);

            int rows = await command.ExecuteNonQueryAsync();
            return rows;

        }
    }
}
