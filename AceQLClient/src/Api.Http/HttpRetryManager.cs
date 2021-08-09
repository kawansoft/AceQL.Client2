﻿/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    /// <summary>
    /// Class HttpRetryManager. Allows to define retry options. Fist implementation is for 407 retry. 
    /// </summary>
    internal static class HttpRetryManager
    {
        /// <summary>
        /// The proxy authentication call limit. Defaults to one retry.
        /// </summary>
        private static int proxyAuthenticationCallLimit = 1;

        /// <summary>
        /// Gets or sets the proxy authentication call limit. This is the limit of retry when an HTTP call return 407
        /// </summary>
        /// <value>The proxy authentication call limit.</value>
        public static int ProxyAuthenticationCallLimit { get => proxyAuthenticationCallLimit; set => proxyAuthenticationCallLimit = value; }
    }
}
