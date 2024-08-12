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

using AceQL.Client.Api;
using AceQL.Client.Api.Metadata;
using AceQL.Client.Api.Metadata.Dto;
using AceQL.Client.Sample;
using AceQL.Client.test.Dml;
using AceQL.Client.Test.Connection;
using AceQL.Client.Test.Dml.Batch;
using AceQL.Client.Test.Executor;
using AceQL.Client.Test.HealthChecks.Test;
using AceQL.Client.Test.Json;
using AceQL.Client.Test.Metadata;
using AceQL.Client.Test.Metadata.misc;
using AceQL.Client.Test.StoredProcedure;
using AceQL.Client.Test.test.misc;
using AceQL.Client.Test.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test
{
    public static class MainStart
    {

        public static void TestAll(string[] args)
        {
            HealthCheckTest.TheMain(args);
            AceQLTestHeaders.TheMain();
            DmlSequenceTest.TheMain(args);
            SqlBatchTest.TheMain(args);
            AceQLTestColumnAsKeyName.TheMain(args);
            SqlServerStoredProcedureTestUtf8.TheMain(args);
            OracleStoredProcedureTest.TheMain(args);
            AceQLTestMetadata.TheMain();
        }


        public static void Main(string[] args)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            folderPath += "\\.kawansoft";

            int mainToLaunch = 0;

            String filepath = folderPath + "\\aceql-csharp-test.txt";

            if (File.Exists(filepath))
            {
                Console.WriteLine("Using mainToLaunch in: " + filepath);
                String mainToLaunchStr = File.ReadAllText(filepath);
                mainToLaunchStr = mainToLaunchStr.Trim();
                mainToLaunch = int.Parse(mainToLaunchStr);
            }

            Console.WriteLine("mainToLaunch: " + mainToLaunch);

            if (mainToLaunch == 0)
            {
                TestAll(args);
            }
            else if (mainToLaunch == 1)
            {
                DmlSequenceTest.TheMain(args);
            }
            else if (mainToLaunch == 2)
            {
                AceQLTestColumnAsKeyName.TheMain(args);
            }
            else if (mainToLaunch == 3)
            {
                JsonTest.TheMain(args);
            }
            else if (mainToLaunch == 4)
            {
                MySqlStoredProcedureTest.TheMain(args);
            }
            else if (mainToLaunch == 5)
            {
                SqlServerStoredProcedureTest.TheMain(args);
            }
            else if (mainToLaunch == 6)
            {
                AceQLTestHeaders.TheMain();
                AceQLTestMetadata.TheMain();
            }
            else if (mainToLaunch == 7)
            {
                AceQLTestSessionId.TheMain();
                AceQLTestMetadata.TheMain();
            }
            else if (mainToLaunch == 8)
            {
                AceQLTestNoClose.TheMain(args);
            }
            else if (mainToLaunch == 9)
            {
                SqlServerStoredProcedureTestUtf8.TheMain(args);
            }
            else if (mainToLaunch == 10)
            {
                SqlBatchTest.TheMain(args);
            }
            else if (mainToLaunch == 11)
            {
                SqlBatchSample.TheMain(args);
            }
            else if (mainToLaunch == 12)
            {
                ServerQueryExecuteTest.TheMain(args);
            }
            else if (mainToLaunch == 13)
            {
                TypesDisplayUtil.TestCSharpTypes();
            }
            else if (mainToLaunch == 14)
            {
                SqlServerTest sqlServerTest = new SqlServerTest();
                sqlServerTest.ExecuteSomeSql();
            }
            else if (mainToLaunch == 15)
            {
                HealthCheckTest.TheMain(args);
            }
            else if (mainToLaunch == 16)
            {
                OracleStoredProcedureTest.TheMain(args);
            }
            else if (mainToLaunch == 17)
            {
                //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //Encoding.GetEncoding(932);
                //Console.OutputEncoding = System.Text.Encoding.UTF8;
                //SpecialSequenceTest.TheMain(args);
                SqlBatchTest.TheMain(args);
            }
        }

    }
}
