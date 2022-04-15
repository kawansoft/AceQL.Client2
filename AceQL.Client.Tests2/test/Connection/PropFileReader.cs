/*
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceQL.Client.Test.Connection
{
    /// <summary>
    /// Class PropFileReader. Allows to read properties from a INI or .properties file, as we would do in Java...
    /// </summary>
    public class PropFileReader
    {
        private Dictionary<string, string> properties;
        private readonly String filePath;

        public PropFileReader(string filePath)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath is null!");
            }

            if (!File.Exists(filePath))
            {
                throw new ArgumentNullException("file does not exist: " + filePath);
            }

            this.filePath = filePath;
            Load();

        }

        /// <summary>
        /// Gets the property from the file, like we would do in Java...
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>the property value for the key.</returns>
        /// <exception cref="ArgumentNullException">key is null!</exception>
        public string getProperty(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key is null!");
            }

            return properties[key];
        }

        /// <summary>
        /// Loads all "properties" of the Java Properties like fie in emoery into a Dictionnary
        /// </summary>
        private void Load()
        {
            properties = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(filePath))
            {
                if (row.Length < 1)
                {
                    continue;
                }
                properties.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            }
        }
    }
}
