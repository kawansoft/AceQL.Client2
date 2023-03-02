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

        private int datatabaseMajorVersion;
        private int databaseMinorVersion;
        private string databaseProductName;
        private string databaseProductVersion;
        private int driverMajorVersion;
        private int driverMinorVersion;
        private string driverName;
        private string driverVersion;

        public int DatatabaseMajorVersion { get => datatabaseMajorVersion; set => datatabaseMajorVersion = value; }
        public int DatabaseMinorVersion { get => databaseMinorVersion; set => databaseMinorVersion = value; }
        public string DatabaseProductName { get => databaseProductName; set => databaseProductName = value; }
        public string DatabaseProductVersion { get => databaseProductVersion; set => databaseProductVersion = value; }
        public int DriverMajorVersion { get => driverMajorVersion; set => driverMajorVersion = value; }
        public int DriverMinorVersion { get => driverMinorVersion; set => driverMinorVersion = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public string DriverVersion { get => driverVersion; set => driverVersion = value; }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "DatabaseInfoDto [status=" + status +  ", datatabaseMajorVersion=" + DatatabaseMajorVersion + ", databaseMinorVersion=" + DatabaseMinorVersion + ", databaseProductName=" + DatabaseProductName + ", databaseProductVersion=" + DatabaseProductVersion + ", driverMajorVersion=" + DriverMajorVersion + ", driverMinorVersion=" + DriverMinorVersion + ", driverName=" + DriverName + ", driverVersion=" + DriverVersion + "]";
        }
    }
}
