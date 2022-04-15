/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2021,  KawanSoft SAS
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


using AceQL.Client.Api.Metadata.Dto;
using AceQL.Client.Api.Util;
using AceQL.Client.Api.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AceQL.Client.Api.Batch;

namespace AceQL.Client.Api.Http
{

    /// <summary>
    /// Class <see cref="AceQLHttpApi"/>. Allows to create a Connection to the remote server
    /// </summary>
    internal class AceQLHttpApi
    {

        internal static readonly bool DEBUG = FrameworkDebug.IsSet("AceQLHttpApi");

        /// <summary>
        /// The server URL
        /// </summary>
        private String server;

        private string username;

        /// <summary>
        /// The database
        /// </summary>
        private String database;
        private char[] password;

        /// <summary>
        /// The Web Proxy Uri
        /// </summary>
        private string proxyUri;

        /// <summary>
        /// The credentials
        /// </summary>
        private ICredentials proxyCredentials;
        private bool useCredentialCache;

        /// <summary>
        /// The timeout in milliseconds
        /// </summary>
        private int timeout;
        private bool enableDefaultSystemAuthentication;

        /// <summary>
        /// The request headers added by the user
        /// </summary>
        private Dictionary<string, string> requestHeaders = new Dictionary<string, string>();

        /// <summary>
        /// The pretty printing
        /// </summary>
        const bool prettyPrinting = true;
        /// <summary>
        /// The gzip result
        /// </summary>
        bool gzipResult = true;


        /// <summary>
        /// The URL
        /// </summary>
        private string url;

        private readonly string connectionString;

        private AceQLProgressIndicator progressIndicator;
        private AceQLCredential credential;
        private CancellationToken cancellationToken;
        private bool useCancellationToken;

        internal SimpleTracer simpleTracer = new SimpleTracer();

        // The HttpManager that contains the HtttClient to use
        internal HttpManager httpManager;

        private ConnectionInfo connectionInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLHttpApi"/> class.
        /// </summary>
        internal AceQLHttpApi()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLHttpApi"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.
        /// </param>"
        /// <exception cref="System.ArgumentException">connectionString token does not contain a = separator: " + line</exception>
        internal AceQLHttpApi(String connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException("connectionString is null!");
        }

        internal AceQLHttpApi(string connectionString, AceQLCredential credential) : this(connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString is null!");
            }

            this.credential = credential ?? throw new ArgumentNullException("credential is null!");
        }

        /// <summary>
        /// Resets the request headers.
        /// </summary>
        internal void ResetRequestHeaders()
        {
            requestHeaders = new Dictionary<string, string>();
        }

        /// <summary>
        /// Adds A request header. This will be added at each server call.
        /// </summary>
        /// <param name="name">The header name.</param>
        /// <param name="value">The header value.</param>
        internal void AddRequestHeader(string name, string value)
        {
            requestHeaders.Add(name, value);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"> if a required parameter extracted from connection string is missing.
        /// </exception>
        /// <exception cref="AceQLException"> if any other Exception occurs.
        /// </exception>
        internal async Task OpenAsync()
        {
            string sessionId;

            try
            {
                ConnectionStringDecoder connectionStringDecoder = new ConnectionStringDecoder();
                connectionStringDecoder.Decode(connectionString);
                this.server = connectionStringDecoder.Server;
                this.database = connectionStringDecoder.Database;
                this.username = connectionStringDecoder.Username;
                this.password = connectionStringDecoder.Password;
                bool isNTLM = connectionStringDecoder.IsNTLM;
                sessionId = connectionStringDecoder.SessionId;
                this.proxyUri = connectionStringDecoder.ProxyUri;
                this.proxyCredentials = connectionStringDecoder.ProxyCredentials;
                this.useCredentialCache = connectionStringDecoder.UseCredentialCache;
                this.timeout = connectionStringDecoder.Timeout;
                this.enableDefaultSystemAuthentication = connectionStringDecoder.EnableDefaultSystemAuthentication;
                bool enableTrace = connectionStringDecoder.EnableTrace;
                this.gzipResult = connectionStringDecoder.GzipResult;

                connectionInfo = new ConnectionInfo(connectionString, server, database, username, isNTLM, sessionId, proxyUri, proxyCredentials,
                    useCredentialCache, timeout, enableDefaultSystemAuthentication, gzipResult, enableTrace);

                if (enableTrace)
                {
                    simpleTracer.SetTraceOn(true);
                }

                simpleTracer.Trace("connectionString: " + connectionString);
                simpleTracer.Trace("DecodeConnectionString() done!");

                if (credential != null)
                {
                    username = credential.Username;

                    if (credential.Password != null)
                    {
                        password = credential.Password;
                    }

                    if (credential.SessionId != null)
                    {
                        sessionId = credential.SessionId;
                    }
                }

                if (server == null)
                {
                    throw new ArgumentNullException("Server keyword not found in connection string.");
                }

                if (password == null && sessionId == null)
                {
                    throw new ArgumentNullException("Password keyword or SessionId keyword not found in connection string or AceQLCredential not set");
                }

                if (database == null)
                {
                    throw new ArgumentNullException("Database keyword not found in connection string.");
                }

                this.username = username ?? throw new ArgumentNullException("Username keyword not found in connection string or AceQLCredential not set.");

                // Create the HttpManager instance
                this.httpManager = new HttpManager(proxyUri, proxyCredentials, timeout, enableDefaultSystemAuthentication, requestHeaders);
                this.httpManager.SetSimpleTracer(simpleTracer);

                Debug("httpManager.Proxy: " + httpManager.Proxy);

                UserLoginStore userLoginStore = new UserLoginStore(server, username,
                    database);

                if (sessionId != null)
                {
                    userLoginStore.SetSessionId(sessionId);
                }

                if (userLoginStore.IsAlreadyLogged())
                {
                    simpleTracer.Trace("Get a new connection with get_connection");
                    sessionId = userLoginStore.GetSessionId();

                    String theUrl = server + "/session/" + sessionId + "/get_connection";
                    String result = await httpManager.CallWithGetAsync(theUrl).ConfigureAwait(false);

                    simpleTracer.Trace("result: " + result);
                    ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result,
                        HttpStatusCode);

                    if (!resultAnalyzer.IsStatusOk())
                    {
                        throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                            resultAnalyzer.GetErrorId(),
                            resultAnalyzer.GetStackTrace(),
                            HttpStatusCode);
                    }

                    String connectionId = resultAnalyzer.GetValue("connection_id");
                    simpleTracer.Trace("result: " + result);

                    this.url = server + "/session/" + sessionId + "/connection/"
                        + connectionId + "/";
                }
                else
                {
                    await DummyGetCallForProxyAuthentication().ConfigureAwait(false);

                    String theUrl = server + "/database/" + database + "/username/" + username + "/login";
                    Debug("theUrl: " + theUrl);

                    Dictionary<string, string> parametersMap = new Dictionary<string, string>
                    {
                        { "password", new String(password) },
                        { "client_version", VersionValues.VERSION}
                    };

                    simpleTracer.Trace("Before CallWithPostAsyncReturnString: " + theUrl);
                    String result = await httpManager.CallWithPostAsyncReturnString(new Uri(theUrl), parametersMap).ConfigureAwait(false);

                    Debug("result: " + result);
                    simpleTracer.Trace("result: " + result);

                    ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, HttpStatusCode);
                    if (!resultAnalyzer.IsStatusOk())
                    {
                        throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                            resultAnalyzer.GetErrorId(),
                            resultAnalyzer.GetStackTrace(),
                            HttpStatusCode);
                    }

                    ConnectionInfo.creationDateTime = DateTime.Now;

                    String theSessionId = resultAnalyzer.GetValue("session_id");
                    String theConnectionId = resultAnalyzer.GetValue("connection_id");

                    this.url = server + "/session/" + theSessionId + "/connection/" + theConnectionId + "/";
                    userLoginStore.SetSessionId(theSessionId);
                    simpleTracer.Trace("OpenAsync url: " + this.url);
                }
            }
            catch (Exception exception)
            {
                simpleTracer.Trace(exception.ToString());

                if (exception.GetType() == typeof(AceQLException))
                {
                    throw;
                }
                else
                {
                    throw new AceQLException(exception.Message, 0, exception, HttpStatusCode);
                }
            }

        }

        /// <summary>
        /// Dummies the get call for proxy authentication.
        /// Hack to force a first GET on just aceql servlet if we are with a proxy, just to avoid system failure
        /// Because of a bug in C#, if a POST is done first to detect a 407 (proxy auth asked), HttpClient throws an Exception
        /// </summary>
        private async Task DummyGetCallForProxyAuthentication()
        {
            if ((this.httpManager.Proxy != null && proxyCredentials != null) || useCredentialCache)
            {
                String getResult = await httpManager.CallWithGetAsync(server).ConfigureAwait(false);
                Debug(server + " - getResult: " + getResult);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [gzip result] is on or off.
        /// </summary>
        /// <value><c>true</c> if [gzip result]; otherwise, <c>false</c>.</value>
        internal bool GzipResult
        {
            get
            {
                return gzipResult;
            }

            set
            {
                gzipResult = value;
            }
        }

        internal string Database
        {
            get
            {
                return database;
            }
        }

        /// <summary>
        /// The timeout in milliseconds
        /// </summary>
        internal int Timeout { get => timeout; }


        public AceQLCredential Credential
        {
            get
            {
                return credential;
            }

            set
            {
                credential = value;
            }
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        internal string ConnectionString { get => connectionString; }

        /// <summary>
        /// Says it use has passed a CancellationToken
        /// </summary>
        public bool UseCancellationToken { get => useCancellationToken; }

        /// <summary>
        /// Gets the HTTP status code of hte latsexecuted HTTP call
        /// </summary>
        /// <value>The HTTP status code.</value>
        public HttpStatusCode HttpStatusCode { get => httpManager.HttpStatusCode; }

        /// <summary>
        /// Gets the connection information.
        /// </summary>
        /// <value>The connection information.</value>
        public ConnectionInfo ConnectionInfo { get => connectionInfo; }

        internal string GetDatabase()
        {
            return database;
        }

        internal string GetUsername()
        {
            return username;
        }

        internal string GetServer()
        {
            return server;
        }


        /// <summary>
        /// Calls the API no result.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandOption">The command option.</param>
        /// <exception cref="System.ArgumentNullException">commandName is null!</exception>
        /// <exception cref="AceQLException">
        /// HTTP_FAILURE" + " " + httpStatusDescription - 0
        /// or
        /// or
        /// 0
        /// </exception>
        internal async Task CallApiNoResultAsync(String commandName, String commandOption)
        {
            try
            {
                if (commandName == null)
                {
                    throw new ArgumentNullException("commandName is null!");
                }

                String result = await CallWithGetAsync(commandName, commandOption).ConfigureAwait(false);

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, httpManager.HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        httpManager.HttpStatusCode);
                }

            }
            catch (Exception exception)
            {
                simpleTracer.Trace(exception.ToString());

                if (exception.GetType() == typeof(AceQLException))
                {
                    throw;
                }
                else
                {
                    throw new AceQLException(exception.Message, 0, exception, httpManager.HttpStatusCode);
                }
            }
        }

        /// <summary>
        /// Calls the API with result.
        /// </summary>
        /// <param name="commandName">Name of the command.</param>
        /// <param name="commandOption">The command option.</param>
        /// <exception cref="System.ArgumentNullException">commandName is null!</exception>
        /// <exception cref="AceQLException">
        /// HTTP_FAILURE" + " " + httpStatusDescription - 0
        /// or
        /// or
        /// 0
        /// </exception>
        internal async Task<string> CallApiWithResultAsync(String commandName, String commandOption)
        {
            try
            {
                if (commandName == null)
                {
                    throw new ArgumentNullException("commandName is null!");
                }

                String result = await CallWithGetAsync(commandName, commandOption).ConfigureAwait(false);

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, httpManager.HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        httpManager.HttpStatusCode);
                }

                return resultAnalyzer.GetResult();

            }
            catch (Exception exception)
            {
                simpleTracer.Trace(exception.ToString());

                if (exception.GetType() == typeof(AceQLException))
                {
                    throw;
                }
                else
                {
                    throw new AceQLException(exception.Message, 0, exception, httpManager.HttpStatusCode);
                }
            }
        }

        /// <summary>
        /// Calls the action with get.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="actionParameter">The action parameter.</param>
        /// <returns>String.</returns>
        private async Task<string> CallWithGetAsync(String action, String actionParameter)
        {
            String urlWithaction = this.url + action;

            if (actionParameter != null && actionParameter.Length != 0)
            {
                urlWithaction += "/" + actionParameter;
            }

            return await httpManager.CallWithGetAsync(urlWithaction).ConfigureAwait(false);
        }


        internal async Task<Stream> ExecuteServerQueryAsync(String serverQueryExecutorClassName, List<Object> parameters)
        {
            String action = "execute_server_query";

            if (parameters == null)
            {
                parameters = new List<object>();
            }

            ServerQueryExecutorDto serverQueryExecutorDto = ServerQueryExecutorDtoBuilder.Build(serverQueryExecutorClassName, parameters);
            string jsonString = JsonConvert.SerializeObject(serverQueryExecutorDto);

            ConsoleEmul.WriteLine(DateTime.Now + " ExecuteServerQueryAsync.jsonString: " + jsonString);

            Dictionary<string, string> parametersMap = new Dictionary<string, string>
            {
                { "gzip_result", gzipResult.ToString() },
                { "pretty_printing", prettyPrinting.ToString()},
                { "column_types", "true" }, // Force column_types, mandatory for C# AceQLDataReader
                { "server_query_executor_dto", jsonString }
            };

            Uri urlWithaction = new Uri(url + action);
            Stream input = await httpManager.CallWithPostAsync(urlWithaction, parametersMap).ConfigureAwait(false);
            return input;
        }

        internal async Task<Stream> ExecuteQueryAsync(string cmdText, bool isStoredProcedure, bool isPreparedStatement, Dictionary<string, string> statementParameters)
        {
            String action = "execute_query";

            Dictionary<string, string> parametersMap = new Dictionary<string, string>
            {
                { "sql", cmdText },
                { "prepared_statement", isPreparedStatement.ToString() },
                { "stored_procedure", isStoredProcedure.ToString() },
                { "column_types", "true" }, // Force column_types, mandatory for C# AceQLDataReader
                { "gzip_result", gzipResult.ToString() },
                { "pretty_printing", prettyPrinting.ToString() }
            };

            if (statementParameters != null)
            {
                List<string> keyList = new List<string>(statementParameters.Keys);
                foreach (string key in keyList)
                {
                    parametersMap.Add(key, statementParameters[key]);
                }
            }

            Uri urlWithaction = new Uri(url + action);
            Stream input = await httpManager.CallWithPostAsync(urlWithaction, parametersMap).ConfigureAwait(false);
            return input;
        }

        internal async Task<int> ExecuteUpdateAsync(string sql, AceQLParameterCollection Parameters, bool isStoredProcedure, bool isPreparedStatement, Dictionary<string, string> statementParameters)
        {
            String action = "execute_update";

            // Call raw execute if non query/select stored procedure. (Dirty!! To be corrected.)
            if (isStoredProcedure)
            {
                action = "execute";
            }

            Dictionary<string, string> parametersMap = new Dictionary<string, string>
            {
                { "sql", sql },
                { "prepared_statement", isPreparedStatement.ToString() },
                { "stored_procedure", isStoredProcedure.ToString() }
            };

            if (statementParameters != null)
            {
                List<string> keyList = new List<string>(statementParameters.Keys);
                foreach (string key in keyList)
                {
                    parametersMap.Add(key, statementParameters[key]);
                }
            }

            Uri urlWithaction = new Uri(url + action);

            simpleTracer.Trace("url: " + url + action);

            foreach (KeyValuePair<String, String> p in parametersMap)
            {
                simpleTracer.Trace("parm: " + p.Key + " / " + p.Value);
            }

            string result = await httpManager.CallWithPostAsyncReturnString(urlWithaction, parametersMap).ConfigureAwait(false);

            Debug("result: " + result);

            ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, httpManager.HttpStatusCode);
            if (!resultAnalyzer.IsStatusOk())
            {
                throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                    resultAnalyzer.GetErrorId(),
                    resultAnalyzer.GetStackTrace(),
                    httpManager.HttpStatusCode);
            }

            int rowCount = resultAnalyzer.GetIntvalue("row_count");

            if (isStoredProcedure)
            {
                UpdateOutParametersValues(result, Parameters);
            }

            return rowCount;

        }

        /// <summary>
        /// Executes the prepared statement batch.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="batchFileParameters">The path of file containing the prepared statement parameters.</param>
        /// <returns>Task&lt;System.Int32[]&gt;.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException"></exception>
        internal async Task<int[]> ExecutePreparedStatementBatch(string sql, String batchFileParameters)
        {
            String action = "prepared_statement_execute_batch";
            String blobId = Path.GetFileName(batchFileParameters);

            Debug("batchFileParameters : " + batchFileParameters);
            Debug("blobId              : " + blobId);

            long length = new FileInfo(batchFileParameters).Length;

            using (Stream stream = File.OpenRead(batchFileParameters))
            {
                await BlobUploadAsync(blobId, stream, length).ConfigureAwait(false);
            }

            Dictionary<string, string> parametersMap = new Dictionary<string, string>
            {
                { "sql", sql },
                { "blob_id", blobId}
            };

            Uri urlWithaction = new Uri(url + action);
            simpleTracer.Trace("url: " + url + action);

            string result = await httpManager.CallWithPostAsyncReturnString(urlWithaction, parametersMap).ConfigureAwait(false);

            Debug("result 1: " + result);

            ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, httpManager.HttpStatusCode);
            if (!resultAnalyzer.IsStatusOk())
            {
                throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                    resultAnalyzer.GetErrorId(),
                    resultAnalyzer.GetStackTrace(),
                    httpManager.HttpStatusCode);
            }

            UpdateCountsArrayDto updateCountsArrayDto = JsonConvert.DeserializeObject<UpdateCountsArrayDto>(result);
            int[] updateCountsArray = updateCountsArrayDto.GetUpdateCountsArray();
            return updateCountsArray;
        }

        /// <summary>
        /// Update the OUT parameters values
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parameters"></param>
        private static void UpdateOutParametersValues(string result, AceQLParameterCollection parameters)
        {
            //1) Build outParametersDict Dict of (paramerer names, values)

            dynamic xj = JsonConvert.DeserializeObject(result);
            dynamic xjParametersOutPername = xj.parameters_out_per_name;

            if (xjParametersOutPername == null)
            {
                return;
            }

            String dictStr = xjParametersOutPername.ToString();
            Dictionary<string, string> outParametersDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictStr);

            //2) Scan  foreach AceQLParameterCollection parameters and modify value if parameter name is in outParametersDict
            foreach (AceQLParameter parameter in parameters)
            {
                string parameterName = parameter.ParameterName;

                if (outParametersDict.ContainsKey(parameterName))
                {
                    parameter.Value = outParametersDict[parameterName];
                }
            }
        }




        /// <summary>
        /// Uploads a Blob/Clob on the server.
        /// </summary>
        /// <param name="blobId">the Blob/Clob Id</param>
        /// <param name="stream">the stream of the Blob/Clob</param>
        /// <param name="totalLength">the total length of all BLOBs to upload</param>
        /// <returns>The result as JSON format.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// blobId is null!
        /// or
        /// filePath is null!
        /// </exception>
        /// <exception cref="System.IO.FileNotFoundException">filePath does not exist: " + filePath</exception>
        /// <exception cref="AceQLException">HTTP_FAILURE" + " " + httpStatusDescription - 0</exception>
        internal async Task<String> BlobUploadAsync(String blobId, Stream stream, long totalLength)
        {

            if (blobId == null)
            {
                throw new ArgumentNullException("blobId is null!");
            }

            if (stream == null)
            {
                throw new ArgumentNullException("stream is null!");
            }

            String theUrl = url + "blob_upload";

            FormUploadStream formUploadStream = new FormUploadStream();
            HttpResponseMessage response = await formUploadStream.UploadAsync(theUrl, proxyUri, proxyCredentials, timeout, enableDefaultSystemAuthentication, blobId, stream,
    totalLength, progressIndicator, cancellationToken, useCancellationToken, requestHeaders).ConfigureAwait(false);
            httpManager.HttpStatusCode = response.StatusCode;

            Stream streamResult = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            String result = null;
            if (streamResult != null)
            {
                result = new StreamReader(streamResult).ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// Returns the server Blob/Clob length.
        /// </summary>
        /// <param name="blobId">the Blob/Clob Id.</param>
        /// <returns>the server Blob/Clob length.</returns>
        internal async Task<long> GetBlobLengthAsync(String blobId)
        {
            AceQLBlobApi aceQLBlobApi = new AceQLBlobApi(httpManager, url, simpleTracer);
            return await aceQLBlobApi.GetBlobLengthAsync(blobId).ConfigureAwait(false);
        }


        /// <summary>
        /// Downloads a Blob/Clob from the server.
        /// </summary>
        /// <param name="blobId">the Blob/Clob Id</param>
        ///
        /// <returns>the Blob input stream</returns>
        internal async Task<Stream> BlobDownloadAsync(String blobId)
        {
            AceQLBlobApi aceQLBlobApi = new AceQLBlobApi(httpManager, url, simpleTracer);
            return await aceQLBlobApi.BlobDownloadAsync(blobId).ConfigureAwait(false);
        }

        /// <summary>
        /// Databases the schema download.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Task&lt;Stream&gt;.</returns>
        /// <exception cref="ArgumentNullException">format is null!</exception>
        /// <exception cref="AceQLException">0</exception>
        /// 
        internal async Task<Stream> DbSchemaDownloadAsync(String format, String tableName)
        {
            AceQLMetadataApi aceQLMetadataApi = new AceQLMetadataApi(httpManager, url, simpleTracer);
            return await aceQLMetadataApi.DbSchemaDownloadAsync(format, tableName).ConfigureAwait(false);

        }

        /// <summary>
        /// Gets the database metadata.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<JdbcDatabaseMetaDataDto> GetDbMetadataAsync()
        {
            AceQLMetadataApi aceQLMetadataApi = new AceQLMetadataApi(httpManager, url, simpleTracer);
            return await aceQLMetadataApi.GetDbMetadataAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the table names.
        /// </summary>
        /// <param name="tableType">Type of the table.</param>
        /// <returns>Task&lt;TableNamesDto&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<TableNamesDto> GetTableNamesAsync(String tableType)
        {
            AceQLMetadataApi aceQLMetadataApi = new AceQLMetadataApi(httpManager, url, simpleTracer);
            return await aceQLMetadataApi.GetTableNamesAsync(tableType).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Task&lt;TableDto&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<TableDto> GetTableAsync(String tableName)
        {
            AceQLMetadataApi aceQLMetadataApi = new AceQLMetadataApi(httpManager, url, simpleTracer);
            return await aceQLMetadataApi.GetTableAsync(tableName).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the database information.
        /// </summary>
        /// <returns>Task&lt;DatabaseInfo&gt;.</returns>
        internal async Task<DatabaseInfo> GetDatabaseInfo()
        {
            AceQLMetadataApi aceQLMetadataApi = new AceQLMetadataApi(httpManager, url, simpleTracer);
            DatabaseInfoDto databaseInfoDto = await aceQLMetadataApi.GetDatabaseInfoDto().ConfigureAwait(false);
            return new DatabaseInfo(databaseInfoDto);
        }

        /// <summary>
        /// To be call at end of each of each public aysnc(CancellationToken) calls to reset to false the usage of a CancellationToken with http calls
        /// and some reader calls.
        /// </summary>
        internal void ResetCancellationToken()
        {
            this.useCancellationToken = false;
        }

        /// <summary>
        /// Sets the CancellationToken asked by user to pass for the current public xxxAsync() call api.
        /// </summary>
        /// <param name="cancellationToken">CancellationToken asked by user to pass for the current public xxxAsync() call api.</param>
        internal void SetCancellationToken(CancellationToken cancellationToken)
        {
            this.useCancellationToken = true;
            this.cancellationToken = cancellationToken;
        }

        /// <summary>
        /// Returns the progress indicator variable that will store Blob/Clob upload or download progress between 0 and 100.
        /// </summary>
        /// <returns>The progress indicator variable that will store Blob/Clob upload or download progress between 0 and 100.</returns>
        internal AceQLProgressIndicator GetProgressIndicator()
        {
            return progressIndicator;
        }


        /// <summary>
        /// Sets the progress indicator variable that will store Blob/Clob upload or download progress between 0 and 100. Will be used by progress indicators to show the progress.
        /// </summary>
        /// <param name="progressIndicator">The progress variable.</param>
        internal void SetProgressIndicator(AceQLProgressIndicator progressIndicator)
        {
            this.progressIndicator = progressIndicator;
        }

        /// <summary>
        /// Returns the SDK current Version.
        /// </summary>
        /// <returns>the SDK current Version.</returns>
        internal static String GetVersion()
        {
            return Util.Version.GetVersion();
        }


        /// <summary>
        /// Closes the connection to the remote database and closes the http session.
        /// </summary>
        public async Task CloseAsync()
        {
            await CallApiNoResultAsync("disconnect", null).ConfigureAwait(false);

            if (httpManager != null)
            {
                httpManager.Dispose();
            }
        }

        internal static void Debug(string s)
        {
            if (DEBUG)
            {
                ConsoleEmul.WriteLine(DateTime.Now + " " + s);
            }
        }
    }
}
