using System.Collections.Generic;

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

namespace AceQL.Client.Api
{

    /// <summary>
    /// Allows to get all the info set and passed when creating an SQL
    /// Connection to the remote AceQL Server.
    /// <br>
    /// A {@code ConnectionInfo} instance is retrieved with the
    /// <seealso cref="AceQLConnection.getConnectionInfo()"/> call: <br/>
    /// 
    /// // Casts the current Connection to get an AceQLConnection object
    /// AceQLConnection aceqlConnection = (AceQLConnection) connection;
    /// 
    /// ConnectionInfo connectionInfo = aceqlConnection.getConnectionInfo();
    /// Console.WriteLine("timeout : " + connectionInfo.getConnectTimeout());
    /// Console.WriteLine("All Info: " + connectionInfo);
    /// // Etc.
    /// </summary>
    public class ConnectionInfo
    {

        private string url;
        private string database;
        private PasswordAuthentication authentication;
        private bool passwordIsSessionId;
        private string proxyUri;
        private PasswordAuthentication proxyAuthentication;

        private int timeout = 0;
        private bool gzipResult;
        private IDictionary<string, string> requestProperties = new Dictionary<string, string>();

        /// <summary>
        /// Package protected constructor, Driver users can not instantiate the class. </summary>
        /// <param name="url"> </param>
        /// <param name="database"> </param>
        /// <param name="authentication"> </param>
        /// <param name="passwordIsSessionId"> </param>
        /// <param name="proxyUri"> </param>
        /// <param name="proxyAuthentication"> </param>
        /// <param name="timeout"> </param>
        /// <param name="gzipResult"> </param>
        /// <param name="requestProperties"> </param>
        internal ConnectionInfo(string url, string database, PasswordAuthentication authentication, bool passwordIsSessionId, string proxyUri, PasswordAuthentication proxyAuthentication, int timeout, bool gzipResult, IDictionary<string, string> requestProperties)
        {
            this.url = url;
            this.database = database;
            this.authentication = authentication;
            this.passwordIsSessionId = passwordIsSessionId;
            this.proxyUri = proxyUri;
            this.proxyAuthentication = proxyAuthentication;
            this.timeout = timeout;
            this.gzipResult = gzipResult;
            this.requestProperties = requestProperties;
        }

        /// <summary>
        /// Gets the URL of the remote database
        /// </summary>
        /// <returns> the URL of the remote database </returns>
        public virtual string Url
        {
            get
            {
                return url;
            }
        }

        /// <summary>
        /// Gets the remote database name
        /// </summary>
        /// <returns> the remote database name </returns>
        public virtual string Database
        {
            get
            {
                return database;
            }
        }

        /// <summary>
        /// Gets the main authentication info against the AceQL server
        /// </summary>
        /// <returns> the main authentication info  </returns>
        public virtual PasswordAuthentication Authentication
        {
            get
            {
                return authentication;
            }
        }


        /// <summary>
        /// Says if the password is an AceQL Session ID. Applies only to Professional Edition. </summary>
        /// <returns> true if the password is an AceQL Session ID, else false </returns>
        public virtual bool PasswordSessionId
        {
            get
            {
                return passwordIsSessionId;
            }
        }

        /// <summary>
        /// Gets the Proxyuri in use. Returns null if no Proxy is in use.
        /// </summary>
        /// <returns> the Proxy in use. </returns>
        public virtual string ProxyUri
        {
            get
            {
                return proxyUri;
            }
        }



        /// <summary>
        /// Gets the Proxy username and password. Returns null if no Proxy is in use.
        /// </summary>
        /// <returns> the username and password. </returns>
        public virtual PasswordAuthentication ProxyAuthentication
        {
            get
            {
                return proxyAuthentication;
            }
        }



        /// <summary>
        /// Gets a boolean that say if the ResultSet is gzipped before download.
        /// </summary>
        /// <returns> true if the ResultSet is gzipped before download,
        ///         else false</returns>
        public virtual bool GzipResult
        {
            get
            {
                return gzipResult;
            }
        }


        /// <summary>
        /// Gets all the request properties that are set
        /// </summary>
        /// <returns> the request properties that are set </returns>
        public virtual IDictionary<string, string> RequestProperties
        {
            get
            {
                return requestProperties;
            }
        }

        /// <summary>
        /// Gets the timeout.
        /// </summary>
        /// <value>The timeout.</value>
        public int Timeout { get => timeout; }

        /// <summary>
        /// Returns all Connection Info.
        /// </summary>
        /// <returns>All Connection Info.</returns>
        public override string ToString()
        {
            return "ConnectionInfo [url=" + url + ", database=" + database + ", authentication=" + authentication + ", passwordIsSessionId=" + passwordIsSessionId + ", proxyUri=" + proxyUri + ", proxyAuthentication=" + proxyAuthentication + ", timeout=" + Timeout + ", gzipResult=" + gzipResult + ", requestProperties=" + requestProperties + "]";
        }


    }
}

