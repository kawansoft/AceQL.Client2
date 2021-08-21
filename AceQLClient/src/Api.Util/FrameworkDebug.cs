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
using System.IO;

namespace AceQL.Client.Api.Util
{
    /// <summary>
    /// Class FrameworkDebug.
    /// </summary>
    internal class FrameworkDebug
    {
        /** The file that contain the classes to debug in user.home */
        private static String KAWANSOFT_DEBUG_INI = "kawansoft-debug.ini";

        /** Stores the classes to debug */
        private static HashSet<String> CLASSES_TO_DEBUG = new HashSet<String>();

        /// <summary>
        /// Determines whether the specified class name is set.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns><c>true</c> if the specified class name is set; otherwise, <c>false</c>.</returns>
        internal static bool IsSet(string className)
        {
            load();
            return CLASSES_TO_DEBUG.Contains(className);
        }

        private static void load()
        {
            if (CLASSES_TO_DEBUG.Count > 0)
            {
                return;
            }

            String filePath = FileUtil2.GetUserFolderPath() + "/" + KAWANSOFT_DEBUG_INI;

            if (! File.Exists(filePath))
            {
                return;
            }

            using (Stream readStream = File.OpenRead(filePath))
            {
                StreamReader streamReader = new StreamReader(readStream);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if ( !line.StartsWith("#")) {
                        CLASSES_TO_DEBUG.Add(line.Trim());
                    }
                }
            }
        }
    }
}