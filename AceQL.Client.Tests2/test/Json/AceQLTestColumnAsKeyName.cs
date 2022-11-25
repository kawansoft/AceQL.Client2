/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
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
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Json
{
    /// <summary>
    /// Class to test that the AceQL JSON key names "row_n" and "row_count" can safely be used as column names.
    ///A dedicated table customer_2 is used.
    /// </summary>
    static class AceQLTestColumnAsKeyName
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
            string connectionString = ConnectionStringCurrent.Build();

            // Make sure connection is always closed to close and release server connection into the pool
            using (AceQLConnection connection = new AceQLConnection(connectionString))
            {
                await ExecuteExample(connection).ConfigureAwait(false);
                await connection.CloseAsync();
            }

        }

        /// <summary>
        /// Executes our example using an <see cref="AceQLConnection"/> 
        /// </summary>
        /// <param name="connection"></param>
        private static async Task ExecuteExample(AceQLConnection connection)
        {

            await connection.OpenAsync();

            AceQLConsole.WriteLine("host: " + connection.ConnectionInfo.ConnectionString);
            AceQLConsole.WriteLine("aceQLConnection.GetClientVersion(): " + AceQLConnection.GetClientVersion());
            AceQLConsole.WriteLine("aceQLConnection.GetServerVersion(): " + await connection.GetServerVersionAsync());
            AceQLConsole.WriteLine("AceQL local folder: ");
            AceQLConsole.WriteLine(AceQLConnection.GetAceQLLocalFolder());

            AceQLTransaction transaction = await connection.BeginTransactionAsync();
            await transaction.CommitAsync();
            transaction.Dispose();

            SqlDeleteTest sqlDeleteTest = new SqlDeleteTest(connection);
            await sqlDeleteTest.DeleteCustomerAll();

            for (int i = 0; i < 10; i++)
            {
                string sql =
                "insert into customer values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8)";

                AceQLCommand command = new AceQLCommand(sql, connection);

                int customer_id = i;

                command.Parameters.AddWithValue("@parm1", customer_id);
                command.Parameters.AddWithValue("@parm2", "Sir");
                command.Parameters.AddWithValue("@parm3", AceQLTestParms.FIRSTNAME + customer_id);
                command.Parameters.Add(new AceQLParameter("@parm4", "Name_" + customer_id));
                command.Parameters.AddWithValue("@parm5", customer_id + "_row_2");
                command.Parameters.AddWithValue("@parm6", customer_id + "_row_count");
                command.Parameters.AddWithValue("@parm7", customer_id + "1111");
                command.Parameters.Add(new AceQLParameter("@parm8", new AceQLNullValue(AceQLNullType.VARCHAR))); //null value for NULL SQL insert.

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                await command.ExecuteNonQueryAsync(cancellationTokenSource.Token);
                command.Dispose();
            }

            

            SqlSelectTest sqlSelectTest = new SqlSelectTest(connection);
            await sqlSelectTest.SelectCustomerExecute();

            AceQLConsole.WriteLine("Done.");

        }

    }
}
