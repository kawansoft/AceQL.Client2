/*
 * This file is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2022,  KawanSoft SAS
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

using AceQL.Client.Api;
using AceQL.Client.Api.Http;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Dml;
using AceQL.Client.Test.Util;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AceQL.Client.Test.HealthChecks.Test
{
    /// <summary>
    /// Tests the HealtCheck class.
    /// </summary>
    public static class HealthCheckTest
    {
        public static void TheMain(string[] args)
        {
            try
            {
                DoIt().Wait();

                AceQLConsole.WriteLine();
                AceQLConsole.WriteLine("Press enter to continue....");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                AceQLConsole.WriteLine(exception.ToString());
                AceQLConsole.WriteLine(exception.StackTrace);
                AceQLConsole.WriteLine("Press enter to continue...");
                Console.ReadLine();
            }
        }

        static async Task DoIt()
        {
            var netCoreVer = System.Environment.Version; // 3.0.0
            AceQLConsole.WriteLine(netCoreVer + "");

            using (AceQLConnection connection = await ConnectionCreator.ConnectionCreateAsync().ConfigureAwait(false))
            {
                await ExecuteExample(connection).ConfigureAwait(false);
                //NOT Neccessary: await connection.CloseAsync(); 
            }
            
        }

        /// <summary>
        /// Executes our example using an <see cref="AceQLConnection"/> 
        /// </summary>
        /// <param name="connection"></param>
        public static async Task ExecuteExample(AceQLConnection connection)
        {
            await connection.OpenAsync();

            AceQLConsole.WriteLine(AceQLConnection.GetAceQLLocalFolder());

            HealthCheck healthCheck = new HealthCheck(connection);
            AceQLConsole.WriteLine("healthCheck.PingAsync()           : " + await healthCheck.PingAsync().ConfigureAwait(false));
            AceQLConsole.WriteLine("healthCheck.GetResponseTimeAsync(): " + await healthCheck.GetResponseTimeAsync().ConfigureAwait(false));
        }
    }
}
