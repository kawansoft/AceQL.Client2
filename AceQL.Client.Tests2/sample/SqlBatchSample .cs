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

using AceQL.Client;
using AceQL.Client.Api;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Sample
{
    /// <summary>
    /// This example: 
    /// 1) Inserts a Customer and an Order on a remote database. 
    /// 2) Displays the inserted raws on the console with two SELECT executed on the remote database.
    /// </summary>
    public class SqlBatchSample
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
                    SqlBatchSample sqlBatchTest = new SqlBatchSample(
                        connection);
                    AceQLConsole.WriteLine("Connection created....");

                    SqlDeleteTest sqlDeleteTest = new SqlDeleteTest(connection);
                    await sqlDeleteTest.DeleteCustomerAll();
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
        public SqlBatchSample(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Example of MS SQL Server Stored Procedure.
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task InsertUsingBatch()
        {
            string sql = "insert into customer values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8)";
            AceQLCommand command = new AceQLCommand(sql, connection);

            // We do the INSERTs in a transaction because it's faster:
            AceQLTransaction transaction = await connection.BeginTransactionAsync();

            try
            {
                // Add first set of parameters
                command.Parameters.AddWithValue("@parm1", 1);
                command.Parameters.AddWithValue("@parm2", "Sir");
                command.Parameters.AddWithValue("@parm3", "John");
                command.Parameters.AddWithValue("@parm4", "Smith");
                command.Parameters.AddWithValue("@parm5", "1 U.S. Rte 66");
                command.Parameters.AddWithValue("@parm6", "Hydro");
                command.Parameters.AddWithValue("@parm7", "OK 730482");
                command.Parameters.AddWithValue("@parm8", "(405) 297 - 2391");
                command.AddBatch();

                // Add a second set of parameters
                command.Parameters.AddWithValue("@parm1", 2);
                command.Parameters.AddWithValue("@parm2", "Miss");
                command.Parameters.AddWithValue("@parm3", "Melanie");
                command.Parameters.AddWithValue("@parm4", "Jones");
                command.Parameters.AddWithValue("@parm5", "1000 U.S. Rte 66");
                command.Parameters.AddWithValue("@parm6", "Sayre");
                command.Parameters.AddWithValue("@parm7", "OK 73662");
                command.Parameters.AddWithValue("@parm8", "(405) 299 - 3359");
                command.AddBatch();

                // Executes the batch. All INSERT orders are uploaded at once:
                int[] results = await command.ExecuteBatchAsync();
                await transaction.CommitAsync();

                // Do whatever with the results...
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}
