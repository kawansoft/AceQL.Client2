/*
 * This file is part of AceQL JDBC Driver.
 * AceQL JDBC Driver: Remote JDBC access over HTTP with AceQL HTTP.
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

using AceQL.Client.src.Api;
using System;
using System.Net;

namespace AceQL.Client.Api
{
    /// <summary>
    /// Class ConnectionInfo. Contains all info about Connections. Password cannot be retrieved for the sake of security.
    /// </summary>
    public class ConnectionInfo
    {

        private readonly string connectionString;
        private readonly string server;
        private readonly string database;
        private readonly string username;
        private readonly bool isNTLM;
        private readonly string sessionId;
        private readonly string proxyUri;
        private readonly ICredentials proxyCredentials;
        private readonly bool useCredentialCache;
        private readonly int timeout;
        private readonly bool enableDefaultSystemAuthentication;
        private readonly bool enableTrace;
        private readonly bool gzipResult;

        internal DateTime creationDateTime;

        private static int maxRetries = 0;
        private static int retryIntervalMs = 0;
        
        internal ConnectionInfo(ConnectionInfoHolder connectionInfoHolder)
        {
            this.connectionString = connectionInfoHolder.ConnectionString;
            this.server = connectionInfoHolder.Server;
            this.database = connectionInfoHolder.Database;
            this.username = connectionInfoHolder.Username;
            this.isNTLM = connectionInfoHolder.IsNTLM;
            this.sessionId = connectionInfoHolder.SessionId;
            this.proxyUri = connectionInfoHolder.ProxyUri;
            this.proxyCredentials = connectionInfoHolder.ProxyCredentials;
            this.useCredentialCache = connectionInfoHolder.UseCredentialCache;
            this.timeout = connectionInfoHolder.Timeout;
            this.enableDefaultSystemAuthentication = connectionInfoHolder.EnableDefaultSystemAuthentication;
            this.gzipResult = connectionInfoHolder.GzipResult;
            this.enableTrace = connectionInfoHolder.EnableTrace;

        }


        /// <summary>
        /// Gets or the connection string used to connect to the remote database.
        /// </summary>
        /// <value>The connection string used to connect to the remote database.</value>
        public string ConnectionString => connectionString;

        /// <summary>
        /// Gets the remote server.
        /// </summary>
        /// <value>The remote server.</value>
        public string Server
        {
            get
            {
                return server;
            }
        }

        /// <summary>
        /// Gets the current remote database in use.
        /// </summary>
        /// <value>The current remote database in use.</value>
        public string Database
        {
            get
            {
                return database;
            }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get
            {
                return username;
            }
        }

        /// <summary>
        /// Says if NTLM is in use.
        /// </summary>
        /// <value><c>true</c> if NTLM is n use; otherwise, <c>false</c>.</value>
        public bool IsNTLM => isNTLM;

        /// <summary>
        /// Gets the AceQL Server Session ID.
        /// </summary>
        /// <value>The AceQL Server Session ID.</value>
        public string SessionId
        {
            get
            {
                return sessionId;
            }
        }

        /// <summary>
        /// Gets the proxy URI.
        /// </summary>
        /// <value>The proxy URI.</value>
        public string ProxyUri
        {
            get
            {
                return proxyUri;
            }
        }


        /// <summary>
        /// Gets the proxy credentials.
        /// </summary>
        /// <value>The proxy credentials.</value>
        public ICredentials ProxyCredentials
        {
            get
            {
                return proxyCredentials;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the credential cache is used.
        /// </summary>
        /// <value><c>true</c> if the credential cache is used; otherwise, <c>false</c>.</value>
        public bool UseCredentialCache
        {
            get
            {
                return useCredentialCache;
            }
        }

        /// <summary>
        /// Gets the time to wait in milliseconds while trying to establish a connection before terminating the attempt and generating an error.
        /// If value is 0, <see cref="System.Net.Http.HttpClient"/> default will value be used.
        /// </summary>
        public int Timeout
        {
            get
            {
                return timeout;
            }
        }
        /// <summary>
        /// Gets a value indicating whether default system authentication is enabled.
        /// </summary>
        /// <value><c>true</c> if default system authentication is enabled; otherwise, <c>false</c>.</value>
        public bool EnableDefaultSystemAuthentication
        {
            get
            {
                return enableDefaultSystemAuthentication;
            }
        }

        /// <summary>
        /// Gets the value indicating whether SQL result sets are returned compressed with the GZIP filePath format
        /// before download. Defaults to <c>false</c>.
        /// </summary>
        /// <value>True if SQL result sets are returned compressed with the GZIP filePath format
        /// before download.</value>
        public bool GzipResult => gzipResult;

        /// <summary>
        /// Gets a value indicating whether trace is enabled
        /// </summary>
        /// <value><c>true</c> if trace is enabled; otherwise, <c>false</c>.</value>
        public bool EnableTrace
        {
            get
            {
                return enableTrace;
            }
        }

        /// <summary>
        /// Gets the creation date and time of the Connection.
        /// </summary>
        /// <value>The creation date and time of the Connection.</value>
        public DateTime CreationDateTime => creationDateTime;

        /// <summary>
        /// Gets or sets the maximum retries.
        /// </summary>
        /// <value>The maximum retries.</value>
        public static int MaxRetries { get => maxRetries; set => maxRetries = value; }

        /// <summary>
        /// Gets or sets the retry interval ms.
        /// </summary>
        /// <value>The retry interval ms.</value>
        public static int RetryIntervalMs { get => retryIntervalMs; set => retryIntervalMs = value; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance. ConnectionString is not returned and password is zeroed with *.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "ConnectionInfo [server=" + server + ", database=" + database + ", username=" + username + ", creationDateTime=" + creationDateTime + ", isNTLM= " + isNTLM + ", sessionId=" + sessionId + ", proxyUri=" + proxyUri + ", proxyCredentials=" + proxyCredentials + ", useCredentialCache=" + useCredentialCache + ", timeout=" + timeout + ", enableDefaultSystemAuthentication=" + enableDefaultSystemAuthentication + ", gzipResult=" + gzipResult + ", enableTrace=" + enableTrace + "]";
        }

    }

}
