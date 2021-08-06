/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2020,  KawanSoft SAS
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

using AceQL.Client;
using AceQL.Client.Api;
using AceQL.Client.Src.Api;
using AceQL.Client.Tests.Test;
using AceQL.Client.Tests.Test.Connection;
using AceQL.Client.Tests.Util;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Tests.DML.Batchs
{
    /// <summary>
    /// This example: 
    /// 1) Inserts a Customer and an Order on a remote database. 
    /// 2) Displays the inserted raws on the console with two SELECT executed on the remote database.
    /// </summary>
    public class SqlBatchTest
    {
        /// <summary>
        /// The connection to the remote database
        /// </summary>
        readonly AceQLConnection connection;

        public static void TheMain(string[] args)
        {
            DoIt(args).Wait();
        }

        /// <summary>
        /// Does it.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static async Task DoIt(string[] args)
        {

            try
            {
                // Make sure connection is always closed in order to close and release
                // server connection into the pool
                using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
                {
                    SqlBatchTest sqlBatchTest = new SqlBatchTest(
                        connection);
                    AceQLConsole.WriteLine("Connection created....");

                    await sqlBatchTest.DeleteCustomerAll();
                    await sqlBatchTest.InsertUsingBatch().ConfigureAwait(false);
                }

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


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connection">The AceQL connection to remote database.</param>
        public SqlBatchTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Example of MS SQL Server Stored Procedure.
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task InsertUsingBatch()
        {
            string sql ="insert into customer values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8)";
            AceQLCommand command = new AceQLCommand(sql, connection);

            for (int i = 1; i < 10; i++)
            {
                int customer_id = i;

                command.Parameters.AddWithValue("@parm1", customer_id);
                command.Parameters.AddWithValue("@parm2", "Sir" + i ); // HACK NDP
                command.Parameters.AddWithValue("@parm3", "André_" + customer_id);
                command.Parameters.Add(new AceQLParameter("@parm4", "Name_" + customer_id));
                command.Parameters.AddWithValue("@parm5", customer_id + ", road Sixty-Six");
                command.Parameters.AddWithValue("@parm6", "Town_" + customer_id);
                command.Parameters.AddWithValue("@parm7", customer_id + "0000");
                command.Parameters.Add(new AceQLParameter("@parm8", new AceQLNullValue(AceQLNullType.VARCHAR))); //null value for NULL SQL insert.

                command.AddBatch();
            }

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            int [] results = await command.ExecuteBatch(cancellationTokenSource.Token);

            foreach (int theResult in results)
            {
                AceQLConsole.WriteLine(theResult + "");
            }

        }

        /// <summary>
        /// Delete all from customers
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCustomerAll()
        {
            // Delete all
            string sql = "delete from customer where customer_id >= 0";
            AceQLCommand command = new AceQLCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
