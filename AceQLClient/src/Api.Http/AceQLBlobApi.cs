using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    internal class AceQLBlobApi
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

        public AceQLBlobApi(HttpManager httpManager, string url, SimpleTracer simpleTracer)
        {
            this.httpManager = httpManager;
            this.url = url;
            this.simpleTracer = simpleTracer;
        }

        /// <summary>
        /// Returns the server Blob/Clob length.
        /// </summary>
        /// <param name="blobId">the Blob/Clob Id.</param>
        /// <returns>the server Blob/Clob length.</returns>
        internal async Task<long> GetBlobLengthAsync(String blobId)
        {
            if (blobId == null)
            {
                throw new ArgumentNullException("blobId is null!");
            }

            String action = "get_blob_length";

            Dictionary<string, string> parametersMap = new Dictionary<string, string>
            {
                { "blob_id", blobId }
            };
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

            String lengthStr = resultAnalyzer.GetValue("length");
            long length = Convert.ToInt64(lengthStr);
            return length;
        }

        /// <summary>
        /// Downloads a Blob/Clob from the server.
        /// </summary>
        /// <param name="blobId">the Blob/Clob Id</param>
        ///
        /// <returns>the Blob input stream</returns>
        internal async Task<Stream> BlobDownloadAsync(String blobId)
        {
            if (blobId == null)
            {
                throw new ArgumentNullException("blobId is null!");
            }

            try
            {
                String theUrl = this.url + "/blob_download?blob_id=" + blobId;
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

    }
}
