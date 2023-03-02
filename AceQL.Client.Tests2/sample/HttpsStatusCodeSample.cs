/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2023,  KawanSoft SAS
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

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AceQL.Client.Tests2.sample
{
    public class HttpsStatusCodeSample
    {

        public static void TheMain(string[] args)
        {
            var httpsStatusMessage = (HttpStatusCode)403; // int to enum conversion
            Console.WriteLine(httpsStatusMessage);//output: Saturday 
            Console.WriteLine();

            HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
            if (httpStatusCode != HttpStatusCode.OK)
            {
                string theErrorMessage = "HTTP FAILURE " + (int)httpStatusCode + " (" + httpStatusCode + ")";
                Console.WriteLine(theErrorMessage);
            }
        }
    }
}
