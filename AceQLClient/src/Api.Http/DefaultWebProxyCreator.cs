using AceQL.Client.Api;
using System;
using System.Collections.Generic;
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

using System.Net;


namespace AceQL.Client.Api.Http
{

    /// <summary>
    /// Class DefaultWebProxyCreator. Allows to get the <see cref="System.Net.WebRequest.DefaultWebProxy"/> or <c>System.Net.WebRequest.GetSystemProxy()</c> proxy value to use. 
    /// </summary>
    internal static class DefaultWebProxyCreator
    {

        /// <summary>
        /// Gets the  the Default Or System proxy in use. Will return null if no Default/System proxy is in use.
        /// </summary>
        /// <returns>System.Net.IWebProxy.</returns>
        public static IWebProxy GetWebProxy()
        {
            IWebProxy webProxy = null;

            // See if end user has forced to use a System.Net.WebRequest.GetSystemWebProxy()
            if (AceQLConnection.GetDefaultWebProxy() != null)
            {
                webProxy = AceQLConnection.GetDefaultWebProxy();
            }
            else
            {
                webProxy = System.Net.WebRequest.DefaultWebProxy;
            }
          
            // Test the secret URL, if it is bypassed, there is no Default/System proxy set, so we will return null:
            if (webProxy.IsBypassed(new Uri(HttpClientHandlerBuilderNew.SECRET_URL))) {
                return null;
            }
            else
            {
                return webProxy;
            }
        }
    }
}
