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

using AceQL.Client.Api.Http;
using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api
{
    /// <summary>
    /// Class HealthCheck. Allows testing if the remote AceQL servlet is accessible with a ping, and to
    /// check database access response time.
    /// </summary>
    public class HealthCheck
    {
        private readonly AceQLConnection connection;
        private AceQLException aceQLException;

        internal SimpleTracer simpleTracer = new SimpleTracer();

        /// <summary>
        /// Returns the AceQLException thrown if a <c>Ping()</c> returns false
        /// </summary>
        /// <value>The AceQLException thrown if a <c>Ping()</c> returns false.</value>
        public AceQLException AceQLException { get => aceQLException; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthCheck"/> class.
        /// </summary>
        /// <param name="connection">The AceQL connection.</param>
        /// <exception cref="System.ArgumentNullException">connection is null!</exception>
        public HealthCheck(AceQLConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException("connection is null!");
        }

        /// <summary>
        /// Pings the AceQL server servlet.
        /// </summary>
        /// <returns><c>true</c> if the the AceQL server main servlet is pingable, else <c>false</c>.</returns>
        public async Task<bool> PingAsync()
        {
            AceQLHttpApi aceQLHttpApi = connection.aceQLHttpApi;
            HttpManager httpManager = aceQLHttpApi.httpManager;

            string url = connection.ConnectionInfo.Server;

            try
            {
                string result = await httpManager.CallWithGetAsync(url).ConfigureAwait(false);

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, httpManager.HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        httpManager.HttpStatusCode);
                }

                return true;

            }
            catch (Exception exception)
            {
                simpleTracer.Trace(exception.ToString());

                if (exception.GetType() == typeof(AceQLException))
                {
                    this.aceQLException = (AceQLException)exception;
                }
                else
                {
                    throw new AceQLException(exception.Message, 0, exception, httpManager.HttpStatusCode);
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the response time of a SQL statement called on the remote database defined 
        /// by the underlying <c>connection</c> (must be a SELECT).
        /// </summary>
        /// <param name="sql">The SQL SELECT command.</param>
        /// <returns>The response time in milliseconds</returns>
        public async Task<double> GetResponseTimeAsync(string sql)
        {
            if (sql == null)
            {
                throw new ArgumentNullException(nameof(sql));
            }

            if (!sql.ToLower().StartsWith("select"))
            {
                throw new ArgumentException("sql command must be a SELECT!");
            }

            DateTime begin = DateTime.Now;
            using (AceQLCommand command = new AceQLCommand(sql, connection))
            {
                using (AceQLDataReader dataReader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    // Nothing to do with result, we want only the response time
                }
            }

            DateTime end = DateTime.Now;
            TimeSpan timeSpan = end - begin;
            return timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Gets the response time of a "select 1" SQL statement called on the remote database defined 
        /// by the underlying <c>connection</c>.
        /// </summary>
        /// <returns>The response time in milliseconds</returns>
        public async Task<double> GetResponseTimeAsync()
        {
            return await GetResponseTimeAsync("select 1").ConfigureAwait(false);
        }

    }
}
