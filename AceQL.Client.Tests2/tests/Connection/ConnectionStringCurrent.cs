/*
 * This filePath is part of AceQL C# Client SDK.
 * AceQL C# Client SDK: Remote SQL access over HTTP with AceQL HTTP.                                 
 * Copyright (C) 2020,  KawanSoft SAS
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

using AceQL.Client.Tests.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Tests.Test.Connection
{
    public static class ConnectionStringCurrent
    {

        public static Boolean useLocal;
        public static Boolean useLdapAuth;
        public static int typeAuthenticatedProxy;

        public static string Build()
        {
            String filePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\aceql.client.ini";
            PropFileReader propFileReader = new PropFileReader(filePath);

            useLocal = Boolean.Parse(propFileReader.getProperty("useLocal"));
            useLdapAuth = Boolean.Parse(propFileReader.getProperty("useLdapAuth"));
            typeAuthenticatedProxy = int.Parse(propFileReader.getProperty("typeAuthenticatedProxy"));

            AceQLConsole.WriteLine("useLocal              : " + useLocal);
            AceQLConsole.WriteLine("useLdapAuth           : " + useLdapAuth);
            AceQLConsole.WriteLine("typeAuthenticatedProxy: " + typeAuthenticatedProxy);
            AceQLConsole.WriteLine();

            String connectionString = null;

            if (useLocal)
            {
                if (useLdapAuth)
                {
                    connectionString = ConnectionStringBuilderFactory.CreateDefaultLocalLdapAuth();
                }
                else
                {
                    connectionString = ConnectionStringBuilderFactory.CreateDefaultLocal();
                }
            }
            else
            {
                if (useLdapAuth)
                {
                    connectionString = ConnectionStringBuilderFactory.CreateDefaultRemoteLdapAuth(typeAuthenticatedProxy);
                }
                else
                {
                    connectionString = ConnectionStringBuilderFactory.CreateDefaultRemote(typeAuthenticatedProxy);
                }
            }

            AceQLConsole.WriteLine("connectionString: " + connectionString);
            return connectionString;
        }

    }
}
