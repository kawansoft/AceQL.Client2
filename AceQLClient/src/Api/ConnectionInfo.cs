/*
 * This file is part of AceQL JDBC Driver.
 * AceQL JDBC Driver: Remote JDBC access over HTTP with AceQL HTTP.
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

using System.Net;

namespace AceQL.Client.Api
{
    /// <summary>
    /// Class ConnectionInfo. Contains all info about Connections 
    /// </summary>
    public class ConnectionInfo
    {

        private string server;
        private string database;
        private string username;
        private char[] password;
        private string sessionId;
        private string proxyUri;
        private ICredentials proxyCredentials;
        private bool useCredentialCache;
        private int timeout;
        private bool enableDefaultSystemAuthentication;
        private bool enableTrace;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionInfo"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="database">The database.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="proxyUri">The proxy URI.</param>
        /// <param name="proxyCredentials">The proxy credentials.</param>
        /// <param name="useCredentialCache">if set to <c>true</c> [use credential cache].</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="enableDefaultSystemAuthentication">if set to <c>true</c> [enable default system authentication].</param>
        /// <param name="enableTrace">if set to <c>true</c> [enable trace].</param>
        public ConnectionInfo(string server, string database, string username, char[] password, 
            string sessionId, string proxyUri, ICredentials proxyCredentials, bool useCredentialCache, 
            int timeout, bool enableDefaultSystemAuthentication, bool enableTrace) 
        {
            this.server = server;
            this.database = database;
            this.username = username;
            this.password = password;
            this.sessionId = sessionId;
            this.proxyUri = proxyUri;
            this.proxyCredentials = proxyCredentials;
            this.useCredentialCache = useCredentialCache;
            this.timeout = timeout;
            this.enableDefaultSystemAuthentication = enableDefaultSystemAuthentication;
            this.enableTrace = enableTrace;
        }

        /// <summary>
        /// Gets the server.
        /// </summary>
        /// <value>The server.</value>
        public virtual string Server
        {
            get
            {
                return server;
            }
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public virtual string Database
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
        public virtual string Username
        {
            get
            {
                return username;
            }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>The password.</value>
        public virtual char[] Password
        {
            get
            {
                return password;
            }
        }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        /// <value>The session identifier.</value>
        public virtual string SessionId
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
        public virtual string ProxyUri
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
        public virtual ICredentials ProxyCredentials
        {
            get
            {
                return proxyCredentials;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [use credential cache].
        /// </summary>
        /// <value><c>true</c> if [use credential cache]; otherwise, <c>false</c>.</value>
        public virtual bool UseCredentialCache
        {
            get
            {
                return useCredentialCache;
            }
        }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        public virtual int Timeout
        {
            get
            {
                return timeout;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [enable default system authentication].
        /// </summary>
        /// <value><c>true</c> if [enable default system authentication]; otherwise, <c>false</c>.</value>
        public virtual bool EnableDefaultSystemAuthentication
        {
            get
            {
                return enableDefaultSystemAuthentication;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [enable trace].
        /// </summary>
        /// <value><c>true</c> if [enable trace]; otherwise, <c>false</c>.</value>
        public virtual bool EnableTrace
        {
            get
            {
                return enableTrace;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "ConnectionInfo [server=" + server + ", database=" + database + ", username=" + username + ", password=" + new string(password) + ", sessionId=" + sessionId + ", proxyUri=" + proxyUri + ", proxyCredentials=" + proxyCredentials + ", useCredentialCache=" + useCredentialCache + ", timeout=" + timeout + ", enableDefaultSystemAuthentication=" + enableDefaultSystemAuthentication + ", enableTrace=" + enableTrace + "]";
        }

    }

}
