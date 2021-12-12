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


using AceQL.Client.Api.Util;
using System;
using System.Collections.Generic;

namespace AceQL.Client.Api.Metadata.Dto
{
    /// <summary>
    /// Class ServerQueryExecutorDtoBuilder. Builds a ServerQueryExecutorDto using the queryExecutorClassName and its parameters
    /// </summary>
    internal static class ServerQueryExecutorDtoBuilder
    {
        /// <summary>
        /// Builds the specified server query executor class name.
        /// </summary>
        /// <param name="serverQueryExecutorClassName">Name of the server AceQL query executor class.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>ServerQueryExecutorDto.</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static ServerQueryExecutorDto Build(string serverQueryExecutorClassName, List<object> parameters)
        {
            // Build the params types
            List<string> paramsTypes = new List<string>();

            // Build the params values
            List<string> paramsValues = new List<string>();

            foreach (object parameter in parameters)
            {
                TypeConverter typeConverter = new TypeConverter(parameter.GetType());
                paramsTypes.Add(typeConverter.GetJavaTypeName());
                paramsValues.Add(parameter.ToString());
            }

            ServerQueryExecutorDto serverQueryExecutorDto = new ServerQueryExecutorDto(serverQueryExecutorClassName, paramsTypes, paramsValues);
            return serverQueryExecutorDto;

        }
    }
}