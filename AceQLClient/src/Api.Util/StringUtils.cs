﻿/*
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace AceQL.Client.Src.Api.Util
{
    /// <summary>
    /// Class StringUtils. Utilities fro String management.
    /// </summary>
    internal static class StringUtils
    {
        /// <summary>
        /// Gets the substring before the first occurrence of a separator. The separator is not returned.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>System.String.</returns>
        internal static string SubstringBefore(string str, string separator)
        {

            if (str == null || str.Length == 0)
            {
                return str;
            }

            int commaIndex = str.IndexOf(separator, StringComparison.CurrentCulture);

            if (commaIndex <= 0)
            {
                return str;
            }
            else
            {
                return str.Substring(0, commaIndex);
            }

        }
    }
}
