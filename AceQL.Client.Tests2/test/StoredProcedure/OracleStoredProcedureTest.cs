﻿/*
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
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AceQL.Client.Test.StoredProcedure
{
    /// <summary>
    /// This example calls an Oracle stored procedure that returns a SELECT result.
    /// </summary>
    public class OracleStoredProcedureTest
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
                using (AceQLConnection theConnection
                    = await ConnectionCreator.ConnectionCreateOracleDatabaseAsync().ConfigureAwait(false))
                {
                    OracleStoredProcedureTest oracleStoredProcedureTest = new OracleStoredProcedureTest(
                        theConnection);
                    AceQLConsole.WriteLine("Connection created....");

                    await oracleStoredProcedureTest.CallStoredProcedure().ConfigureAwait(false);
                    await theConnection.CloseAsync();
                    AceQLConsole.WriteLine("The end...");
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
        public OracleStoredProcedureTest(AceQLConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Example of MySQL Stored Procedure.
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task CallStoredProcedure()
        {
            string sql = "{ call PROCEDURE2(@parm1, ?) }";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            AceQLParameter aceQLParameter1 = new AceQLParameter("@parm1", 2);

            command.Parameters.Add(aceQLParameter1);

            AceQLConsole.WriteLine(sql);
            AceQLConsole.WriteLine("BEFORE execute @parm1: " + aceQLParameter1.ParameterName + " / " + aceQLParameter1.Value);
            AceQLConsole.WriteLine();

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i));
                }
            }
        }
     
    }
}