/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
 * (http://www.kawansoft.com). All rights reserved.                                
 *                                                                               
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this filePath except in compliance with the License.
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
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Connection
{
    /// <summary>
    /// Class ConnectionCreator.
    /// </summary>
    public class ConnectionCreator
    {
        /// <summary>
        /// RemoteConnection Quick Start client example.
        /// Creates a Connection to a remote database and open it.
        /// </summary>
        /// <returns>The connection to the remote database</returns>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public static async Task<AceQLConnection> ConnectionCreateAsync()
        {
            string connectionString = ConnectionStringCurrent.Build();

            AceQLConnection theConnection = new AceQLConnection(connectionString);

            // Opens the connection with the remote database.
            // On the server side, a JDBC connection is extracted from the connection 
            // pool created by the server at startup. The connection will remain ours 
            // during the session.
            await theConnection.OpenAsync();

            return theConnection;
        }

        /// <summary>
        /// RemoteConnection Quick Start client example.
        /// Creates a Connection to a MySQL database and open it.
        /// </summary>
        /// <returns>The connection to the remote database</returns>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public static async Task<AceQLConnection> ConnectionCreateMySqlAsync()
        {
            string connectionString = ConnectionStringBuilderFactory.CreateLocalMySql();

            AceQLConnection theConnection = new AceQLConnection(connectionString);

            // Opens the connection with the remote database.
            // On the server side, a JDBC connection is extracted from the connection 
            // pool created by the server at startup. The connection will remain ours 
            // during the session.
            await theConnection.OpenAsync();

            return theConnection;
        }

        /// <summary>
        /// RemoteConnection Quick Start client example.
        /// Creates a Connection to a SQL Server database and open it.
        /// </summary>
        /// <returns>The connection to the remote database</returns>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public static async Task<AceQLConnection> ConnectionCreateSqlServerAsync()
        {
            string connectionString = ConnectionStringBuilderFactory.CreateLocalSqlServer();

            AceQLConnection theConnection = new AceQLConnection(connectionString);

            // Opens the connection with the remote database.
            // On the server side, a JDBC connection is extracted from the connection 
            // pool created by the server at startup. The connection will remain ours 
            // during the session.
            await theConnection.OpenAsync();

            return theConnection;
        }

        /// <summary>
        /// RemoteConnection Quick Start client example.
        /// Creates a Connection to an Oracle database and open it.
        /// </summary>
        /// <returns>The connection to the remote database</returns>
        /// <exception cref="AceQLException">If any Exception occurs.</exception>
        public static async Task<AceQLConnection> ConnectionCreateOracleDatabaseAsync()
        {
            string connectionString = ConnectionStringBuilderFactory.CreateLocalOracleDatabase();

            AceQLConnection theConnection = new AceQLConnection(connectionString);

            // Opens the connection with the remote database.
            // On the server side, a JDBC connection is extracted from the connection 
            // pool created by the server at startup. The connection will remain ours 
            // during the session.
            await theConnection.OpenAsync();

            return theConnection;
        }
    }
}
