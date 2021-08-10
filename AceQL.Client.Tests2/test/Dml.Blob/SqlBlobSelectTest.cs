/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2021,  KawanSoft SAS
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
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.test.Dml.Blob
{
    public class SqlBlobSelectTest
    {
        private AceQLConnection connection;

        public SqlBlobSelectTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        public async Task BlobDownload(int customerId, int itemId, string blobPath)
        {
            string sql = "select * from orderlog where customer_id = @parm1 and item_id = @parm2 ";
            AceQLCommand command = new AceQLCommand(sql, connection);
            command.Parameters.AddWithValue("@parm1", customerId);
            command.Parameters.AddWithValue("@parm2", itemId);

            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("Get values using ordinal values:");
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i));

                    //customer_id
                    //item_id
                    //description
                    //item_cost
                    //date_placed
                    //date_shipped
                    //jpeg_image
                    //is_delivered
                    //quantity

                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("Get values using column name values:");
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("customer_id"))
                        + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("item_id")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("description")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("item_cost")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("date_placed")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("date_shipped")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("jpeg_image")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("is_delivered")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("quantity")));


                    AceQLConsole.WriteLine("==> dataReader.IsDBNull(3): " + dataReader.IsDBNull(3));
                    AceQLConsole.WriteLine("==> dataReader.IsDBNull(4): " + dataReader.IsDBNull(4));

                    // Download Blobs
                    using (Stream stream = await dataReader.GetStreamAsync(6))
                    {
                        using (var fileStream = File.Create(blobPath))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
        }

    }
}
