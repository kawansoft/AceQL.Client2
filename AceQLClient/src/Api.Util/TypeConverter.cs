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

using System;
using System.Collections.Generic;
using System.Text;

namespace AceQL.Client.Api.Util
{


    /// <summary>
    /// Class TypeConverter.
    /// </summary>
    internal class TypeConverter
    {
        private readonly Type csharpType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeConverter"/> class.
        /// </summary>
        /// <param name="csharpType">CSharp Type.</param>
        /// <exception cref="ArgumentNullException">csharpType is null!</exception>
        public TypeConverter(Type csharpType)
        {
            this.csharpType = csharpType ?? throw new ArgumentNullException("csharpType is null!");
        }

        /// <summary>
        /// Gets the name of the java type.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetJavaTypeName()
        {
            /*
           bool    : Boolean
           short   : Int16
           int     : Int32
           float   : Single
           double  : Double
           long    : Int64
           String  : String
           string  : String
           DateTime: DateTime 132837806962201774
           */

            String name = csharpType.Name;

            if (name.Equals("Boolean", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Boolean";
            }
            else if (name.Equals("Int16", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Short";
            }
            else if (name.Equals("Int32", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Integer";
            }
            else if (name.Equals("Single", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Float";
            }
            else if (name.Equals("Double", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Double";
            }
            else if (name.Equals("String", StringComparison.InvariantCultureIgnoreCase))
            {
                return "String";
            }
            else if (name.Equals("DateTime", StringComparison.InvariantCultureIgnoreCase))
            {
                return "Timestamp";
            }
            else
            {
                throw new NotSupportedException("CSharp parameter type is not supported in this version: " + name);
            }

        }
    }
}
