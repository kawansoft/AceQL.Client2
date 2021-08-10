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
