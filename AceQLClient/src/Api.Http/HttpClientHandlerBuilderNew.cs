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

using AceQL.Client.Api.Http;
using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    /// <summary>
    /// Class HttpClientHandlerBuilder. Allows to build an HttpClientHandler.
    /// </summary>
    static internal class HttpClientHandlerBuilderNew
    {
        internal static bool DEBUG = FrameworkDebug.IsSet("HttpClientHandlerBuilderNew");

        internal static readonly String SECRET_URL = "http://secret.aceql.com";

        /// <summary>
        /// Builds an HttpClientHandler instance with proxy settings, if necessary. IWebProxy used is System.Net.WebRequest.DefaultWebProxy
        /// </summary>
        /// <param name="proxyUri">The URI of the web proxy to use.</param>
        /// <param name="credentials">The credentials to use for an authenticated proxy. null if none.</param>
        /// <param name="enableDefaultSystemAuthentication">if True ==> call HttpClientHandler.UseDefaultCredentials = true</param>
        /// <returns>The HtpClientHandler.</returns>
        internal static HttpClientHandler Build(string proxyUri, ICredentials credentials, bool enableDefaultSystemAuthentication)
        {
            Debug("httpClientHandler.UseDefaultCredentials: "  + enableDefaultSystemAuthentication);

            IWebProxy webProxy = null;
            HttpClientHandlerCreator httpClientHandlerCreator = null;

            if (proxyUri == null)
            {
                // Detect the System.Net.WebRequest.DefaultWebProxy or WebRequest.GetSystemWebProxy() in use. 
                // We will get null if no Default/System proxy is configured.
                // We use the webproxy credentials if they are set / not null:
                webProxy = DefaultWebProxyCreator.GetWebProxy();
               
                if (webProxy == null || webProxy.Credentials == null)
                {
                    Debug("webProxy or webProxy.Credentials is NULL!");
                    httpClientHandlerCreator = new HttpClientHandlerCreator(webProxy, credentials, enableDefaultSystemAuthentication);
                }
                else
                {
                    // Use the Credentials of the Web Proxy if webProxy.Credentials  is not null
                    httpClientHandlerCreator = new HttpClientHandlerCreator(webProxy, webProxy.Credentials, enableDefaultSystemAuthentication);
                }

                
            }
            else
            {
                Uri uri = new Uri(proxyUri);
                webProxy = new UriWebProxy(uri);

                // Creates the HttpClientHandler, with or without an associated IWebProxy
                httpClientHandlerCreator = new HttpClientHandlerCreator(webProxy, credentials, enableDefaultSystemAuthentication);
            }

            HttpClientHandler httpClientHandler = httpClientHandlerCreator.GetHttpClientHandler();
            return httpClientHandler;


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
