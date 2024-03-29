﻿/*
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
    public class SqlServerStoredProcedureTestUtf8
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
                using (AceQLConnection theConnection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
                {
                    SqlServerStoredProcedureTestUtf8 sqlServerStoredProcedureTestUtf8 = new SqlServerStoredProcedureTestUtf8(
                        theConnection);
                    AceQLConsole.WriteLine("Connection created....");

                    await sqlServerStoredProcedureTestUtf8.CallStoredProcedure().ConfigureAwait(false);
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
        public SqlServerStoredProcedureTestUtf8(AceQLConnection connection)
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
                AceQLConsole.WriteLine("SqlServerStoredProcedureTestUtf8 must be called with a remote SQL Server database");
                return;
            }

            await deleteTest1(connection);

            string sql = "{call spAddNvarchar(@parm1)}";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            String parm1 = "Name";
            AceQLParameter aceQLParameter1 = new AceQLParameter("@parm1", parm1)
            {
                Direction = ParameterDirection.Input
            };

            command.Parameters.Add(aceQLParameter1);

            AceQLConsole.WriteLine(sql);
            AceQLConsole.WriteLine("BEFORE execute @parm1: " + aceQLParameter1.ParameterName + " / " + aceQLParameter1.Value + " (" + aceQLParameter1.Value.GetType() + ")");
            AceQLConsole.WriteLine();
            await command.ExecuteNonQueryAsync();
            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("Done!");

        }

        private async Task deleteTest1(AceQLConnection connection)
        {
            AceQLConsole.WriteLine("Before delete from test1");
            string sql = "delete from test1";
            AceQLCommand command = new AceQLCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
