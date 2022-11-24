/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Api.Http
{
    internal class AceQLHealthCheckInfoApi
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

        public AceQLHealthCheckInfoApi(HttpManager httpManager, string url, SimpleTracer simpleTracer)
        {
            this.httpManager = httpManager;
            this.url = url;
            this.simpleTracer = simpleTracer;
        }

        /// <summary>
        /// Gets the HealthCheck DTO
        /// </summary>
        /// <returns>HealthCheckInfoDto.</returns>
        /// <exception cref="AceQLException">
        /// 0
        /// </exception>
        internal async Task<HealthCheckInfoDto> GetHealthCheckInfoDtoAsync()
        {
            try
            {
                String commandName = "health_check_info";
                String result = await CallWithGetAsync(commandName, null).ConfigureAwait(false);

                ResultAnalyzer resultAnalyzer = new ResultAnalyzer(result, HttpStatusCode);
                if (!resultAnalyzer.IsStatusOk())
                {
                    throw new AceQLException(resultAnalyzer.GetErrorMessage(),
                        resultAnalyzer.GetErrorId(),
                        resultAnalyzer.GetStackTrace(),
                        HttpStatusCode);
                }

                HealthCheckInfoDto healthCheckInfoDto = JsonConvert.DeserializeObject<HealthCheckInfoDto>(result);

                return healthCheckInfoDto;
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
