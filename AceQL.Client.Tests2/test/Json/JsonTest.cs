﻿/*
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

using AceQL.Client.Api.Util;
using AceQL.Client.Test.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Json
{
    static class JsonTest
    {
        public static void TheMain(string[] args)
        {
          
            try
            {
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

        public static void JsonParseResultAsFile()
        {
            Dictionary<int, string> valuesPerParamIndex = new Dictionary<int, string>();
            Dictionary<string, string> valuesPerParamName = new Dictionary<string, string>();

            Stream stream =  File.OpenRead(@"C:\test\json_out.txt");

            TextReader textReader = new StreamReader(stream);
            var reader = new JsonTextReader(textReader);

            // Necessary because a SQL columns could have the name "row_count".
            // We know that we are reading the good end of file "row_count" if
            // We are not any more in a array
            bool isInsideArray = false;

            while (reader.Read())
            {

                if (reader.TokenType != JsonToken.PropertyName || !reader.Value.Equals("parameters_out_per_index") || isInsideArray)
                {
                    continue;
                }

                String typeParameter = "per_index";

                string paramName = null;
                string paramValue = null;

                while (reader.Read())
                {
                    if (reader.Value != null && reader.TokenType == JsonToken.PropertyName && reader.Value.Equals("parameters_out_per_name"))
                    {
                        typeParameter = "per_name";
                    }

                    if ( reader.TokenType == JsonToken.PropertyName)
                    {
                        paramName = reader.Value.ToString();
                        paramName = paramName.Trim();
                        AceQLConsole.WriteLine("property: " + paramName + ":");
                    }
                    else if(reader.TokenType == JsonToken.String)
                    {
   
                        paramValue = reader.Value.ToString();
                        AceQLConsole.WriteLine("paramIndex: " + paramName + ":");
                        AceQLConsole.WriteLine("value     : " + paramValue + ":");
                        if (paramValue.Equals("NULL"))
                        {
                            paramValue = null;
                        }

                        if (typeParameter.Equals("per_index")) {
                            int paramIndexInt = Int32.Parse(paramName);
                            valuesPerParamIndex.Add(paramIndexInt, paramValue);
                        }
                        else
                        {
                            valuesPerParamName.Add(paramName, paramValue);
                        }

                    }
                       
                }
 
            }

            AceQLConsole.WriteLine();
            AceQLConsole.WriteLine("------- dict display ------");
            AceQLConsole.WriteLine();

            foreach (KeyValuePair<int, string> outParameter in valuesPerParamIndex)
            {
                AceQLConsole.WriteLine(outParameter.Key + " / " + outParameter.Value);
            }

            AceQLConsole.WriteLine();

            foreach (KeyValuePair<string, string> outParameter in valuesPerParamName)
            {
                AceQLConsole.WriteLine(outParameter.Key + " / " + outParameter.Value);
            }

            stream.Close();

        }

        public static void JsonParseResultAsString()
        {
            string jsonResult = File.ReadAllText(@"C:\test\json_out.txt");
            AceQLConsole.WriteLine(jsonResult);
            AceQLConsole.WriteLine();

            dynamic xj = JsonConvert.DeserializeObject(jsonResult);
            dynamic xjParametersOutPername = xj.parameters_out_per_name;

            if (xjParametersOutPername != null)
            {
                String dictStr = xjParametersOutPername.ToString();
                AceQLConsole.WriteLine("dictStr:" + dictStr);
                AceQLConsole.WriteLine();

                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(dictStr);

                // For each parameter 1) get the index 2) get the dbType
                foreach (KeyValuePair<String, string> theParameter in dict)
                {
                    AceQLConsole.WriteLine(theParameter.Key + " / " + theParameter.Value);
                }
            }
            else
            {
                AceQLConsole.WriteLine("No Out parameters per name");
            }
        }
    }
}
