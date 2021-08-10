﻿/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2021,  KawanSoft SAS
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
using AceQL.Client.Api.Http;
using AceQL.Client.Tests.Test;
using AceQL.Client.Tests.Test.Connection;
using AceQL.Client.Tests.tests.Dml;
using AceQL.Client.Tests.tests.Dml.BLOB;
using AceQL.Client.Tests.Util;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Tests
{
    /// <summary>
    /// Tests AceQL client SDK by calling all APIs.
    /// </summary>
    public static class AceQLTest
    {
        public static bool DO_LOOP;

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

            string connectionString = ConnectionStringCurrent.Build();

            using (AceQLConnection connection = new AceQLConnection(connectionString))
            {
                connection.RequestRetry = true;
                connection.AddRequestHeader("aceqlHeader1", "myAceQLHeader1");
                connection.AddRequestHeader("aceqlHeader2", "myAceQLHeader2");

                await connection.OpenAsync();

                if (DO_LOOP)
                {
                    while (true)
                    {
                        await ExecuteExample(connection).ConfigureAwait(false);
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    await ExecuteExample(connection).ConfigureAwait(false);
                }


                await connection.CloseAsync();
            }
        }


        /// <summary>
        /// Executes our example using an <see cref="AceQLConnection"/> 
        /// </summary>
        /// <param name="connection"></param>
        public static async Task ExecuteExample(AceQLConnection connection)
        {

            _ = Directory.CreateDirectory(AceQLTestParms.OUT_DIRECTORY);

            AceQLConsole.WriteLine("host: " + connection.ConnectionInfo.ConnectionString);
            AceQLConsole.WriteLine("aceQLConnection.GetClientVersion(): " + AceQLConnection.GetClientVersion());
            AceQLConsole.WriteLine("aceQLConnection.GetServerVersion(): " + await connection.GetServerVersionAsync());
            AceQLConsole.WriteLine("AceQL local folder: ");
            AceQLConsole.WriteLine(AceQLConnection.GetAceQLLocalFolder());
            AceQLConsole.WriteLine("ConnectionInfo: " + connection.ConnectionInfo);

            AceQLConsole.WriteLine("Press enter to continue....");
            Console.ReadLine();

            //AceQLTransaction transaction = await connection.BeginTransactionAsync();
            //await transaction.CommitAsync();
            //transaction.Dispose();

            AceQLTransaction transaction = null;

            SqlDeleteTest sqlDeleteTest = new SqlDeleteTest(connection);
            await sqlDeleteTest.DeleteCustomerAll();

            SqlInsertTest sqlInsertTest;
            for (int i = 0; i < 100; i++)
            {
                sqlInsertTest = new SqlInsertTest(connection);
                int rows = await sqlInsertTest.InsertCustomer(i); 
            }

            SqlSelectTest sqlSelectTest;
            int maxSelect = 10;
            for (int j = 0; j < maxSelect; j++)
            {
                sqlSelectTest = new SqlSelectTest(connection);
                await sqlSelectTest.SelectCustomerExecute();
            }

            sqlSelectTest = new SqlSelectTest(connection);
            int maxCustomerId = await sqlSelectTest.SelectMaxCustomers();
            AceQLConsole.WriteLine("maxCustomerId: " + maxCustomerId);
            AceQLConsole.WriteLine("Press enter to continue....");
            Console.ReadLine();

            AceQLConsole.WriteLine("Before delete from orderlog");

            // Do next delete in a transaction because of BLOB
            sqlDeleteTest = new SqlDeleteTest(connection);
            await sqlDeleteTest.DeleteOrderlogAll();

            transaction = await connection.BeginTransactionAsync();

            AceQLConsole.WriteLine("Before insert into orderlog");
            
            try
            {
                string blobPath = AceQLTestParms.IN_DIRECTORY + "username_koala.jpg";
                for (int j = 1; j < 4; j++)
                {
                    SqlInsertBlobTest sqlInsertBlobTest  = new SqlInsertBlobTest(connection);
                    await sqlInsertBlobTest.InsertOrderlog(j, blobPath);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            AceQLConsole.WriteLine("Before select *  from orderlog");

            // Do next selects in a transaction because of BLOB
            transaction = await connection.BeginTransactionAsync();

            SqlSelectBlobTest sqlSelectBlobTest = new SqlSelectBlobTest(connection);
            await sqlSelectBlobTest.SelectAndStoreBlob();

            await transaction.CommitAsync();
        }

    }
}
