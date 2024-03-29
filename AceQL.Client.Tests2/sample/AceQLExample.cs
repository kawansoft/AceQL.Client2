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

using AceQL.Client.Api;
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using static System.String;

namespace AceQL.Client.Sample
{
    /// <summary>
    /// Tests AceQL client SDK by calling all APIs.
    /// </summary>
    class AceQLExample
    {
        private const string ACEQL_PCL_FOLDER = "AceQLPclFolder";

        public const string serverUrlLocalhost = "http://localhost:9090/aceql";
        public const string serverUrlLocalhostSsl = "https://localhost:9443/aceql";
        public const string serverUrlLocalhostTomcat = "http://localhost:8080/aceql-test/aceql";
        public const string serverUrlLinux = "https://www.aceql.com:9443/aceql";
        public const string serverUrlLinux2 = "http://www.aceql.com:9090/aceql";

        public const string database_sampledb = "sampledb";
        public const string database_sampledb_2 = "sampledb_2";

        public static string server = serverUrlLocalhost;
        public static string database = database_sampledb;

        private static bool CONSOLE_INPUT_DONE = false;

        public static void TheMain(string[] args)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\aceql_csharp_init.txt";

                string databaseToUse = database_sampledb;
                if (File.Exists(path))
                {
                    databaseToUse = File.ReadAllText(path);
                }

                AceQLConsole.WriteLine("path         : " + path + ":");
                AceQLConsole.WriteLine("databaseToUse: " + databaseToUse + ":");
                database = databaseToUse;

                bool doContinue = true;
                while (doContinue)
                {
                    DoIt(args).Wait();
                    doContinue = true;
                }

                //DoIt(args).GetAwaiter().GetResult();

                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue...");
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


        static async Task DoIt(string[] args)
        {

            string username = "username";
            string password = "password";

            //customer_id integer NOT NULL,
            //customer_title character(4),
            //fname character varying(32),
            //lname character varying(32) NOT NULL,
            //addressline character varying(64),
            //town character varying(32),
            //zipscode character(10) NOT NULL,
            //phone character varying(32),

            string connectionString = null;
            bool useProxy = false;

            if (useProxy)
            {
                string proxyUsername = "ndepomereu2";
                string proxyPassword = "";
                connectionString = $"Server={server}; Database={database}; ProxyUsername={proxyUsername}; ProxyPassword= {proxyPassword}";
            }
            else
            {
                connectionString = $"Server={server}; Database={database}";
            }

            AceQLCredential credential = new AceQLCredential(username, password.ToCharArray());

            // Make sure connection is always closed to close and release server connection into the pool
            using (AceQLConnection connection = new AceQLConnection(connectionString))
            {
                //connection.SetTraceOn(true);

                connection.Credential = credential;
                await ExecuteExample(connection).ConfigureAwait(false);
                await connection.CloseAsync();

                AceQLConnection connection2 = new AceQLConnection(connectionString)
                {
                    Credential = credential
                };

                await connection2.OpenAsync();
                AceQLConsole.WriteLine("connection2.GetServerVersion(): " + await connection2.GetServerVersionAsync());

                await connection2.LogoutAsync().ConfigureAwait(false);

            }

        }

        /// <summary>
        /// Executes our example using an <see cref="AceQLConnection"/> 
        /// </summary>
        /// <param name="connection"></param>
        private static async Task ExecuteExample(AceQLConnection connection)
        {
            string IN_DIRECTORY = AceQLConnection.GetAceQLLocalFolder() + "\\";
            string OUT_DIRECTORY = IN_DIRECTORY + "out\\";
            _ = Directory.CreateDirectory(OUT_DIRECTORY);

            await connection.OpenAsync();

            AceQLConsole.WriteLine("ConnectionString: " + connection.ConnectionInfo.ConnectionString);
            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("AceQLConnection.GetClientVersion(): " + AceQLConnection.GetClientVersion());
            AceQLConsole.WriteLine("AceQLConnection.GetServerVersion(): " + await connection.GetServerVersionAsync());
            AceQLConsole.WriteLine("AceQL local folder: ");
            AceQLConsole.WriteLine(AceQLConnection.GetAceQLLocalFolder());

            if (!CONSOLE_INPUT_DONE)
            {
                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue....");
                Console.ReadLine();
                CONSOLE_INPUT_DONE = true;
            }

            AceQLTransaction transaction = await connection.BeginTransactionAsync();
            await transaction.CommitAsync();
            transaction.Dispose();

            string sql = "delete from customer";

            AceQLCommand command = null;
            command = new AceQLCommand()
            {
                CommandText = sql,
                Connection = connection
            };
            command.Prepare();

            await command.ExecuteNonQueryAsync();

            sql = "delete from dustomer";

            command = new AceQLCommand()
            {
                CommandText = sql,
                Connection = connection
            };
            command.Prepare();

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
            }

            sql = "delete from dustomer where customer_id = @parm1 or fname = @parm2  ";
            command = new AceQLCommand()
            {
                CommandText = sql,
                Connection = connection
            };
            command.Parameters.AddWithValue("@parm1", 1);
            command.Parameters.AddWithValue("@parm2", "Doe");

            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
            }



            for (int i = 0; i < 3; i++)
            {
                sql =
                "insert into customer values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8)";

                command = new AceQLCommand(sql, connection);

                int customer_id = i;

                command.Parameters.AddWithValue("@parm1", customer_id);
                command.Parameters.AddWithValue("@parm2", "Sir");
                command.Parameters.AddWithValue("@parm3", AceQLTestParms.FIRSTNAME + customer_id);
                command.Parameters.Add(new AceQLParameter("@parm4", "Name_" + customer_id));
                command.Parameters.AddWithValue("@parm5", customer_id + ", road 66");
                command.Parameters.AddWithValue("@parm6", "Town_" + customer_id);
                command.Parameters.AddWithValue("@parm7", customer_id + "1111");
                command.Parameters.Add(new AceQLParameter("@parm8", new AceQLNullValue(AceQLNullType.VARCHAR))); //null value for NULL SQL insert.

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                await command.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }

            command.Dispose();

            sql = "select * from customer";
            command = new AceQLCommand(sql, connection);

            // Our dataReader must be disposed to delete underlying downloaded files
            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                //await dataReader.ReadAsync(new CancellationTokenSource().Token)
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    int i = 0;
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++));
                }
            }

            AceQLConsole.WriteLine("Before delete from orderlog 2");

            // Do next delete in a transaction because of BLOB
            transaction = await connection.BeginTransactionAsync();

            sql = "delete from orderlog";
            command = new AceQLCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
            command.Dispose();

            AceQLConsole.WriteLine("After delete from orderlog 2");

            await transaction.CommitAsync();

            Boolean doBlob = true;
            if (!doBlob)
            {
                return;
            }

            // Do next inserts in a transaction because of BLOB
            transaction = await connection.BeginTransactionAsync();

            try
            {
                for (int j = 1; j < 4; j++)
                {
                    sql =
                    "insert into orderlog values (@parm1, @parm2, @parm3, @parm4, @parm5, @parm6, @parm7, @parm8, @parm9)";

                    command = new AceQLCommand(sql, connection);

                    int customer_id = j;

                    string blobPath = null;

                    int index = getIndexFromDatabase();
                    blobPath = IN_DIRECTORY + "username_koala_" + index + ".jpg";

                    Stream stream = new FileStream(blobPath, FileMode.Open, System.IO.FileAccess.Read);

                    //customer_id integer NOT NULL,
                    //item_id integer NOT NULL,
                    //description character varying(64) NOT NULL,
                    //cost_price numeric,
                    //date_placed date NOT NULL,
                    //date_shipped timestamp without time zone,
                    //jpeg_image oid,
                    //is_delivered numeric,
                    //quantity integer NOT NULL,

                    command.Parameters.AddWithValue("@parm1", customer_id);
                    command.Parameters.AddWithValue("@parm2", customer_id);
                    command.Parameters.AddWithValue("@parm3", "Description_" + customer_id);
                    command.Parameters.Add(new AceQLParameter("@parm4", new AceQLNullValue(AceQLNullType.DECIMAL))); //null value for NULL SQL insert.
                    command.Parameters.AddWithValue("@parm5", DateTime.Now);
                    command.Parameters.AddWithValue("@parm6", DateTime.Now);
                    // Adds the Blob. (Stream will be closed by AceQLCommand)

                    command.Parameters.AddWithValue("@parm7", stream);

                    command.Parameters.AddWithValue("@parm8", 1);
                    command.Parameters.AddWithValue("@parm9", j * 2000);

                    AceQLConsole.WriteLine("Before await command.ExecuteNonQueryAsync()");
                    await command.ExecuteNonQueryAsync();
                    AceQLConsole.WriteLine("After await command.ExecuteNonQueryAsync()");
                }

                AceQLConsole.WriteLine("transaction.CommitAsync()");
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

            sql = "select * from orderlog";
            command = new AceQLCommand(sql, connection);

            using (AceQLDataReader dataReader = await command.ExecuteReaderAsync())
            {
                int k = 0;
                while (dataReader.Read())
                {
                    AceQLConsole.WriteLine();
                    int i = 0;
                    AceQLConsole.WriteLine("Get values using ordinal values:");
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++) + "\n"
                        + "GetValue: " + dataReader.GetValue(i++));

                    //customer_id
                    //item_id
                    //description
                    //item_cost
                    //date_placed
                    //date_shipped
                    //jpeg_image
                    //is_delivered
                    //quantity

                    AceQLConsole.WriteLine();
                    AceQLConsole.WriteLine("Get values using column name values:");
                    AceQLConsole.WriteLine("GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("customer_id"))
                        + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("item_id")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("description")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("item_cost")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("date_placed")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("date_shipped")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("jpeg_image")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("is_delivered")) + "\n"
                        + "GetValue: " + dataReader.GetValue(dataReader.GetOrdinal("quantity")));

                    AceQLConsole.WriteLine("==> dataReader.IsDBNull(3): " + dataReader.IsDBNull(3));
                    AceQLConsole.WriteLine("==> dataReader.IsDBNull(4): " + dataReader.IsDBNull(4));

                    // Download Blobs
                    int index = getIndexFromDatabase();
                    string blobPath = OUT_DIRECTORY + "username_koala_" + index + "_" + k + ".jpg";
                    k++;

                    using (Stream stream = await dataReader.GetStreamAsync(6))
                    {
                        using (var fileStream = File.Create(blobPath))
                        {
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }

            await transaction.CommitAsync();
        }

        private static int getIndexFromDatabase()
        {
            if (database.Equals(database_sampledb))
            {
                return 1;
            }
            else if (database.Equals(database_sampledb_2))
            {
                return 2;
            }
            else
            {
                throw new NotImplementedException("No index for database: " + database);
            }

        }
    }
}
