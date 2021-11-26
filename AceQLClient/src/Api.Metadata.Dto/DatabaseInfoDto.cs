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
namespace AceQL.Client.Api.Metadata.Dto
{
    /// <summary>
    /// Class DatabaseInfoDto.
    /// </summary>
    internal class DatabaseInfoDto
    {
        /// <summary>
        /// The status
        /// </summary>
        private readonly string status = "OK";

        /// <summary>
        /// The database information
        /// </summary>
        private readonly DatabaseInfo databaseInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseInfoDto"/> class.
        /// </summary>
        /// <param name="databaseInfo">The JDBC database meta data.</param>
        public DatabaseInfoDto(DatabaseInfo databaseInfo)
        {
            this.databaseInfo = databaseInfo;
        }

        /// <summary>
        /// Gets the database information.
        /// </summary>
        /// <value>The database information.</value>
        public DatabaseInfo DatabaseInfo { get => databaseInfo; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "DatabaseInfoDto [status=" + status + ", databaseInfo=" + databaseInfo + "]";
        }
    }
}
