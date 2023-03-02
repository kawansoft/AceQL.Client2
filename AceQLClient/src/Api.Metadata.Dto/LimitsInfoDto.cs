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
using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Metadata.Dto
{
    /// <summary>
    /// Class LimitsInfoDto.
    /// </summary>
    internal class LimitsInfoDto
    {
        private string status = "OK";
        private long maxRows = 0;
        private long maxBlobLength = 0;

        public string Status { get => status; set => status = value; }
        public long MaxRows { get => maxRows; set => maxRows = value; }
        public long MaxBlobLength { get => maxBlobLength; set => maxBlobLength = value; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "LimitsInfoDto [status=" + status + ", maxRows=" + maxRows + ", maxBlobLength=" + maxBlobLength + "]";
        }

    }
}
