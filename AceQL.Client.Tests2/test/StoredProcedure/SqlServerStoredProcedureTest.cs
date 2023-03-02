/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2023,  KawanSoft SAS
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
using AceQL.Client.Api.Metadata;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AceQL.Client.Test.StoredProcedure
{
    /// <summary>
    /// This example: 
    /// 1) Inserts a Customer and an Order on a remote database. 
    /// 2) Displays the inserted raws on the console with two SELECT executed on the remote database.
    /// </summary>
    public class SqlServerStoredProcedureTest
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
                using (AceQLConnection theConnection = await ConnectionCreator.ConnectionCreateSqlServerAsync().ConfigureAwait(false))
                {
                    SqlServerStoredProcedureTest sqlServerStoredProcedureTest = new SqlServerStoredProcedureTest(
                        theConnection);
                    AceQLConsole.WriteLine("Connection created....");

                    await sqlServerStoredProcedureTest.CallStoredProcedure().ConfigureAwait(false);
                    await theConnection.CloseAsync();
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
        public SqlServerStoredProcedureTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Example of MS SQL Server Stored Procedure.
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task CallStoredProcedure()
        {
            RemoteDatabaseMetaData remoteDatabaseMetaData = ((AceQLConnection)connection).GetRemoteDatabaseMetaData();
            JdbcDatabaseMetaData jdbcDatabaseMetaData = await remoteDatabaseMetaData.GetJdbcDatabaseMetaDataAsync();
            String databaseProductName = jdbcDatabaseMetaData.GetDatabaseProductName;
            AceQLConsole.WriteLine(databaseProductName);

            if (!databaseProductName.Contains("Microsoft") && !databaseProductName.Contains("SQL Server"))
            {
                AceQLConsole.WriteLine("SqlServerStoredProcedureTest must be called with a remote SQL Server database");
                return;
            }

            string sql = "{call MY_STORED_PROCEDURE(@parm1, @parm2, @parm3)}";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            AceQLParameter aceQLParameter2 = new AceQLParameter("@parm2", 2)
            {
                Direction = ParameterDirection.InputOutput
            };

            AceQLParameter aceQLParameter1 = new AceQLParameter("@parm1", 0);

            AceQLParameter aceQLParameter3 = new AceQLParameter("@parm3")
            {
                Direction = ParameterDirection.Output
            };

            command.Parameters.Add(aceQLParameter1);
            command.Parameters.Add(aceQLParameter2);
            command.Parameters.Add(aceQLParameter3);

            AceQLConsole.WriteLine(sql);
            AceQLConsole.WriteLine("BEFORE execute @parm1: " + aceQLParameter1.ParameterName + " / " + aceQLParameter1.Value + " (" + aceQLParameter2.Value.GetType() + ")");
            AceQLConsole.WriteLine("BEFORE execute @parm2: " + aceQLParameter2.ParameterName + " / " + aceQLParameter2.Value + " (" + aceQLParameter2.Value.GetType() + ")");
            AceQLConsole.WriteLine("BEFORE execute @parm3: " + aceQLParameter3.ParameterName + " / " + aceQLParameter3.Value);
            AceQLConsole.WriteLine();

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    int i = 2;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i));
                }
            }
            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("AFTER execute @parm2: " + aceQLParameter2.ParameterName + " / " + aceQLParameter2.Value + " (" + aceQLParameter2.Value.GetType() + ")");
            AceQLConsole.WriteLine("AFTER execute @parm3: " + aceQLParameter3.ParameterName + " / " + aceQLParameter3.Value);

        }
     
    }
}
