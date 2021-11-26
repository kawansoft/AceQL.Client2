using AceQL.Client.Api.Metadata.Dto;
using AceQL.Client.Api.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    internal class AceQLMetadataApi
    {
        // The HttpManager that contains the HtttClient to use
        internal HttpManager httpManager;

        /// <summary>
        /// The URL
        /// </summary>
        private readonly string url;

        /// <summary>
        /// The simple tracer
        /// </summary>
        private readonly SimpleTracer simpleTracer;

        /// <summary>
        /// Gets the HTTP status code of hte last executed HTTP call
        /// </summary>
        /// <value>The HTTP status code.</value>
        public HttpStatusCode HttpStatusCode { get => httpManager.HttpStatusCode; }

        public AceQLMetadataApi(HttpManager httpManager, string url, SimpleTracer simpleTracer)
        {
            this.httpManager = httpManager;
            this.url = url;
            this.simpleTracer = simpleTracer;
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
            if (format == null)
            {
                throw new ArgumentNullException("format is null!");
            }

            try
            {
                String theUrl = this.url + "/metadata_query/db_schema_download?format=" + format;

                if (tableName != null)
                {
                    tableName = tableName.ToLowerInvariant();
                    theUrl += "&table_name=" + tableName;
                }

                Stream input = await httpManager.CallWithGetReturnStreamAsync(theUrl);
                return input;
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
        /// Gets the database metadata.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<JdbcDatabaseMetaDataDto> GetDbMetadataAsync()
        {
            try
            {
                String commandName = "metadata_query/get_db_metadata";
                String result = await CallWithGetAsync(commandName, null).ConfigureAwait(false);

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        HttpStatusCode);
                }

                JdbcDatabaseMetaDataDto jdbcDatabaseMetaDataDto = JsonConvert.DeserializeObject<JdbcDatabaseMetaDataDto>(result);
                return jdbcDatabaseMetaDataDto;
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
        /// Gets the table names.
        /// </summary>
        /// <param name="tableType">Type of the table.</param>
        /// <returns>Task&lt;TableNamesDto&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<TableNamesDto> GetTableNamesAsync(String tableType)
        {
            try
            {
                String action = "metadata_query/get_table_names";

                Dictionary<string, string> parametersMap = new Dictionary<string, string>();
                if (tableType != null)
                {
                    parametersMap.Add("table_type", tableType);
                }

                String result = null;

                Uri urlWithaction = new Uri(url + action);
                using (Stream input = await httpManager.CallWithPostAsync(urlWithaction, parametersMap).ConfigureAwait(false))
                {
                    if (input != null)
                    {
                        result = new StreamReader(input).ReadToEnd();
                    }
                }

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        HttpStatusCode);
                }

                TableNamesDto tableNamesDto = JsonConvert.DeserializeObject<TableNamesDto>(result);
                return tableNamesDto;
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
        /// Gets the table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>Task&lt;TableDto&gt;.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<TableDto> GetTableAsync(String tableName)
        {
            try
            {
                String action = "metadata_query/get_table";

                Dictionary<string, string> parametersMap = new Dictionary<string, string>();
                parametersMap.Add("table_name", tableName);

                String result = null;

                Uri urlWithaction = new Uri(url + action);
                using (Stream input = await httpManager.CallWithPostAsync(urlWithaction, parametersMap).ConfigureAwait(false))
                {
                    if (input != null)
                    {
                        result = new StreamReader(input).ReadToEnd();
                    }
                }

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        HttpStatusCode);
                }

                TableDto tableDto = JsonConvert.DeserializeObject<TableDto>(result);
                return tableDto;
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
        /// Calls the with get.
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
    }
}
