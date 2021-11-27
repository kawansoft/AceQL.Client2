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
    internal static class AceQLConnectionUtil
    {
        internal static readonly string BATCH_MIN_SERVER_VERSION = "8.0";
        internal static readonly string GET_DATABASE_INFO_MIN_SERVER_VERSION = "9.0";

        private static string SERVER_VERSION_NUMBER;

   

        /// <summary>
        /// Says if the current version is OK foe a feature execution.</summary>
        /// <param name="rawServerVersion">		the current server version </param>
        /// <param name="minServerVersion">		the minimum version for feature execution </param>
        /// <returns> true if rawServerVersion is OK for execution </returns>
   
        public static bool IsCurrentVersionOk(string rawServerVersion, string minServerVersion)
        {
            // Because of US Culture
            rawServerVersion = rawServerVersion.Replace(".", ",");
            minServerVersion = minServerVersion.Replace(".", ",");

            double rawServerVersionDouble = Convert.ToDouble(rawServerVersion);
            double minServerVersionDouble = Convert.ToDouble(minServerVersion);
            return rawServerVersionDouble >= minServerVersionDouble;
        }


        /// <summary>
        /// Extracts the raw server version.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>Task&lt;System.String&gt;.</returns>
        public static async Task<string> ExtractRawServerVersion(AceQLConnection connection)
        {
            if (SERVER_VERSION_NUMBER == null)
            {
                SERVER_VERSION_NUMBER = await connection.GetServerVersionAsync();
            }
            
            String serverVersionNumber = SERVER_VERSION_NUMBER;
            String[] stringArray = serverVersionNumber.Split('v');
            String[] stringArrayFinal = stringArray[1].Split('-');
            String versionRaw = stringArrayFinal[0].Trim();
            return versionRaw;
        }

        /// <summary>
        /// Determines whether if batch commands are supported on server side.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal static async Task<bool> IsBatchSupported(AceQLConnection connection)
        {
            String rawServerVersion = await ExtractRawServerVersion(connection).ConfigureAwait(false);
            return IsCurrentVersionOk(rawServerVersion, BATCH_MIN_SERVER_VERSION);
        }

        /// <summary>
        /// Determines whether if GetDatabaseInfo() is supported on server side.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <returns>Task&lt;System.Boolean&gt;.</returns>
        internal static async Task<bool> IsGetDatabaseInfoSupported(AceQLConnection connection)
        {
            String rawServerVersion = await ExtractRawServerVersion(connection).ConfigureAwait(false);
            return IsCurrentVersionOk(rawServerVersion, GET_DATABASE_INFO_MIN_SERVER_VERSION);
        }
    }
}