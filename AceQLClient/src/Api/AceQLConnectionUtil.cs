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


using System;
using System.Threading.Tasks;

namespace AceQL.Client.Api
{
    internal class AceQLConnectionUtil
    {
        internal static readonly string BATCH_MIN_SERVER_VERSION = "8.0";
        private static string SERVER_VERSION_NUMBER;

        /// <summary>
        /// Determines whether [is batch supported] [the specified connection].
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        /// <exception cref="NotImplementedException"></exception>
        internal static async Task<bool>  IsBatchSupported(AceQLConnection connection)
        {
            if (SERVER_VERSION_NUMBER == null)
            {
                SERVER_VERSION_NUMBER = await connection.GetServerVersionAsync();
            }

            String rawServerVersion = ExtractRawServerVersion(SERVER_VERSION_NUMBER);
            return rawServerVersion.CompareTo(BATCH_MIN_SERVER_VERSION) >= 0;
        }

        /// <summary>
        /// Extracts the raw server version.
        /// </summary>
        /// <param name="serverVersionNumber">The server version number.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static string ExtractRawServerVersion(string serverVersionNumber)
        {
            String[] stringArray = serverVersionNumber.Split('v');
            String[] stringArrayFinal = stringArray[1].Split('-');
            String versionRaw = stringArrayFinal[0].Trim();
            return versionRaw;
        }
    }
}