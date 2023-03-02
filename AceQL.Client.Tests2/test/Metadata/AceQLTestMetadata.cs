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

using AceQL.Client.Api;
using AceQL.Client.Api.Metadata;
using AceQL.Client.Api.Metadata.Dto;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Metadata
{
    /// <summary>
    /// Tests AceQL client SDK by calling all APIs.
    /// </summary>
    static class AceQLTestMetadata
    {

        public static void TheMain()
        {
            try
            {
                AceQLConsole.WriteLine("AceQLTestMetadata Begin...");
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


        public static async Task DoIt()
        {

            // Make sure connection is always closed to close and release server connection into the pool
            using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
            {
                await ExecuteExample(connection).ConfigureAwait(false);
                await connection.CloseAsync();
            }
        }

        /// <summary>
        /// Executes our example using an <see cref="AceQLConnection"/> 
        /// </summary>
        /// <param name="connection"></param>
        public static async Task ExecuteExample(AceQLConnection connection)
        {
            RemoteDatabaseMetaData remoteDatabaseMetaData = connection.GetRemoteDatabaseMetaData();

            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string schemaFilePath = userPath + "\\db_schema.out.html";

            // Download Schema in HTML format:
            using (Stream stream = await remoteDatabaseMetaData.DbSchemaDownloadAsync())
            {
                using (var fileStream = File.Create(schemaFilePath))
                {
                    stream.CopyTo(fileStream);
                }
            }

            bool displaySchema = false;

            if (displaySchema)
            {
                FileInfo fileInfo = new FileInfo(schemaFilePath);
                if (fileInfo.Exists)
                {
                    System.Diagnostics.Process.Start(schemaFilePath);
                }
            }

            AceQLConsole.WriteLine("Creating schema done.");

            JdbcDatabaseMetaData jdbcDatabaseMetaData = await remoteDatabaseMetaData.GetJdbcDatabaseMetaDataAsync().ConfigureAwait(true);
            AceQLConsole.WriteLine("Major Version: " + jdbcDatabaseMetaData.GetJDBCMajorVersion);
            AceQLConsole.WriteLine("Minor Version: " + jdbcDatabaseMetaData.GetJDBCMinorVersion);
            AceQLConsole.WriteLine("IsReadOnly   : " + jdbcDatabaseMetaData.IsReadOnly);

            AceQLConsole.WriteLine("JdbcDatabaseMetaData: " + jdbcDatabaseMetaData.ToString().Substring(1, 200));
            AceQLConsole.WriteLine();

            AceQLConsole.WriteLine("Get the table names:");
            List<String> tableNames = await remoteDatabaseMetaData.GetTableNamesAsync();

            AceQLConsole.WriteLine("Print the column details of each table:");
            foreach (String tableName in tableNames)
            {
                Table table = await remoteDatabaseMetaData.GetTableAsync(tableName);

                AceQLConsole.WriteLine("Columns:");
                foreach(Column column in table.Columns)
                {
                    AceQLConsole.WriteLine(column.ToString());
                }
            }

            AceQLConsole.WriteLine();

            String name = "orderlog";
            Table tableOrderlog = await remoteDatabaseMetaData.GetTableAsync(name);

            AceQLConsole.WriteLine("table name: " + tableOrderlog.TableName);
            AceQLConsole.WriteLine("table keys: ");
            List<PrimaryKey> primakeys = tableOrderlog.PrimaryKeys;
            foreach (PrimaryKey primaryKey in primakeys)
            {
                AceQLConsole.WriteLine("==> primaryKey: " + primaryKey);
            }
            AceQLConsole.WriteLine();

            AceQLConsole.WriteLine("Full table: " + tableOrderlog);

            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("Done.");

        }

    }
}
