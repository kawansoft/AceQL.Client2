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
﻿// ***********************************************************************

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Util
{
    /// <summary>
    /// Class <see cref="StreamResultAnalyzer"/>. Allows to analyze the result of a downloaded result of a SQL query stored in a local PC filePath.
    /// </summary>
    internal class StreamResultAnalyzer
    {
        /// <summary>
        /// The error identifier
        /// </summary>
        private string errorType;
        /// <summary>
        /// The error message
        /// </summary>
        private string errorMessage;
        /// <summary>
        /// The stack trace
        /// </summary>
        private string stackTrace;

        private readonly HttpStatusCode httpStatusCode;

        // The JSON filePath containing Result Set
        private readonly string filePath;


        /// <summary>
        /// Initializes a new instance of the <see cref="StreamResultAnalyzer"/> class.
        /// </summary>
        /// <param name="filePath">The filePath to analyze.</param>
        /// <param name="httpStatusCode">The http status code.</param>
        /// <exception cref="System.ArgumentNullException">The filePath is null.</exception>
        public StreamResultAnalyzer(string filePath, HttpStatusCode httpStatusCode)
        {
            this.filePath = filePath ?? throw new ArgumentNullException("filePath is null!");
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Determines whether the SQL correctly executed on server side.
        /// </summary>
        /// <returns><c>true</c> if [is status ok]; otherwise, <c>false</c>.</returns>
        internal bool IsStatusOK()
        {
            if (!File.Exists(filePath))
            {
                this.errorType = "0";
                errorMessage = "Unknown error.";
                if (httpStatusCode != HttpStatusCode.OK)
                {
                    errorMessage = "HTTP FAILURE " + (int)httpStatusCode + " (" + httpStatusCode + ")";
                }
                return false;
            }

            using (Stream stream = File.OpenRead(filePath))
            {
                TextReader textReader = new StreamReader(stream);
                var reader = new JsonTextReader(textReader);

                while (reader.Read())
                {
                    if (reader.Value == null)
                    {
                        continue;
                    }

                    if (reader.TokenType != JsonToken.PropertyName || !reader.Value.Equals("status"))
                    {
                        continue;
                    }

                    if (!reader.Read())
                    {
                        return false;
                    }

                    if (reader.Value.Equals("OK"))
                    {
                        return true;
                    }
                    else
                    {
                        ParseErrorKeywords(reader);
                        return false;
                    }

                }

            }

            return false;
        }

        /// <summary>
        /// Parses the error keywords.
        /// </summary>
        /// <param name="reader">The reader.</param>
        private void ParseErrorKeywords(JsonTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Value == null)
                {
                    continue;
                }

                if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("error_type"))
                {
                    if (reader.Read())
                    {
                        this.errorType = reader.Value.ToString();
                    }
                    else
                    {
                        return;
                    }
                }

                if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("error_message"))
                {
                    if (reader.Read())
                    {
                        this.errorMessage = reader.Value.ToString();
                    }
                    else
                    {
                        return;
                    }
                }

                if (reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("stack_trace"))
                {
                    if (reader.Read())
                    {
                        this.stackTrace = (string)reader.Value;
                    }
                    else
                    {
                        return;
                    }
                }

            }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <returns>The error message</returns>
        internal string GetErrorMessage()
        {
            return this.errorMessage;
        }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        /// <returns>The error type.</returns>
        internal int GetErrorType()
        {
            return Int32.Parse(this.errorType);
        }

        /// <summary>
        /// Gets the remote stack trace.
        /// </summary>
        /// <returns>The remote stack trace.</returns>
        internal string GetStackTrace()
        {
            return this.stackTrace;
        }
    }
}
