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


using AceQL.Client.Api.Batch;
using AceQL.Client.Api.Http;
using AceQL.Client.Api.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Api
{
    /// <summary>Represents a SQL statement to execute against a remote SQL database.</summary>
    public class AceQLCommand : IDisposable
    {
        internal static readonly bool DEBUG = FrameworkDebug.IsSet("AceQLCommand");

        /// <summary>
        /// The instance that does all http stuff
        /// </summary>
        private AceQLHttpApi aceQLHttpApi;
        private readonly SimpleTracer simpleTracer = new SimpleTracer();

        /// <summary>
        /// The text of the query.
        /// </summary>
        private string cmdText;
        /// <summary>
        /// The AceQL connection
        /// </summary>
        private AceQLConnection connection;

        /// <summary>
        /// The associated AceQLTransaction. 
        /// </summary>
        private AceQLTransaction transaction;


        /// <summary>
        /// The parameters
        /// </summary>
        private AceQLParameterCollection parameters;

        private bool prepare;

        private CommandType commandType = CommandType.Text;

        private int executeQueryRetryCount;

        /// <summary>
        /// For batchs 
        /// </summary>
        private String cmdTextWithQuestionMarks;

        // For batch, contain all SQL orders, one per line, in text mode: 
        private String batchFileParameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLCommand"/> class.
        /// </summary>
        public AceQLCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLCommand"/> class with the text of the query.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        /// <exception cref="System.ArgumentNullException">If cmdText is null.
        /// </exception>
        public AceQLCommand(string cmdText)
        {
            this.cmdText = cmdText ?? throw new ArgumentNullException("cmdText is null!");
            parameters = new AceQLParameterCollection(cmdText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLCommand"/> class with the text of the query 
        /// and a <see cref="AceQLConnection"/>.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        /// <param name="connection">A <see cref="AceQLConnection"/> that represents the connection to a remote database.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If cmdText is null
        /// or
        /// connection is null.
        /// </exception>
        public AceQLCommand(string cmdText, AceQLConnection connection) : this(cmdText)
        {

            if (connection == null)
            {
                throw new ArgumentNullException("connection is null!");
            }

            connection.TestConnectionOpened();

            this.connection = connection;
            this.aceQLHttpApi = connection.aceQLHttpApi;
            this.simpleTracer = aceQLHttpApi.simpleTracer;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AceQLCommand"/> class with the text of the query and a 
        /// <see cref="AceQLConnection"/>, and the <see cref="AceQLTransaction"/>.
        /// </summary>
        /// <param name="cmdText">The text of the query.</param>
        /// <param name="connection">A <see cref="AceQLConnection"/> that represents the connection to a remote database.</param>
        /// <param name="transaction">A <see cref="AceQLTransaction"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// If cmdText is null or connection or transaction is null.
        /// </exception>
        public AceQLCommand(string cmdText, AceQLConnection connection, AceQLTransaction transaction) : this(cmdText, connection)
        {
            this.transaction = transaction ?? throw new ArgumentNullException("transaction is null!");
        }


        /// <summary>
        ///  Executes the query, and returns the first column of the first row in the result set returned by the query. 
        ///  Additional columns or rows are ignored.
        /// <para/>The cancellation token can be used to can be used to request that the operation be abandoned before the http request timeout
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns> The first column of the first row in the result set, or a null reference if the result set is empty. </returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<object> ExecuteScalar(CancellationToken cancellationToken)
        {
            try
            {
                // Global var avoids to propagate cancellationToken as parameter to all methods... 
                aceQLHttpApi.SetCancellationToken(cancellationToken);
                return await ExecuteScalar().ConfigureAwait(false);
            }
            finally
            {
                aceQLHttpApi.ResetCancellationToken();
            }
        }

        /// <summary>
        ///  Executes the query, and returns the first column of the first row in the result set returned by the query. 
        ///  Additional columns or rows are ignored.
        /// </summary>
        /// <returns> The first column of the first row in the result set, or a null reference if the result set is empty. </returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<object> ExecuteScalar()
        {
            using (AceQLDataReader dataReader = await ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (dataReader == null)
                {
                    return null;
                }

                return dataReader.Read() ? dataReader.GetValue(0) : null;
            }

        }

        /// <summary>
        /// Sends the <see cref="AceQLCommand"/>.CommandText to the <see cref="AceQLConnection"/> and builds an <see cref="AceQLDataReader"/>.
        /// <para/>The cancellation token can be used to can be used to request that the operation be abandoned before the http request timeout.
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>An <see cref="AceQLDataReader"/>object.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<AceQLDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
        {

            try
            {
                // Global var avoids to propagate cancellationToken as parameter to all methods... 
                aceQLHttpApi.SetCancellationToken(cancellationToken);
                return await ExecuteReaderAsync().ConfigureAwait(false);
            }
            finally
            {
                aceQLHttpApi.ResetCancellationToken();
            }
        }



        /// <summary>
        ///  Sends the <see cref="AceQLCommand"/>.CommandText to the <see cref="AceQLConnection"/> and builds an <see cref="AceQLDataReader"/>.
        /// </summary>
        /// <returns>An <see cref="AceQLDataReader"/>object.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<AceQLDataReader> ExecuteReaderAsync()
        {
            if (cmdText == null)
            {
                throw new ArgumentNullException("cmdText is null!");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection is null!");
            }

            // Statement wit parameters are always prepared statement
            if (Parameters.Count == 0 && !prepare)
            {
                return await ExecuteQueryAsStatementAsync().ConfigureAwait(false);
            }
            else
            {
                return await ExecuteQueryAsPreparedStatementAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Creates a prepared version of the command. Optional call.
        /// Note that the remote statement will always be a prepared statement if
        /// the command contains parameters.
        /// </summary>
        public void Prepare()
        {
            this.prepare = true;
        }

        /// <summary>
        /// Executes the SQL statement against the connection and returns the number of rows affected.
        /// <para/>The cancellation token can be used to can be used to request that the operation be abandoned before the http request timeout.
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Global var avoids to propagate cancellationToken as parameter to all methods... 
                aceQLHttpApi.SetCancellationToken(cancellationToken);
                return await ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            finally
            {
                aceQLHttpApi.ResetCancellationToken();
            }
        }

        /// <summary>
        /// Executes the SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<int> ExecuteNonQueryAsync()
        {
            if (cmdText == null)
            {
                throw new ArgumentNullException("cmdText is null!");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection is null!");
            }

            // Statement with parameters are always prepared statement
            if (Parameters.Count == 0 && !prepare)
            {
                return await ExecuteUpdateAsStatementAsync().ConfigureAwait(false);
            }
            else
            {
                return await ExecuteUpdateAsPreparedStatementAsync().ConfigureAwait(false);
            }

        }


        /// <summary>
        /// Executes the query as statement.
        /// </summary>
        /// <returns>An <see cref="AceQLDataReader"/>object.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        private async Task<AceQLDataReader> ExecuteQueryAsStatementAsync()
        {
            try
            {
                string filePath = FileUtil2.GetUniqueResultSetFile();

                bool isStoredProcedure = (commandType == CommandType.StoredProcedure ? true : false);
                Boolean isPreparedStatement = false;
                Dictionary<string, string> parametersMap = null;

                using (Stream input = await aceQLHttpApi.ExecuteQueryAsync(cmdText, Parameters, isStoredProcedure, isPreparedStatement, parametersMap).ConfigureAwait(false))
                {
                    try
                    {
                        FileUtil2.CopyHttpStreamToFile(filePath, input, aceQLHttpApi.GzipResult);
                    }
                    catch (Exception exception)
                    {
                        if (this.connection.RequestRetry && (this.executeQueryRetryCount < 1 || exception.Message.Contains("GZip")))
                        {
                            this.executeQueryRetryCount++;
                            Boolean saveGzipResultValue = this.aceQLHttpApi.GzipResult;
                            this.aceQLHttpApi.GzipResult = false;
                            AceQLDataReader dataReader = await ExecuteQueryAsStatementAsync().ConfigureAwait(false);
                            this.aceQLHttpApi.GzipResult = saveGzipResultValue;
                            return dataReader;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                this.executeQueryRetryCount = 0;

                StreamResultAnalyzer streamResultAnalyzer = new StreamResultAnalyzer(filePath, aceQLHttpApi.HttpStatusCode);
                if (!streamResultAnalyzer.IsStatusOK())
                {
                    throw new AceQLException(streamResultAnalyzer.GetErrorMessage(),
                        streamResultAnalyzer.GetErrorType(),
                        streamResultAnalyzer.GetStackTrace(),
                        aceQLHttpApi.HttpStatusCode);
                }

                int rowsCount = 0;

                using (Stream readStreamCout = File.OpenRead(filePath))
                {
                    RowCounter rowCounter = new RowCounter(readStreamCout);
                    rowsCount = rowCounter.Count();
                }

                if (isStoredProcedure)
                {
                    using (Stream readStreamOutParms = File.OpenRead(filePath))
                    {
                        UpdateOutParametersValues(readStreamOutParms, Parameters);
                    }
                }

                Stream readStream = File.OpenRead(filePath);

                AceQLDataReader aceQLDataReader = new AceQLDataReader(filePath, readStream, rowsCount, connection);
                return aceQLDataReader;

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
                    throw new AceQLException(exception.Message, 0, exception, aceQLHttpApi.HttpStatusCode);
                }
            }
        }

        private static void UpdateOutParametersValues(Stream stream, AceQLParameterCollection parameters)
        {
            //1) Build outParametersDict Dict of (paramerer names, values)
            OutParamBuilder outParamBuilder = new OutParamBuilder(stream);
            Dictionary<string, string> outParametersDict = outParamBuilder.GetvaluesPerParamName();

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
        /// Executes the query as prepared statement.
        /// </summary>
        /// <returns>An <see cref="AceQLDataReader"/> object.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        private async Task<AceQLDataReader> ExecuteQueryAsPreparedStatementAsync()
        {
            try
            {
                AceQLCommandUtil aceQLCommandUtil = new AceQLCommandUtil(cmdText, Parameters);

                // Get the parameters and build the result set
                Dictionary<string, string> statementParameters = aceQLCommandUtil.GetPreparedStatementParameters();

                foreach (string key in statementParameters.Keys)
                {
                    Debug("key:==> " + key + " / " + statementParameters[key]);
                }

                // Replace all @parms with ? in sql command
                cmdText = aceQLCommandUtil.ReplaceParmsWithQuestionMarks();

                String filePath = FileUtil2.GetUniqueResultSetFile();

                bool isStoredProcedure = (commandType == CommandType.StoredProcedure ? true : false);
                bool isPreparedStatement = true;

                using (Stream input = await aceQLHttpApi.ExecuteQueryAsync(cmdText, Parameters, isStoredProcedure, isPreparedStatement, statementParameters).ConfigureAwait(false))
                {
                    try
                    {
                        FileUtil2.CopyHttpStreamToFile(filePath, input, aceQLHttpApi.GzipResult);
                    }
                    catch (Exception exception)
                    {
                        if (this.connection.RequestRetry && (this.executeQueryRetryCount < 1 || exception.Message.Contains("GZip")))
                        {
                            this.executeQueryRetryCount++;
                            Boolean saveGzipResultValue = this.aceQLHttpApi.GzipResult;
                            this.aceQLHttpApi.GzipResult = false;
                            AceQLDataReader dataReader = await ExecuteQueryAsPreparedStatementAsync().ConfigureAwait(false);
                            this.aceQLHttpApi.GzipResult = saveGzipResultValue;
                            return dataReader;
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                this.executeQueryRetryCount = 0;

                StreamResultAnalyzer streamResultAnalyzer = new StreamResultAnalyzer(filePath, aceQLHttpApi.HttpStatusCode);
                if (!streamResultAnalyzer.IsStatusOK())
                {
                    throw new AceQLException(streamResultAnalyzer.GetErrorMessage(),
                        streamResultAnalyzer.GetErrorType(),
                        streamResultAnalyzer.GetStackTrace(),
                        aceQLHttpApi.HttpStatusCode);
                }

                int rowsCount = 0;

                using (Stream readStreamCout = File.OpenRead(filePath))
                {
                    RowCounter rowCounter = new RowCounter(readStreamCout);
                    rowsCount = rowCounter.Count();
                }

                if (isStoredProcedure)
                {
                    using (Stream readStreamOutParms = File.OpenRead(filePath))
                    {
                        UpdateOutParametersValues(readStreamOutParms, Parameters);
                    }
                }

                Stream readStream = File.OpenRead(filePath);

                AceQLDataReader aceQLDataReader = new AceQLDataReader(filePath, readStream, rowsCount, connection);
                return aceQLDataReader;

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
                    throw new AceQLException(exception.Message, 0, exception, aceQLHttpApi.HttpStatusCode);
                }
            }
        }


        /// <summary>
        /// Copies the HTTP stream to filePath.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="input">The input.</param>
        public void CopyHttpStreamToFile(String path, Stream input)
        {
            if (input != null)
            {
                //if (aceQLHttpApi.GzipResult)
                bool GzipResult = true;
                if (GzipResult)
                {
                    using (GZipStream decompressionStream = new GZipStream(input, CompressionMode.Decompress))
                    {
                        using (var stream = File.OpenRead(path))
                        {
                            decompressionStream.CopyTo(stream);
                        }
                    }
                }
                else
                {
                    using (var stream = File.OpenRead(path))
                    {
                        input.CopyTo(stream);
                    }
                }
            }
        }


        /// <summary>
        /// Executes the update as prepared statement.
        /// </summary>
        /// <returns>System.Int32.</returns>
        /// <exception cref="AceQLException">
        /// </exception>
        private async Task<int> ExecuteUpdateAsPreparedStatementAsync()
        {
            try
            {
                AceQLCommandUtil aceQLCommandUtil = new AceQLCommandUtil(cmdText, Parameters);

                // Get the parameters and build the result set
                Dictionary<string, string> statementParameters = aceQLCommandUtil.GetPreparedStatementParameters();

                // Uploads Blobs
                List<string> blobIds = aceQLCommandUtil.BlobIds;
                List<Stream> blobStreams = aceQLCommandUtil.BlobStreams;
                List<long> blobLengths = aceQLCommandUtil.BlobLengths;

                long totalLength = 0;
                for (int i = 0; i < blobIds.Count; i++)
                {
                    totalLength += blobLengths[i];
                }

                for (int i = 0; i < blobIds.Count; i++)
                {
                    await aceQLHttpApi.BlobUploadAsync(blobIds[i], blobStreams[i], totalLength).ConfigureAwait(false);
                }

                // Replace all @parms with ? in sql command
                cmdText = aceQLCommandUtil.ReplaceParmsWithQuestionMarks();

                Dictionary<string, string> parametersMap = new Dictionary<string, string>
                {
                    { "sql", cmdText },
                    { "prepared_statement", "true" }
                };

                List<string> keyList = new List<string>(statementParameters.Keys);
                foreach (string key in keyList)
                {
                    parametersMap.Add(key, statementParameters[key]);
                }

                bool isStoredProcedure = (commandType == CommandType.StoredProcedure ? true : false);
                bool isPreparedStatement = true;

                int result = await aceQLHttpApi.ExecuteUpdateAsync(cmdText, Parameters, isStoredProcedure, isPreparedStatement, statementParameters).ConfigureAwait(false);
                return result;
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
                    throw new AceQLException(exception.Message, 0, exception, aceQLHttpApi.HttpStatusCode);
                }

            }
        }

        /// <summary>
        /// Empties this current batch of commands.
        /// </summary>
        public void ClearBatch()
        {
            this.cmdTextWithQuestionMarks = null;
            this.parameters = new AceQLParameterCollection(cmdText);
            if (this.batchFileParameters != null)
            {
                File.Delete(batchFileParameters);
            }

        }

        /// <summary>
        /// Adds a set of parameters to this batch of commands.
        /// </summary>
        public void AddBatch()
        {
            if (cmdText == null)
            {
                throw new ArgumentNullException("cmdText is null!");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection is null!");
            }

            if (this.Parameters.ContainsBlob())
            {
                throw new NotSupportedException("Cannot call AddBatch() for a table with BLOB parameter in this AceQL JDBC Client version.");
            }

            if (Parameters.Count == 0)
            {
                throw new NotSupportedException("Cannot call AddBatch() if the SQL command has not parameters.");
            }

            Debug("cmdText: " + cmdText);

            AceQLCommandUtil aceQLCommandUtil = new AceQLCommandUtil(cmdText, Parameters);

            // Get the parameters and build the result set
            Dictionary<string, string> statementParameters = aceQLCommandUtil.GetPreparedStatementParameters();

            // We can do only one "@param" params replace with attended JDBC "?" values
            if (this.cmdTextWithQuestionMarks == null)
            {
                cmdTextWithQuestionMarks = aceQLCommandUtil.ReplaceParmsWithQuestionMarks();
            }

            int cpt = 0;
            foreach (KeyValuePair<string, string> kvp in statementParameters)
            {
                Debug(cpt++ + " statementParameters key, value: " + kvp.Key + ", " + kvp.Value);
            }

            PrepStatementParamsHolder paramsHolder = new PrepStatementParamsHolder(statementParameters);

            if (this.batchFileParameters == null)
            {
                this.batchFileParameters = FileUtil2.GetUniqueBatchFile();
            }

            String jsonString = JsonConvert.SerializeObject(paramsHolder);
            File.AppendAllText(this.batchFileParameters, jsonString + Environment.NewLine);

            this.parameters = new AceQLParameterCollection(cmdText);

        }

        /// <summary>
        /// Submits a batch of commands to the database for execution and if all commands execute successfully, 
        /// returns an array of update counts.
        /// The int elements of the array that is returned are ordered to correspond to the commands in the batch, 
        /// which are ordered according to the order in which they were added to the batch.
        /// <para/>The cancellation token can be used to can be used to request that the operation 
        /// be abandoned before the http request timeout.
        /// </summary>
        /// <param name="cancellationToken">The cancellation instruction.</param>
        /// <returns>an array of update counts containing one element for each command in the batch. 
        /// The elements of the array are ordered according to the order in which commands were added to the batch.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<int[]> ExecuteBatchAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Global var avoids to propagate cancellationToken as parameter to all methods... 
                aceQLHttpApi.SetCancellationToken(cancellationToken);
                return await ExecuteBatchAsync().ConfigureAwait(false);
            }
            finally
            {
                aceQLHttpApi.ResetCancellationToken();
            }
        }

        /// <summary>
        /// Submits a batch of commands to the database for execution and if all commands execute successfully, 
        /// returns an array of update counts.
        /// The int elements of the array that is returned are ordered to correspond to the commands in the batch, 
        /// which are ordered according to the order in which they were added to the batch.
        /// <para/>The cancellation token can be used to can be used to request that the operation 
        /// be abandoned before the http request timeout.
        /// </summary>
        /// <returns>an array of update counts containing one element for each command in the batch. 
        /// The elements of the array are ordered according to the order in which commands were added to the batch.</returns>
        /// <exception cref="AceQL.Client.Api.AceQLException">If any Exception occurs.</exception>
        public async Task<int[]> ExecuteBatchAsync()
        {
            if (this.batchFileParameters == null || ! new FileInfo(this.batchFileParameters).Exists)
            {
                throw new NotSupportedException("Cannot call executeBatch: addBatch() has never been called.");
            }

            if (! await AceQLConnectionUtil.IsBatchSupported(connection))
            {
                throw new NotSupportedException("AceQL Server version must be >= " + AceQLConnectionUtil.BATCH_MIN_SERVER_VERSION
                    + " in order to call PreparedStatement.executeBatch().");
            }

            try
            {
                int[] updateCountsArray = await aceQLHttpApi.ExecutePreparedStatementBatch(cmdTextWithQuestionMarks, this.batchFileParameters);
                this.ClearBatch();
                return updateCountsArray;
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
                    throw new AceQLException(exception.Message, 0, exception, aceQLHttpApi.HttpStatusCode);
                }

            }
        }

        /// <summary>
        /// Executes the update as statement.
        /// </summary>
        /// <returns>System.Int32.</returns>
        /// <exception cref="AceQLException">
        /// </exception>
        private async Task<int> ExecuteUpdateAsStatementAsync()
        {
            bool isStoredProcedure = (commandType == CommandType.StoredProcedure ? true : false);
            bool isPreparedStatement = false;
            Dictionary<string, string> statementParameters = null;
            return await aceQLHttpApi.ExecuteUpdateAsync(cmdText, Parameters, isStoredProcedure, isPreparedStatement, statementParameters).ConfigureAwait(false);
        }



        /// <summary>
        /// Gets ot set SQL statement to execute against a remote SQL database.
        /// </summary>
        /// <value>The SQL statement to execute against a remote SQL database.</value>
        public string CommandText
        {
            get
            {
                return cmdText;
            }

            set
            {
                this.cmdText = value ?? throw new ArgumentNullException("cmdText is null!");
                parameters = new AceQLParameterCollection(cmdText);
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="AceQLConnection"/> used by this instance of <see cref="AceQLCommand"/>.
        /// </summary>
        /// <value>The remote database connection.</value>
        public AceQLConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                this.connection = value ?? throw new ArgumentNullException("connection is null!");
                this.connection.TestConnectionOpened();
                this.aceQLHttpApi = connection.aceQLHttpApi;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="AceQLTransaction"/> used by this instance of <see cref="AceQLCommand"/>.
        /// </summary>
        /// <value>The <see cref="AceQLTransaction"/>.</value>
        public AceQLTransaction Transaction
        {
            get
            {
                return transaction;
            }

            set
            {
                this.transaction = value ?? throw new ArgumentNullException("transaction is null!");
            }

        }

        /// <summary>
        /// Gets the <see cref="AceQLParameterCollection"/>.
        /// </summary>
        /// <value>The <see cref="AceQLParameterCollection"/>.</value>
        public AceQLParameterCollection Parameters
        {
            get
            {
                return parameters;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating how the CommandText property is to be interpreted.
        /// </summary>
        /// <value>The <see cref="CommandType"/>.</value>
        public CommandType CommandType { get => commandType; set => commandType = value; }

        /// <summary>
        /// Disposes this instance. This call is optional and does nothing because all resources are released after 
        /// each other <see cref="AceQLCommand"/> method call. Class implements <see cref="IDisposable"/> to ease code migration.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="v"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool v)
        {

        }

        private static void Debug(string s)
        {
            if (DEBUG)
            {
                ConsoleEmul.WriteLine(DateTime.Now + " " + s);
            }
        }

    }
}
