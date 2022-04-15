/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this filePath except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. 
 */

using AceQL.Client.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Dml.Blob
{
    public class SqlBlobInsertTest
    {
        private AceQLConnection connection;

        public SqlBlobInsertTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        public async Task<int> BlobUpload(int customerId, int itemId, string blobPath)
        {
            string sql =
            "insert into orderlog values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8, @parm9)";

            AceQLCommand command = new AceQLCommand(sql, connection);
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
            command.Parameters.AddWithValue("@parm2", itemId);
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

            AceQLProgressIndicator progressIndicator = new AceQLProgressIndicator();
            connection.SetProgressIndicator(progressIndicator);

            int rows = await command.ExecuteNonQueryAsync();
            return rows;

        }
    }
}
