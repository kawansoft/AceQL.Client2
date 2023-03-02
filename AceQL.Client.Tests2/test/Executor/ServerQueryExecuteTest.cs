/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2023,  KawanSoft SAS
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
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Executor
{
    public class ServerQueryExecuteTest
    {

        public static void TheMain(string[] args)
        {
            try
            {
                DoIt().Wait();

                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue....");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
                AceQLConsole.WriteLine(exception.StackTrace);
                AceQLConsole.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        static async Task DoIt()
        {

            var netCoreVer = System.Environment.Version; // 3.0.0
            AceQLConsole.WriteLine(netCoreVer + "");

            using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
            {
                ServerQueryExecuteTest serverQueryExecuteTest = new ServerQueryExecuteTest();
                await serverQueryExecuteTest.ExecuteServerQueryAsync(connection);
            }

        }

        /// <summary>
        /// Calls an AceQL Java stored procedure
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task ExecuteServerQueryAsync(AceQLConnection connection)
        {
            AceQLCommand command = new AceQLCommand(connection);

            // Define the server Java class name to call
            String serverclassName = "com.mycompany.MyServerQueryExecutor";

            // Define the parameters list to pass to the server
            List<object> parameters = new List<object>();
            parameters.Add(1);

            AceQLConsole.WriteLine("com.mycompany.MyServerQueryExecutor execution:");

            // Our dataReader must be disposed to delete underlying downloaded files
            // Call the remote com.mycompany.MyServerQueryExecutor.executeQuery method
            // and get the result
            using AceQLDataReader dataReader = await command.ExecuteServerQueryAsync(serverclassName, parameters);
            while (dataReader.Read())
            {
                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("" + DateTime.Now);
                int i = 0;
                AceQLConsole.WriteLine(
                    "customer_id   : " + dataReader.GetValue(i++) + "\n"
                    + "customer_title: " + dataReader.GetValue(i++) + "\n"
                    + "fname         : " + dataReader.GetValue(i++) + "\n"
                    + "lname         : " + dataReader.GetValue(i++));
            }
        }
    }
}
