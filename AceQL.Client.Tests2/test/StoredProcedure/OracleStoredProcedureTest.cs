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
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AceQL.Client.Test.StoredProcedure
{
    /// <summary>
    ///  This class shows how to use AceQL C# Client SDK in order to call
    /// Oracle stored procedures.
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

                    await oracleStoredProcedureTest.StoredProcedureOracleSelectCustomer().ConfigureAwait(false);
                    await oracleStoredProcedureTest.StoredProcedureOracleInOut().ConfigureAwait(false);
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
        /// Example of Oracle Stored Procedure with a SELECT call
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task StoredProcedureOracleSelectCustomer()
        {
            /**
            --
            -- Oracle stored procedure sample with a SELECT.
            -- Returns all customer_id from customer table 
            -- that are > to the input value p_customer_id
            -- 
            create or replace PROCEDURE ORACLE_SELECT_CUSTOMER 
                (p_customer_id NUMBER, p_rc OUT sys_refcursor) AS 
            BEGIN
                OPEN p_rc
                For select customer_id from customer where customer_id > p_customer_id;
            END ORACLE_SELECT_CUSTOMER;
             */

            string sql = "{ call ORACLE_SELECT_CUSTOMER(@parm1, ?) }";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            AceQLParameter aceQLParameter1 = new AceQLParameter("@parm1", 2);

            command.Parameters.Add(aceQLParameter1);

            AceQLConsole.WriteLine(sql);
            AceQLConsole.WriteLine("BEFORE execute @parm1: " + aceQLParameter1.ParameterName + " / " + aceQLParameter1.Value);
            AceQLConsole.WriteLine();

            using AceQLDataReader dataReader = await command.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                int i = 0;
                AceQLConsole.WriteLine("Customer ID: " + dataReader.GetValue(i));
            }
            AceQLConsole.WriteLine("Done StoredProcedureOracleSelectCustomer!");
            AceQLConsole.WriteLine();
        }

        /// <summary>
        /// Example of Oracle Stored Procedure with IN & OUT parameters
        /// </summary>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public async Task StoredProcedureOracleInOut()
        {

            /**
            --
            -- Oracle stored procedure sample with a compute.
            -- Returns in OUT param2 the sum of IN param1 and IN/OU param2
            -- 
            create or replace PROCEDURE ORACLE_IN_OUT 
            (
              PARAM1 IN NUMBER 
            , PARAM2 IN OUT NUMBER 
            ) AS 
            BEGIN
              param2 := param1 + param2;
            END ORACLE_IN_OUT;
            */

            string sql = "{ call ORACLE_IN_OUT(@parm1, @parm2) }";

            AceQLCommand command = new AceQLCommand(sql, connection);
            command.CommandType = CommandType.StoredProcedure;

            AceQLParameter aceQLParameter1 = new AceQLParameter("@parm1", 1);

            AceQLParameter aceQLParameter2 = new AceQLParameter("@parm2", 2)
            {
                Direction = ParameterDirection.InputOutput
            };

            command.Parameters.Add(aceQLParameter1);
            command.Parameters.Add(aceQLParameter2);

            AceQLConsole.WriteLine(sql);
            AceQLConsole.WriteLine("BEFORE execute @parm1: " + aceQLParameter1.ParameterName + " / " + aceQLParameter1.Value);
            AceQLConsole.WriteLine("BEFORE execute @parm2: " + aceQLParameter2.ParameterName + " / " + aceQLParameter2.Value);
            AceQLConsole.WriteLine();

            await command.ExecuteNonQueryAsync();

            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("AFTER execute @parm2: " + aceQLParameter2.ParameterName + " / " + aceQLParameter2.Value + " (" + aceQLParameter2.Value.GetType() + ")");

            AceQLConsole.WriteLine("Done StoredProcedureOracleInOut!");
            AceQLConsole.WriteLine();

        }

    }
}
