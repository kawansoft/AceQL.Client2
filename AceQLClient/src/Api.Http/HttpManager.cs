/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2023,  KawanSoft SAS
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

using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    /// <summary>
    /// Class HttpManager. A wrapped HttpClient tool that can do everything
    /// </summary>
    internal class HttpManager : IDisposable
    {
        private CancellationToken cancellationToken;
        private bool useCancellationToken;

        /// <summary>
        /// The timeout in milliseconds
        /// </summary>
        private readonly int timeout;

        /// <summary>
        /// The HTTP status code
        /// </summary>
        private HttpStatusCode httpStatusCode;

        private SimpleTracer simpleTracer = new SimpleTracer();

        /// <summary>
        /// The HTTP client
        /// </summary>
        private HttpClient httpClient;

        private int proxyAuthenticationCallCount;

        /// <summary>
        /// The proxy. Is null if no Defaut/System proxy and no used defined proxy.
        /// </summary>
        private IWebProxy proxy;


        /// <summary>
        /// Initializes a new instance of the <see cref="HttpManager"/> class.
        /// </summary>
        /// <param name="proxyUri">The proxy URI.</param>
        /// <param name="proxyCredentials">The proxy credentials.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="enableDefaultSystemAuthentication">if set to <c>true</c> [enable default system authentication].</param>
        /// <param name="requestHeaders">The request headers to add to all requests.</param>
        public HttpManager(string proxyUri, ICredentials proxyCredentials, int timeout, bool enableDefaultSystemAuthentication, Dictionary<string, string> requestHeaders)
        {
            this.timeout = timeout;
            BuildHttpClient(proxyUri, proxyCredentials, enableDefaultSystemAuthentication, requestHeaders);
        }

        /// <summary>
        /// Builds the HTTP client.
        /// </summary>
        /// <param name="proxyUri">The proxy URI.</param>
        /// <param name="proxyCredentials">The proxy credentials.</param>
        /// <param name="enableDefaultSystemAuthentication">if set to <c>true</c> [enable default system authentication].</param>
        /// <param name="requestHeaders">The request headers to add to all requests.</param>
        private void BuildHttpClient(string proxyUri, ICredentials proxyCredentials, bool enableDefaultSystemAuthentication, Dictionary<string, string> requestHeaders)
        {
            HttpClientHandler httpClientHandler = HttpClientHandlerBuilderNew.Build(proxyUri, proxyCredentials, enableDefaultSystemAuthentication);
            this.proxy = httpClientHandler.Proxy;
            this.httpClient = new HttpClient(httpClientHandler);

            AddRequestHeaders(httpClient, requestHeaders);
        }

        /// <summary>
        /// Add request headers to the HttpClient instance.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="requestHeaders"></param>
        public static void AddRequestHeaders(HttpClient httpClient, Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders != null)
            {
                List<string> keyList = new List<string>(requestHeaders.Keys);
                foreach (string key in keyList)
                {
                    httpClient.DefaultRequestHeaders.Add(key, requestHeaders[key]);
                }
            }
        }

        /// <summary>
        /// Says it use has passed a CancellationToken
        /// </summary>
        public bool UseCancellationToken { get => useCancellationToken; }

        /// <summary>
        /// Gets the HTTP status code of the last HTTP call.
        /// </summary>
        /// <value>The HTTP status code.</value>
        public HttpStatusCode HttpStatusCode { get => httpStatusCode; set => httpStatusCode = value; }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        /// <value>The proxy.</value>
        public IWebProxy Proxy { get => proxy; set => proxy = value; }

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
        /// Calls the with get return stream.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>Stream.</returns>
        internal async Task<Stream> CallWithGetReturnStreamAsync(String url)
        {
            if (timeout != 0)
            {
                long nanoseconds = 1000000 * timeout;
                httpClient.Timeout = new TimeSpan(nanoseconds / 100);
            }

            HttpResponseMessage response = null;

            if (!UseCancellationToken)
            {
                response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            }
            else
            {
                response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            }

            this.httpStatusCode = response.StatusCode;

            // Allows a retry for 407, because can happen time to time with Web proxies 
            if (this.httpStatusCode.Equals(HttpStatusCode.ProxyAuthenticationRequired))
            {
                while(proxyAuthenticationCallCount < HttpRetryManager.ProxyAuthenticationCallLimit)
                {
                    proxyAuthenticationCallCount++;
                    Stream input = await CallWithGetReturnStreamAsync(url).ConfigureAwait(false);

                    if (!this.httpStatusCode.Equals(HttpStatusCode.ProxyAuthenticationRequired))
                    {
                        proxyAuthenticationCallCount = 0;
                        return input;
                    }
                }
            }

            HttpContent content = response.Content;

            return await content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a POST with parameters and returns a Stream
        /// </summary>
        /// <param name="theUrl">The Url.</param>
        /// <param name="parameters">The request parameters.</param>
        /// <returns>Stream.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// action is null!
        /// or
        /// postParameters is null!
        /// </exception>
        public async Task<Stream> CallWithPostAsync(Uri theUrl, Dictionary<string, string> parameters)
        {
            if (theUrl == null)
            {
                throw new ArgumentNullException("urlWithaction is null!");
            }

            if (parameters == null)
            {
                throw new ArgumentNullException("postParameters is null!");
            }

            if (timeout != 0)
            {
                long nanoseconds = 1000000 * timeout;
                httpClient.Timeout = new TimeSpan(nanoseconds / 100);
            }

            // This is the postdata
            var postData = new List<KeyValuePair<string, string>>();

            simpleTracer.Trace();
            simpleTracer.Trace("----------------------------------------");
            simpleTracer.Trace(theUrl.ToString());

            foreach (var param in parameters)
            {
                postData.Add(new KeyValuePair<string, string>(param.Key, param.Value));
                simpleTracer.Trace("param: " + param.Key + "/" + param.Value);
            }
            simpleTracer.Trace("----------------------------------------");

            HttpContent content = new FormUrlEncodedContent(postData);

            HttpResponseMessage response = null;

            simpleTracer.Trace("Before await httpClient.PostAsync(theUrl, content).ConfigureAwait(false) call");

            if (!UseCancellationToken)
            {
                response = await httpClient.PostAsync(theUrl, content).ConfigureAwait(false);
            }
            else
            {
                response = await httpClient.PostAsync(theUrl, content, cancellationToken).ConfigureAwait(false);
            }

            simpleTracer.Trace("After await httpClient.PostAsync(theUrl, content).ConfigureAwait(false) call");

            this.httpStatusCode = response.StatusCode;

            // Allows a retry for 407, because can happen time to time with Web proxies 
            if (this.httpStatusCode.Equals(HttpStatusCode.ProxyAuthenticationRequired))
            {
                simpleTracer.Trace("Before while (proxyAuthenticationCallCount < HttpRetryManager.ProxyAuthenticationCallLimit)");

                while (proxyAuthenticationCallCount < HttpRetryManager.ProxyAuthenticationCallLimit)
                {
                    proxyAuthenticationCallCount++;
                    Stream input = await CallWithPostAsync(theUrl, parameters).ConfigureAwait(false);

                    if (!this.httpStatusCode.Equals(HttpStatusCode.ProxyAuthenticationRequired))
                    {
                        proxyAuthenticationCallCount = 0;
                        return input;
                    }
                    simpleTracer.Trace("this.httpStatusCode: " + this.httpStatusCode);
                }
            }

            return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Calls the with get.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>String.</returns>
        /// <exception cref="System.ArgumentNullException">url is null!</exception>
        public async Task<string> CallWithGetAsync(String url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url is null!");
            }

            using (Stream stream = await CallWithGetReturnStreamAsync(url).ConfigureAwait(false))
            {
                if (stream == null)
                {
                    return null;
                }

                var responseString = new StreamReader(stream).ReadToEnd();

                simpleTracer.Trace();
                simpleTracer.Trace("----------------------------------------");
                simpleTracer.Trace(url);
                simpleTracer.Trace(responseString);
                simpleTracer.Trace("----------------------------------------");

                return responseString;
            }
        }

        /// <summary>
        /// Allowso the parent AceQLHttpApi to set the simple tracer.
        /// </summary>
        /// <param name="simpleTracer">The simple tracer.</param>
        internal void SetSimpleTracer(SimpleTracer simpleTracer)
        {
            this.simpleTracer = simpleTracer;
        }

        /// <summary>
        /// Executes a POST with parameters and returns a Srring
        /// </summary>
        /// <param name="theUrl">The Url.</param>
        /// <param name="parametersMap">The request parameters.</param>
        /// <returns>Stream.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// action is null!
        /// or
        /// postParameters is null!
        /// </exception>
        public async Task<string> CallWithPostAsyncReturnString(Uri theUrl, Dictionary<string, string> parametersMap)
        {
            String result = null;

            using (Stream input = await CallWithPostAsync(theUrl, parametersMap).ConfigureAwait(false))
            {
                if (input != null)
                {
                    result = new StreamReader(input).ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// Disposes the HttpClient 
        /// </summary>
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
